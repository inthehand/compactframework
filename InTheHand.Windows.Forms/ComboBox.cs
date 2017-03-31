// In The Hand - .NET Components for Mobility
//
// InTheHand.Windows.Forms.ComboBox
// 
// Copyright (c) 2007-2012 In The Hand Ltd, All rights reserved.

using System;
using System.Windows.Forms;
using Microsoft.WindowsCE.Forms;
using InTheHand.WindowsCE.Forms;

namespace InTheHand.Windows.Forms
{
    /// <summary>
    /// Provides supporting methods for <see cref="ListBox"/>.
    /// </summary>
    /// <seealso cref="ListBox"/>
    public static class ListBoxInTheHand
    {
        /// <summary>
        /// Finds the first item in the <see cref="ListBox"/> that starts with the specified string.
        /// </summary>
        /// <param name="lb">The <see cref="ListBox"/> to search.</param>
        /// <param name="s">The <see cref="System.String"/> to search for.</param>
        /// <returns>The zero-based index of the first item found; returns -1 if no match is found.</returns>
        public static int FindString(this ListBox lb, string s)
        {
            return FindString(lb, s, -1);
        }

        /// <summary>
        /// Finds the first item in the <see cref="ListBox"/> that starts with the specified string.
        /// </summary>
        /// <param name="lb">The <see cref="ListBox"/> to search.</param>
        /// <param name="s">The <see cref="System.String"/> to search for.</param>
        /// <param name="startIndex">The zero-based index of the item before the first item to be searched.
        /// Set to -1 to search from the beginning of the control.</param>
        /// <returns>The zero-based index of the first item found; returns -1 if no match is found.</returns>
        public static int FindString(this ListBox lb, string s, int startIndex)
        {
            return ControlInTheHand.FindString(lb, NativeMethods.LB_FINDSTRING, s, startIndex);
        }

        /// <summary>
        /// Finds the first item in the <see cref="ListBox"/> that matches the specified string.
        /// </summary>
        /// <param name="lb">The <see cref="ListBox"/> to search.</param>
        /// <param name="s">The <see cref="System.String"/> to search for.</param>
        /// <returns>The zero-based index of the first item found; returns -1 if no match is found.</returns>
        public static int FindStringExact(this ListBox lb, string s)
        {
            return FindStringExact(lb, s, -1);
        }

        /// <summary>
        /// Finds the first item in the <see cref="ListBox"/> after the specified index that matches the specified string.
        /// </summary>
        /// <param name="lb">The <see cref="ListBox"/> to search.</param>
        /// <param name="s">The <see cref="System.String"/> to search for.</param>
        /// <param name="startIndex">The zero-based index of the item before the first item to be searched. Set to -1 to search from the beginning of the control.</param>
        /// <returns>The zero-based index of the first item found; returns -1 if no match is found.</returns>
        public static int FindStringExact(this ListBox lb, string s, int startIndex)
        {
            return ControlInTheHand.FindString(lb, NativeMethods.LB_FINDSTRINGEXACT, s, startIndex);
        }

        private const int LB_SETITEMHEIGHT = 0x01A0;
        private const int LB_GETITEMHEIGHT = 0x01A1;

        /// <summary>
        /// Gets the height of an item in the ListBox.
        /// </summary>
        /// <param name="listBox"></param>
        /// <returns></returns>
        public static int GetItemHeight(this ListBox listBox)
        {
            return NativeMethods.SendMessage(listBox.Handle, LB_GETITEMHEIGHT, 0, 0);
        }

        /// <summary>
        /// Sets the height of an item in the ListBox.
        /// </summary>
        /// <param name="listBox"></param>
        /// <param name="value"></param>
        public static void SetItemHeight(this ListBox listBox, int value)
        {
            NativeMethods.SendMessage(listBox.Handle, LB_SETITEMHEIGHT, 0, value);
        }

        private const int LBS_NOINTEGRALHEIGHT = 0x0100;
        /// <summary>
        /// Gets a value indicating whether the list uses the full height specified in the designer.
        /// </summary>
        /// <param name="listBox">The <see cref="ListBox"/> control.</param>
        /// <returns></returns>
        public static bool GetNoIntegralHeight(this ListBox listBox)
        {
            // get current style flags
            int style = InTheHand.Windows.Forms.NativeMethods.GetWindowLong(listBox.Handle, InTheHand.Windows.Forms.NativeMethods.GWL.STYLE).ToInt32();
            return (style & LBS_NOINTEGRALHEIGHT) != 0;
        }

        /// <summary>
        /// Gets a value indicating whether the list uses the full height specified in the designer.
        /// </summary>
        /// <param name="listBox">The <see cref="ListBox"/> control.</param>
        /// <param name="enable">true if gradient is to be drawn behind list; otherwise, false. The default is false.</param>
        public static void SetNoIntegralHeight(this ListBox listBox, bool enable)
        {
            // get current style flags
            int style = InTheHand.Windows.Forms.NativeMethods.GetWindowLong(listBox.Handle, InTheHand.Windows.Forms.NativeMethods.GWL.STYLE).ToInt32();
            // add tooltips style
            style = InTheHand.Windows.Forms.NativeMethods.SetWindowLong(listBox.Handle, InTheHand.Windows.Forms.NativeMethods.GWL.STYLE, (enable ? style | LBS_NOINTEGRALHEIGHT : style & ~LBS_NOINTEGRALHEIGHT));
        }
    }

