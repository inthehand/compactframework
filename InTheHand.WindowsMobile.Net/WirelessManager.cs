// In The Hand - .NET Components for Mobility
//
// InTheHand.WindowsMobile.Net.WirelessManager
// 
// Copyright (c) 2003-2010 In The Hand Ltd, All rights reserved.

using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Runtime.InteropServices;
using InTheHand.Runtime.InteropServices;

namespace InTheHand.WindowsMobile.Net
{
	/// <summary>
	/// Provides access to the wireless devices to control their states.
	/// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
    /// </list>
    /// </remarks>
	public sealed class WirelessManager : CollectionBase
	{
        /// <summary>
        /// Initializes a new instance of WirelessManager.
        /// </summary>
        public WirelessManager()
        {
            Refresh();
        }

        /// <summary>
        /// Refreshes the wireless device information.
        /// </summary>
        public void Refresh()
        {
            IntPtr devList;
            int result = GetWirelessDevices(out devList, 0);
            IntPtr pThis = devList;

            while (pThis != IntPtr.Zero)
            {
                RDD r = (RDD)Marshal.PtrToStructure(pThis, typeof(RDD));
                WirelessDevice wd = new WirelessDevice(this, r);
                this.InnerList.Add(wd);
                switch (r.DeviceType)
                {
                    case WirelessDeviceType.Bluetooth:
                        bluetooth = wd;
                        break;
                    case WirelessDeviceType.Phone:
                        phone = wd;
                        break;
                    case WirelessDeviceType.WiFi:
                        wifi = wd;
                        break;
                }

                pThis = r.pNext;
            }

            result = FreeDeviceList(devList);
        }

        internal int SetWirelessState(WirelessDeviceType type, WirelessState state)
        {
            int stateResult = -1;
            IntPtr devList;
            int result = GetWirelessDevices(out devList, 0);
            IntPtr pThis = devList;

            while (pThis != IntPtr.Zero)
            {
                RDD r = (RDD)Marshal.PtrToStructure(pThis, typeof(RDD));

                if (r.DeviceType == type)
                {
                    stateResult = ChangeRadioState(pThis, state, SAVEACTION.RADIODEVICES_PRE_SAVE);
                }

                pThis = r.pNext;
            }

            result = FreeDeviceList(devList);
            return stateResult;
        }

        internal void SetAll(WirelessState state)
        {
            IntPtr devList;
            int result = GetWirelessDevices(out devList, 0);
            IntPtr pThis = devList;

            while (pThis != IntPtr.Zero)
            {
                RDD r = (RDD)Marshal.PtrToStructure(pThis, typeof(RDD));

                ChangeRadioState(pThis, state, SAVEACTION.RADIODEVICES_PRE_SAVE);
                
                pThis = r.pNext;
            }
            result = FreeDeviceList(devList);
        }

        /// <summary>
        /// Returns the WirelessDevice at the specified index.
        /// </summary>
        /// <param name="index">zero-based index of a wireless device.</param>
        /// <returns>A WirelessDevice instance for the individual radio device.</returns>
        public WirelessDevice this[int index]
        {
            get
            {
                return (WirelessDevice)base.InnerList[index];
            }
        }

        private WirelessDevice bluetooth;
        /// <summary>
        /// Returns the WirelessDevice for Bluetooth (if present).
        /// </summary>
        public WirelessDevice Bluetooth
        {
            get
            {
                return bluetooth;
            }
        }

        private WirelessDevice phone;
        /// <summary>
        /// Returns the WirelessDevice for the Phone (if present).
        /// </summary>
        public WirelessDevice Phone
        {
            get
            {
                return phone;
            }
        }

        private WirelessDevice wifi;
        /// <summary>
        /// Returns the WirelessDevice for WiFi (if present).
        /// </summary>
        public WirelessDevice WiFi
        {
            get
            {
                return wifi;
            }
        }

        /// <summary>
        /// Turns all attached wireless devices on.
        /// </summary>
        public void AllOn()
        {
            SetAll(WirelessState.On);
        }

