// In The Hand - .NET Components for Mobility
//
// InTheHand.Windows.Forms.Button
// 
// Copyright (c) 2007-2012 In The Hand Ltd, All rights reserved.

using System;
using System.Drawing;
using InTheHand.Runtime.InteropServices;
using InTheHand.Drawing;
using System.Windows.Forms;

namespace InTheHand.Windows.Forms
{
    /// <summary>
    /// Extension methods for <see cref="Button"/>.
    /// </summary>
    public static class ButtonInTheHand
    {
        private const int BS_MULTILINE = 0x00002000;
 
        /// <summary>
        /// Supports multi-line text on the Button control (to match default behaviour on the desktop).
        /// </summary>
        /// <param name="button">The button to update.</param>
        public static void SetMultiline(this ButtonBase button)
        {
            ControlInTheHand.ModifyStyles(button.Handle, BS_MULTILINE, 0);
        }

    }
}
