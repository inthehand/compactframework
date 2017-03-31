// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VibrateController.cs" company="In The Hand Ltd">
// Copyright (c) 2008-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using InTheHand.Runtime.InteropServices;
using Microsoft.WindowsCE.Forms;

namespace InTheHand.Devices
{
    /// <summary>
    /// Allows applications to start and stop vibration on the device.
    /// Obtain an instance of this class using the <see cref="Default"/> property.
    /// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Requirements</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile 2003 and later</description></item>
    /// </list>
    /// </remarks>
    public sealed class VibrateController
    {
        // private static TimeSpan MinimumDuration = TimeSpan.FromSeconds(0.0);
        private static TimeSpan MaximumDuration = TimeSpan.FromSeconds(5.0);

        private static VibrateController instance = null;

        /// <summary>
        /// The static method used to obtain an instance of the <see cref="VibrateController"/> object.
        /// </summary>
        public static VibrateController Default
        {
            get
            {
                if (instance == null)
                {
                    instance = new VibrateController();
                }

                return instance;
            }
        }

        private VibrateController()
        {
            if (InTheHand.WindowsCE.Forms.SystemSettingsInTheHand.Platform != WinCEPlatform.Smartphone)
            {
                // get count
                int count = -1;
                IntPtr p = Marshal.AllocHGlobal(4);
                try
                {
                    bool success = NativeMethods.NLedGetDeviceInfo(0, p);
                    count = Marshal.ReadInt32(p);
                }
                finally
                {
                    Marshal.FreeHGlobal(p);
                }

                // loop through leds
                for (int i = 0; i < count; i++)
                {
                    System.Diagnostics.Debug.WriteLine(i.ToString());

                    // set ledIndex if vibrate
                    IntPtr ps = MarshalInTheHand.AllocHGlobal(Marshal.SizeOf(typeof(NativeMethods.NLED_SUPPORTS_INFO)));
                    try
                    {
                        Marshal.WriteInt32(ps, i);
                        bool success = NativeMethods.NLedGetDeviceInfo(1, ps);
                        if (success)
                        {
                            NativeMethods.NLED_SUPPORTS_INFO lsi = (NativeMethods.NLED_SUPPORTS_INFO)Marshal.PtrToStructure(ps, typeof(NativeMethods.NLED_SUPPORTS_INFO));
                            
                            if (lsi.lCycleAdjust == -1)
                            {
                                ledIndex = i;
                                return;
                            }
                        }
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(ps);
                    }
                }             
            }
        }

        private static int ledIndex = -1;
 
        /// <summary>
        /// Starts vibration on the device.
        /// </summary>
        /// <param name="duration">A <see cref="TimeSpan"/> object specifying the amount of time for which the phone vibrates.</param>
        /// <exception cref="ArgumentOutOfRangeException">Duration is greater than the maximum allowed duration or duration is negative.</exception>
        public void Start(System.TimeSpan duration)
        {
            if (duration.CompareTo(MaximumDuration) > 0)
            {
                throw new ArgumentOutOfRangeException("duration", Phone.Properties.Resources.vibratecontroller_DurationMax);
            }

            if (duration.Ticks < 0)
            {
                throw new ArgumentOutOfRangeException("duration", Phone.Properties.Resources.vibratecontroller_DurationMin);
            }

            if (InTheHand.WindowsCE.Forms.SystemSettingsInTheHand.Platform == WinCEPlatform.Smartphone)
            {
                int hresult = NativeMethods.Vibrate(0, IntPtr.Zero, true, (int)duration.TotalMilliseconds);
                if (hresult < 0)
                {
                    Marshal.ThrowExceptionForHR(hresult);
                }
            }
            else if (InTheHand.WindowsCE.Forms.SystemSettingsInTheHand.Platform == WinCEPlatform.PocketPC)
            {
                if (ledIndex > -1)
                {
                    NativeMethods.NLED_SETTINGS_INFO nsi = new NativeMethods.NLED_SETTINGS_INFO();
                    nsi.LedNum = ledIndex;
                    nsi.OffOnBlink = 1;
                    bool success = NativeMethods.NLedSetDevice(2, ref nsi);

                    // setup a thread to turn off after duration
                    System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(TurnOffLed), (int)duration.TotalMilliseconds);
                }
            }
        }

        private void TurnOffLed(object duration)
        {
            System.Threading.Thread.Sleep((int)duration);
            NativeMethods.NLED_SETTINGS_INFO nsi = new NativeMethods.NLED_SETTINGS_INFO();
            nsi.LedNum = ledIndex;
            nsi.OffOnBlink = 0;
            bool success = NativeMethods.NLedSetDevice(2, ref nsi);
        }

        /// <summary>
        /// Stops vibration on the device.
        /// </summary>
        public void Stop()
        {
            if (InTheHand.WindowsCE.Forms.SystemSettingsInTheHand.Platform == WinCEPlatform.Smartphone)
            {
                int result = NativeMethods.VibrateStop();
                if (result < 0)
                {
                    Marshal.ThrowExceptionForHR(result);
                }
            }
            else if (ledIndex > -1)
            {
                TurnOffLed(0);
            }
        }

        private static class NativeMethods
        {
            // Standard Edition
            [DllImport("aygshell", EntryPoint = "Vibrate", SetLastError = false)]
            internal static extern int Vibrate(int cvn, IntPtr rgvn, [MarshalAs(UnmanagedType.Bool)] bool repeat, int timeout);

            [DllImport("aygshell", EntryPoint = "VibrateStop", SetLastError = false)]
            internal static extern int VibrateStop();

            // Everything else - use the LED API
            [DllImport("coredll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool NLedGetDeviceInfo(short id, IntPtr output);

            [DllImport("coredll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool NLedSetDevice(short deviceId, ref NLED_SETTINGS_INFO input);

            [StructLayout(LayoutKind.Sequential)]
            internal struct NLED_SETTINGS_INFO
            {
                internal int LedNum;
                internal int OffOnBlink;
                internal int TotalCycleTime;
                internal int OnTime;
                internal int OffTime;
                internal int MetaCycleOn;
                internal int MetaCycleOff;
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct NLED_SUPPORTS_INFO
            {
                internal int LedNum;						// @FIELD 	LED number, 0 is first LED
                internal int lCycleAdjust;				// @FIELD	Granularity of cycle time adjustments (microseconds)
                internal int fAdjustTotalCycleTime;		// @FIELD	LED has an adjustable total cycle time
                internal int fAdjustOnTime;				// @FIELD	LED has separate on time
                internal int fAdjustOffTime;				// @FIELD	LED has separate off time
                internal int fMetaCycleOn;				// @FIELD	LED can do blink n, pause, blink n, ...
                internal int fMetaCycleOff;				// @FIELD	LED can do blink n, pause n, blink n, ...
            }
        }
    }
}
