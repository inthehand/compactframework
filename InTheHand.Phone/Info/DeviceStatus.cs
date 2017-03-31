// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeviceStatus.cs" company="In The Hand Ltd">
// Copyright (c) 2010-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Microsoft.WindowsCE.Forms;

namespace InTheHand.Phone.Info
{
    /// <summary>
    /// Allows an application to obtain information about the device on which it is running.
    /// </summary>
    public static class DeviceStatus
    {
        private static string manufacturer;
        private static string deviceName;
        private static long peakMemoryUsage = 0;

        #region Device Manufacturer
        /// <summary>
        /// Returns the manufacturer of the device, such as Samsung or LGE.
        /// </summary>
        /// <remarks>Because this property requires the DeviceIdentity capability it is not available on Windows Phone 7 - use GetValue instead.</remarks>
        public static string DeviceManufacturer
        {
            get
            {
                if (manufacturer == null)
                {
                    bool success = InTheHand.NativeMethods.SystemParametersInfoString(InTheHand.NativeMethods.SPI.GETPLATFORMMANUFACTURER, out manufacturer);
                }

                return manufacturer;
            }
        }
        #endregion

        #region Device Name
        /// <summary>
        /// Returns the name of the device and possibly the manufacturer, such as Samsung Taylor or Pacific.
        /// </summary>
        /// <remarks>Because this property requires the DeviceIdentity capability it is not available on Windows Phone 7 - use GetValue instead.</remarks>
        public static string DeviceName
        {
            get
            {
                if (deviceName == null)
                {
                    bool success = InTheHand.NativeMethods.SystemParametersInfoString(InTheHand.NativeMethods.SPI.GETPLATFORMNAME, out deviceName);
                }

                return deviceName;
            }
        }
        #endregion

        #region Device Unique Id
        /// <summary>
        /// Returns a unique hash for the device, constant across all applications and across flashing / upgrading the OS.
        /// </summary>
        /// <remarks>Because this property requires the DeviceIdentity capability it is not available on Windows Phone 7 - use GetValue instead.</remarks>
        public static byte[] DeviceUniqueId
        {
            get
            {
                if (deviceUniqueId == null)
                {
                    object o;
                    bool success = GetDeviceUniqueId(out o);
                }

                return deviceUniqueId;
            }
        }
        #endregion

        #region Device Total Memory
        private static long deviceTotalMemory = -1;
        /// <summary>
        /// Returns the device physical RAM size in bytes.
        /// </summary>
        public static long DeviceTotalMemory
        {
            get
            {
                if (deviceTotalMemory == -1)
                {
                    NativeMethods.MEMORYSTATUS tms = new NativeMethods.MEMORYSTATUS();
                    NativeMethods.GlobalMemoryStatus(ref tms);
                    deviceTotalMemory = (long)tms.dwTotalPhys;
                }

                return deviceTotalMemory;
            }
        }
        #endregion

        #region Application Current Memory Usage
        /// <summary>
        /// Returns the current application’s memory usage in bytes.
        /// </summary>
        public static long ApplicationCurrentMemoryUsage
        {
            get
            {
                object o;
                if (GetApplicationCurrentMemoryUsage(out o))
                {
                    return (long)o;
                }

                return 0;
            }
        }
        #endregion

        #region Application Peak Memory Usage
        /// <summary>
        /// Returns the current application peak memory usage in bytes.
        /// </summary>
        public static long ApplicationPeakMemoryUsage
        {
            get
            {
                object o;
                GetApplicationCurrentMemoryUsage(out o);

                return peakMemoryUsage;
            }
        }
        #endregion

        #region Get Application Current Memory Usage
        private static int processId = 0;

        private static bool GetApplicationCurrentMemoryUsage(out object propertyValue)
        {
            propertyValue = null;

            long memoryUsage = 0;

            if (processId == 0)
            {
                processId = System.Diagnostics.Process.GetCurrentProcess().Id;
            }
            IntPtr snapshot = InTheHand.Diagnostics.NativeMethods.CreateToolhelp32Snapshot(InTheHand.Diagnostics.NativeMethods.TH32CS.SNAPHEAPLIST | InTheHand.Diagnostics.NativeMethods.TH32CS.SNAPMODULE, processId);
            if (snapshot != IntPtr.Zero)
            {
                InTheHand.Diagnostics.NativeMethods.MODULEENTRY32 me = new InTheHand.Diagnostics.NativeMethods.MODULEENTRY32();
                me.dwSize = Marshal.SizeOf(me);

                bool gotModule = InTheHand.Diagnostics.NativeMethods.Module32First(snapshot, ref me);
                while (gotModule)
                {
                    if (me.modBaseAddr.ToInt32() < 0x1ffffff)
                    {
                        // slot zero
                        memoryUsage += me.modBaseSize;
                    }
                    gotModule = InTheHand.Diagnostics.NativeMethods.Module32Next(snapshot, ref me);
                }

                try
                {
                    InTheHand.Diagnostics.NativeMethods.HEAPLIST32 hl = new InTheHand.Diagnostics.NativeMethods.HEAPLIST32();
                    hl.dwSize = Marshal.SizeOf(hl);


                    InTheHand.Diagnostics.NativeMethods.HEAPENTRY32 he = new InTheHand.Diagnostics.NativeMethods.HEAPENTRY32();
                    he.dwSize = Marshal.SizeOf(he);

                    bool gotHeapList = InTheHand.Diagnostics.NativeMethods.Heap32ListFirst(snapshot, ref hl);
                    while (gotHeapList)
                    {
                        bool gotHeap = InTheHand.Diagnostics.NativeMethods.Heap32First(snapshot, ref he, processId, hl.th32HeapID);
                        while (gotHeap)
                        {
                            memoryUsage += he.dwBlockSize;
                            
                            gotHeap = InTheHand.Diagnostics.NativeMethods.Heap32Next(snapshot, ref he);
                        }
                        gotHeapList = InTheHand.Diagnostics.NativeMethods.Heap32ListNext(snapshot, ref hl);
                    }
                }
                finally
                {
                    bool closed = InTheHand.Diagnostics.NativeMethods.CloseToolhelp32Snapshot(snapshot);
                }
            }

            if (memoryUsage > peakMemoryUsage)
            {
                peakMemoryUsage = memoryUsage;
            }

            propertyValue = memoryUsage;
            return memoryUsage > 0;
        }
        #endregion

