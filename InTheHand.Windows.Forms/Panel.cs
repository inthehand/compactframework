// In The Hand - .NET Components for Mobility
//
// InTheHand.Windows.Forms.ScrollableControl
// 
// Copyright (c) 2011-12 In The Hand Ltd, All rights reserved.

using System;
using System.Drawing;
using InTheHand.Runtime.InteropServices;
using InTheHand.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.WindowsCE.Forms;
using InTheHand.WindowsCE.Forms;

namespace InTheHand.Windows.Forms
{
    /// <summary>
    /// Provides supporting methods for <see cref="ScrollableControl"/>.
    /// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 6.1 Professional Edition and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows CE 6.0</description></item>
    /// </list>
    /// </remarks>
    public static class ScrollableControlInTheHand
    {
        /// <summary>
        /// Gets a value which determines if Pan and Flick gestures are automatically handled in the ScrollableControl.
        /// </summary>
        /// <param name="control">The ScrollableControl</param>
        /// <returns>true if automatic gestures are enabled, else false.</returns>
        /// <remarks>
        /// <list type="table"><listheader><term>Requirements</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile 6.1 and later</description></item>
        /// <item><term>Windows Embedded</term><description>Windows Embedded 6.0</description></item>
        /// </list>
        /// </remarks>
        public static bool GetGesturesEnabled(this ScrollableControl control)
        {
            if ((InTheHand.WindowsCE.Forms.SystemSettingsInTheHand.Platform != WinCEPlatform.Smartphone) && InTheHand.NativeMethods.IsMobile6 && System.Environment.OSVersion.Version.Major < 7)
            {
                try
                {
                    WAGINFO wi;

                    bool success = NativeMethods.GetWindowAutoGesture(control.Handle, out wi);

                    if (success)
                    {
                        if (wi.dwFlags.HasFlag(WAGIF.HSCROLLABLE) || wi.dwFlags.HasFlag(WAGIF.VSCROLLABLE))
                        {
                            return true;
                        }
                    }
                }
                catch
                {
                }
            }

            return false;
        }

        /// <summary>
        /// Sets a value which determines if Pan and Flick gestures are automatically handled in the ScrollableControl.
        /// </summary>
        /// <param name="control">The ScrollableControl</param>
        /// <param name="value">true to enable automatic gestures, false to disable.</param>
        /// <remarks>
        /// <list type="table"><listheader><term>Requirements</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile 6.1 and later</description></item>
        /// <item><term>Windows Embedded</term><description>Windows Embedded 6.0</description></item>
        /// </list>
        /// </remarks>
        public static void SetGesturesEnabled(this ScrollableControl control, bool value)
        {
            if ((SystemSettingsInTheHand.Platform != WinCEPlatform.Smartphone) && InTheHand.NativeMethods.IsMobile6 && System.Environment.OSVersion.Version.Major < 7)
            {
                try
                {
                    WAGINFO wi = new WAGINFO();
                    wi.cbSize = Marshal.SizeOf(wi);
                    wi.dwFlags = (value ? WAGIF.VSCROLLABLE | WAGIF.HSCROLLABLE : 0) | WAGIF.OWNERANIMATE;

                    bool success = NativeMethods.SetWindowAutoGesture(control.Handle, ref wi);
                }
                catch
                {
                }
            }
        }

        internal static class NativeMethods
        {
            [DllImport("coredll", SetLastError = true)]
            internal static extern bool SetWindowAutoGesture(IntPtr hWnd, ref WAGINFO lpAutoGestureInfo);

            [DllImport("coredll", SetLastError = true)]
            internal static extern bool GetWindowAutoGesture(IntPtr hWnd, out WAGINFO lpAutoGestureInfo);
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct WAGINFO
        {
            internal int cbSize;
            internal WAGIF dwFlags;
            internal int nOwnerAnimateMessage;
            internal int nAnimateStatusMessage;
            internal IntPtr hExtentBrush;
            internal int nItemHeight;
            internal int nItemWidth;
            internal byte bHorizontalExtent;
            internal byte bVerticalExtent;
        }

        [Flags()]
        internal enum WAGIF
        {
            OWNERANIMATE = 0x0001,
            VSCROLLABLE = 0x0002,
            HSCROLLABLE = 0x0004,
            LOCKAXES = 0x0008,
            IGNOREPAN = 0x0010,
            IGNORESCROLL = 0x0020,
        }
    }
}
