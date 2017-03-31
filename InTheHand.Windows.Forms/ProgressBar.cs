// In The Hand - .NET Components for Mobility
//
// InTheHand.Windows.Forms.ProgressBar
// 
// Copyright (c) 2011-12 In The Hand Ltd, All rights reserved.

using System;
using System.Drawing;
using InTheHand.Runtime.InteropServices;
using InTheHand.Drawing;
using System.Windows.Forms;

namespace InTheHand.Windows.Forms
{
    /// <summary>
    /// Provides supporting methods for <see cref="ProgressBar"/>.
    /// </summary>
    public static class ProgressBarInTheHand
    {
        private const int PBM_SETSTEP = 0x404;
        private const int PBM_STEPIT = 0x405;
        private const int PBM_SETMARQUEE = 0x40A;

        /// <summary>
        /// Advances the current position of the progress bar by the amount of the Step property.
        /// </summary>
        /// <param name="progressBar">The progress bar.</param>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Pocket PC 2003 and later, Windows Mobile Version 5.0 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE 4.1 and later</description></item>
        /// </list>
        /// </remarks>
        public static void PerformStep(this ProgressBar progressBar)
        {
            NativeMethods.SendMessage(progressBar.Handle, PBM_STEPIT, 0, 0);
        }

        /// <summary>
        /// Sets the amount by which a call to the <see cref="PerformStep"/> method increases the current position of the progress bar.
        /// </summary>
        /// <param name="progressBar">The progress bar.</param>
        /// <param name="value">The amount by which to increment the progress bar with each call to the <see cref="PerformStep"/> method.
        /// The default is 10.</param>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Pocket PC 2003 and later, Windows Mobile Version 5.0 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE 4.1 and later</description></item>
        /// </list>
        /// </remarks>
        public static void SetStep(this ProgressBar progressBar, int value)
        {
            NativeMethods.SendMessage(progressBar.Handle, PBM_SETSTEP, value, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="progressBar"></param>
        /// <param name="value"></param>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE 6.0 and later</description></item>
        /// </list>
        /// </remarks>
        public static void SetMarqueeStyle(this ProgressBar progressBar, bool value)
        {
            ControlInTheHand.ModifyStyles(progressBar.Handle, value ? PBS_MARQUEE : ~PBS_MARQUEE, value ? ~PBS_MARQUEE : PBS_MARQUEE);
            NativeMethods.SendMessage(progressBar.Handle, PBM_SETMARQUEE, value ? 1 : 0, 100);
        }

        private const int PBS_MARQUEE = 0x8;
    }
}
