// In The Hand - .NET Components for Mobility
//
// InTheHand.Windows.Forms.TabControl
// 
// Copyright (c) 2009-2012 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace InTheHand.Windows.Forms
{
    /// <summary>
    /// Provides helper methods for the <see cref="TabControl"/> on Windows Mobile 6.5.
    /// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 6.5 and later</description></item>
    /// </list>
    /// </remarks>
    public static class TabControlInTheHand
    {
        private const int TCS_TOOLTIPS = 0x4000;

        /// <summary>
        /// Updates the selected <see cref="TabControl"/> with the Windows Mobile 6.5 style.
        /// </summary>
        /// <param name="tabControl"></param>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile Version 6.5 Professional or Classic Edition and later</description></item>
        /// </list>
        /// </remarks>
        public static void EnableVisualStyles(this TabControl tabControl)
        {
            if (InTheHand.NativeMethods.IsMobile65)
            {
                //get handle of native control
                IntPtr hNativeTab = InTheHand.Windows.Forms.NativeMethods.GetWindow(tabControl.Handle, InTheHand.Windows.Forms.NativeMethods.GW.CHILD);
                //get current style flags
                int style = InTheHand.Windows.Forms.NativeMethods.GetWindowLong(hNativeTab, InTheHand.Windows.Forms.NativeMethods.GWL.STYLE).ToInt32();
                //add tooltips style
                style = InTheHand.Windows.Forms.NativeMethods.SetWindowLong(hNativeTab, InTheHand.Windows.Forms.NativeMethods.GWL.STYLE, style | 0x4000);
            }
        }
    }
}