        /// <summary>
        /// Turns all attached wireless devices off.
        /// </summary>
        public void AllOff()
        {
            SetAll(WirelessState.Off);
        }

        [DllImport("ossvcs.dll", EntryPoint="#276")]
        private static extern int GetWirelessDevices(out IntPtr pDevices, int dwFlags);

        [DllImport("ossvcs.dll", EntryPoint = "#273")]
        internal static extern int ChangeRadioState(IntPtr pDev, WirelessState dwState, SAVEACTION sa);

        [DllImport("ossvcs.dll", EntryPoint = "#280")]
        private static extern int FreeDeviceList(IntPtr pRoot);
    }

    /// <summary>
    /// Specifies wireless device types.
    /// </summary>
    public enum WirelessDeviceType
    {
        /// <summary>
        /// Device is a Wireless LAN adapter.
        /// </summary>
        WiFi = 1,
        /// <summary>
        /// Device is a phone (GSM or CDMA) device.
        /// </summary>
        Phone,
        /// <summary>
        /// Device is a Bluetooth adapter.
        /// </summary>
        Bluetooth,
    }

    /// <summary>
    /// Specifies possible states for a wireless device.
    /// </summary>
    public enum WirelessState : int
    {
        /// <summary>
        /// Wireless device is off.
        /// </summary>
        Off = 0,
        /// <summary>
        /// Wireless device is on.
        /// </summary>
        On = 1,
        /// <summary>
        /// Only supported for Bluetooth.
        /// </summary>
        Discoverable = 2,//bt only
    }

    // whether to save before or after changing state
    internal enum SAVEACTION
    {
        RADIODEVICES_DONT_SAVE = 0,
        RADIODEVICES_PRE_SAVE,
        RADIODEVICES_POST_SAVE,
    }

    // Details of radio devices
    [StructLayout(LayoutKind.Sequential)]
    internal struct RDD
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        internal string pszDeviceName;  // Device name for registry setting.
        [MarshalAs(UnmanagedType.LPWStr)]
        internal string pszDisplayName; // Name to show the world
        internal WirelessState dwState;        // ON/off/[Discoverable for BT]
        internal WirelessState dwDesired;      // desired state - used for setting registry etc.
        internal WirelessDeviceType DeviceType;         // Managed, phone, BT etc.
        internal IntPtr pNext;    // Next device in list
    }

    /// <summary>
    /// Represents an individual radio device.
    /// </summary>
    public class WirelessDevice : IDisposable
    {
        private WirelessManager parent;

        private WirelessDeviceType type;
        private string deviceName;
        private string displayName;
        private WirelessState state;

        internal WirelessDevice(WirelessManager parent, RDD info)
        {
            this.parent = parent;
            this.type = info.DeviceType;
            deviceName = info.pszDeviceName;
            displayName = info.pszDisplayName;
            this.state = info.dwState;
        }

        /// <summary>
        /// Returns the name of the wireless device.
        /// </summary>
        public string DeviceName
        {
            get
            {
                return deviceName;
            }
        }
        
        /// <summary>
        /// Returns the friendly display name of the device (if available).
        /// </summary>
        public string DisplayName
        {
            get
            {
                return displayName;
            }
        }

        /// <summary>
        /// Returns the type of the wireless device.
        /// </summary>
        public WirelessDeviceType DeviceType
        {
            get
            {
                return this.type;
            }
        }

        /// <summary>
        /// Gets or sets the current radio state.
        /// </summary>
        public WirelessState RadioState
        {
            get
            {
                return this.state;
            }

            set
            {
                int result = parent.SetWirelessState(type, value);
                //on success update stored value
                if (result == 0)
                {
                    state = value;
                }
            }
        }

        #region IDisposable Members
        private void Dispose(bool disposing)
        {
            parent = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        ~WirelessDevice()
        {
            Dispose(false);
        }
        #endregion
    }
}