    /// <summary>
    /// Provides supporting methods for <see cref="ComboBox"/>.
    /// </summary>
    /// <seealso cref="ComboBox"/>
    public static class ComboBoxInTheHand
    {
        /// <summary>
        /// Finds the first item in the <see cref="ComboBox"/> that starts with the specified string.
        /// </summary>
        /// <param name="cb">The <see cref="ComboBox"/> to search.</param>
        /// <param name="s">The <see cref="System.String"/> to search for.</param>
        /// <returns>The zero-based index of the first item found; returns -1 if no match is found.</returns>
        public static int FindString(this ComboBox cb, string s)
        {
            return FindString(cb, s, -1);
        }

        /// <summary>
        /// Finds the first item in the <see cref="ComboBox"/> that starts with the specified string.
        /// </summary>
        /// <param name="cb">The <see cref="ComboBox"/> to search.</param>
        /// <param name="s">The <see cref="System.String"/> to search for.</param>
        /// <param name="startIndex">The zero-based index of the item before the first item to be searched. Set to -1 to search from the beginning of the control.</param>
        /// <returns>The zero-based index of the first item found; returns -1 if no match is found.</returns>        
        public static int FindString(this ComboBox cb, string s, int startIndex)
        {
            if (SystemSettingsInTheHand.Platform == WinCEPlatform.Smartphone)
            {
                return ControlInTheHand.FindString(cb, NativeMethods.LB_FINDSTRING, s, startIndex);
            }
            else
            {
                return ControlInTheHand.FindString(cb, NativeMethods.CB_FINDSTRING, s, startIndex);
            }
        }

        /// <summary>
        /// Finds the first item in the <see cref="ComboBox"/> that matches the specified string.
        /// </summary>
        /// <param name="cb">The <see cref="ComboBox"/> to search.</param>
        /// <param name="s">The <see cref="System.String"/> to search for.</param>
        /// <returns>The zero-based index of the first item found; returns -1 if no match is found.</returns>
        public static int FindStringExact(this ComboBox cb, string s)
        {
            return FindStringExact(cb, s, -1);
        }

        /// <summary>
        /// Finds the first item in the <see cref="ComboBox"/> after the specified index that matches the specified string.
        /// </summary>
        /// <param name="cb">The <see cref="ComboBox"/> to search.</param>
        /// <param name="s">The <see cref="System.String"/> to search for.</param>
        /// <param name="startIndex">The zero-based index of the item before the first item to be searched. Set to -1 to search from the beginning of the control.</param>
        /// <returns>The zero-based index of the first item found; returns -1 if no match is found.</returns>
        public static int FindStringExact(this ComboBox cb, string s, int startIndex)
        {
            if (InTheHand.WindowsCE.Forms.SystemSettingsInTheHand.Platform == WinCEPlatform.Smartphone)
            {
                return ControlInTheHand.FindString(cb, NativeMethods.LB_FINDSTRINGEXACT, s, startIndex);
            }
            else
            {
                return ControlInTheHand.FindString(cb, NativeMethods.CB_FINDSTRINGEXACT, s, startIndex);
            }
        }

        /// <summary>
        /// Gets the caption of the associated full-screen picker (Standard Edition/Smartphone only)
        /// </summary>
        /// <param name="cb">ComboBox control.</param>
        /// <returns>The caption of the associated full-screen picker</returns>
        public static string GetCaption(this ComboBox cb)
        {
            if (SystemSettingsInTheHand.Platform == WinCEPlatform.Smartphone)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder(260);

                // Obtain the native window handle of the up/down spinner control
                IntPtr hWndSpinner = NativeMethods.GetWindow(cb.Handle, NativeMethods.GW.HWNDNEXT);

                // get the title of the spinner
                int chars = NativeMethods.GetWindowText(hWndSpinner, sb, sb.Capacity);
                return sb.ToString(0, chars);
            }

            return cb.Text;
        }

        /// <summary>
        /// Sets the caption of the associated full-screen picker (Standard Edition/Smartphone only)
        /// </summary>
        /// <param name="cb">ComboBox control.</param>
        /// <param name="caption">The caption to display in the Titlebar.</param>
        public static void SetCaption(this ComboBox cb, string caption)
        {
            if (SystemSettingsInTheHand.Platform == WinCEPlatform.Smartphone)
            {
                // Obtain the native window handle of the up/down spinner control
                IntPtr hWndSpinner = NativeMethods.GetWindow(cb.Handle, NativeMethods.GW.HWNDNEXT);

                // Set the title of the spinner
                NativeMethods.SetWindowText(hWndSpinner, caption);
            }
        }
    }
}