        #region Get Device Unique ID
        private static byte[] deviceUniqueId = null;

        internal static bool GetDeviceUniqueId(out object propertyValue)
        {
            bool success = true;
            propertyValue = null;

            if (deviceUniqueId == null)
            {
                if (InTheHand.NativeMethods.IsMobile5)
                {
                    byte[] key = new byte[] { 0x49, 0x6e, 0x54, 0x68, 0x65, 0x48, 0x61, 0x6e, 0x64 };

                    int outLen = 20;
                    byte[] uniqueId = new byte[outLen];

                    int hresult = NativeMethods.DeviceUniqueID(key, key.Length, 1, uniqueId, ref outLen);
                    if (hresult < 0)
                    {
                        System.Diagnostics.Debug.WriteLine("DeviceUniqueID=" + hresult.ToString("X"));
                        success = false;
                    }

                    deviceUniqueId = uniqueId;
                    success = true;
                }
                else
                {
                    bool returnValue = false;

                    // use legacy method only for PPC2003 and Windows CE. It is privileged so not recommened for recent WinMo versions
                    int len = 64;
                    int cb = 0;
                    byte[] buff = new byte[len];
                    buff[0] = Convert.ToByte(len);

                    try
                    {
                        returnValue = NativeMethods.KernelIoControl(0x1010054, null, 0, buff, len, ref cb);
                    }
                    catch
                    {
                        return false;
                    }

                    if (returnValue)
                    {
                        int dwPresetIDOffset = BitConverter.ToInt32(buff, 4);
                        int dwPresetIDSize = BitConverter.ToInt32(buff, 0x8);
                        int dwPlatformIDOffset = BitConverter.ToInt32(buff, 0xC);
                        int dwPlatformIDSize = BitConverter.ToInt32(buff, 0x10);

                        // in testing this is often 16 bytes but don't assume this
                        byte[] idbytes = new byte[dwPresetIDSize + dwPlatformIDSize];

                        Buffer.BlockCopy(buff, dwPresetIDOffset, idbytes, 0, dwPresetIDSize);
                        Buffer.BlockCopy(buff, dwPlatformIDOffset, idbytes, dwPresetIDSize, dwPlatformIDSize);

                        // now has the result to generate our unique 20bytes
                        System.Security.Cryptography.SHA1 sha1 = System.Security.Cryptography.SHA1.Create();
                        deviceUniqueId = sha1.ComputeHash(idbytes);

                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                }
            }

            // return stored value
            propertyValue = deviceUniqueId;
            return success;
        }
        #endregion

        #region Is Keyboard Present
        private static bool? isKeyboardPresent;
        /// <summary>
        /// Indicates whether the device contains a physical hardware keyboard.
        /// </summary>
        public static bool IsKeyboardPresent
        {
            get
            {
                // retrieve once
                if (!isKeyboardPresent.HasValue)
                {
                    if (InTheHand.WindowsCE.Forms.SystemSettingsInTheHand.Platform != WinCEPlatform.WinCEGeneric)
                    {
                        try
                        {
                            int hasKeyboard = (int)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Shell", "HasKeyboard", 0);
                            isKeyboardPresent = hasKeyboard != 0;
                        }
                        catch { }
                    }
                    else
                    {
                        isKeyboardPresent = (NativeMethods.GetKeyboardStatus() & 0x1) == 0x1;
                    }
                }

                return isKeyboardPresent.Value;
            }
        }
        #endregion

        #region Power Source
        //TODO: PowerSource
        #endregion

        #region Native Methods
        private static class NativeMethods
        {
            [DllImport("coredll", SetLastError = true)]
            internal static extern void GlobalMemoryStatus(ref MEMORYSTATUS lpBuffer);

            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
            internal struct MEMORYSTATUS
            {
                internal uint dwLength;
                internal uint dwMemoryLoad;
                internal uint dwTotalPhys;
                internal uint dwAvailPhys;
                internal uint dwTotalPageFile;
                internal uint dwAvailPageFile;
                internal uint dwTotalVirtual;
                internal uint dwAvailVirtual;
            }

            [DllImport("coredll", EntryPoint = "GetDeviceUniqueID", SetLastError = false)]
            internal static extern int DeviceUniqueID(byte[] pbApplicationData, int cbApplictionData, int dwDeviceIDVersion, byte[] pbDeviceIDOutput, ref int pcbDeviceIDOutput);

            [DllImport("coredll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool KernelIoControl(int dwIoControlCode, byte[] lpInBuf, int nInBufSize, byte[] outBuf, int nOutBufSize, ref int lpBytesReturned);

            [DllImport("coredll")]
            internal static extern int GetKeyboardStatus(); 
        }
        #endregion
    }

    
}
