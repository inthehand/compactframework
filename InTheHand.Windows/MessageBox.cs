// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageBox.cs" company="In The Hand Ltd">
// Copyright (c) 2010-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Windows
{
    /// <summary>
    /// Displays a message box.
    /// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Pocket PC 2003, Windows Mobile 5.0 and later</description></item>
    /// <item><term>Windows Phone</term><description>Windows Phone 7 and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
    /// </list>
    /// </remarks>
    public static class MessageBox
    {
        /// <summary>
        /// Displays a message box that has a message and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A <see cref="String"/> that specifies the text to display.</param>
        /// <returns>A <see cref="MessageBoxResult"/> value that specifies which message box button is clicked by the user.</returns>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Pocket PC 2003, Windows Mobile 5.0 and later</description></item>
        /// <item><term>Windows Phone</term><description>Windows Phone 7 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
        /// </list>
        /// </remarks>
        public static MessageBoxResult Show(string messageBoxText)
        {
            return Show(messageBoxText, "", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None);
        }

        /// <summary>
        /// Displays a message box that has a message and title bar caption; and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A <see cref="String"/> that specifies the text to display.</param>
        /// <param name="caption">A <see cref="String"/> that specifies the title bar caption to display.</param>
        /// <returns>A <see cref="MessageBoxResult"/> value that specifies which message box button is clicked by the user.</returns>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Pocket PC 2003, Windows Mobile 5.0 and later</description></item>
        /// <item><term>Windows Phone</term><description>Windows Phone 7 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
        /// </list>
        /// </remarks>
        public static MessageBoxResult Show(string messageBoxText, string caption)
        {
            return Show(messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None);
        }

        /// <summary>
        /// Displays a message box that has a message, title bar caption, and button; and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A <see cref="String"/> that specifies the text to display.</param>
        /// <param name="caption">A <see cref="String"/> that specifies the title bar caption to display.</param>
        /// <param name="button">A <see cref="MessageBoxButton"/> value that specifies which button or buttons to display.</param>
        /// <returns>A <see cref="MessageBoxResult"/> value that specifies which message box button is clicked by the user.</returns>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Pocket PC 2003, Windows Mobile 5.0 and later</description></item>
        /// <item><term>Windows Phone</term><description>Windows Phone 7 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
        /// </list>
        /// </remarks>
        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button)
        {
            return Show(messageBoxText, caption, button, MessageBoxImage.None, MessageBoxResult.None);
        }

        /// <summary>
        /// Displays a message box that has a message, title bar caption, button, and icon; and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A <see cref="String"/> that specifies the text to display.</param>
        /// <param name="caption">A <see cref="String"/> that specifies the title bar caption to display.</param>
        /// <param name="button">A <see cref="MessageBoxButton"/> value that specifies which button or buttons to display.</param>
        /// <param name="icon">A <see cref="MessageBoxImage"/> value that specifies the icon to display.</param>
        /// <returns>A <see cref="MessageBoxResult"/> value that specifies which message box button is clicked by the user.</returns>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Pocket PC 2003, Windows Mobile 5.0 and later</description></item>
        /// <item><term>Windows Phone</term><description>Windows Phone 7 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
        /// </list>
        /// </remarks>
        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            return Show(messageBoxText, caption, button, icon, MessageBoxResult.None);
        }

        /// <summary>
        /// Displays a message box that has a message, title bar caption, button, and icon; and that accepts a default message box result and returns a result.
        /// </summary>
        /// <param name="messageBoxText">A <see cref="String"/> that specifies the text to display.</param>
        /// <param name="caption">A <see cref="String"/> that specifies the title bar caption to display.</param>
        /// <param name="button">A <see cref="MessageBoxButton"/> value that specifies which button or buttons to display.</param>
        /// <param name="icon">A <see cref="MessageBoxImage"/> value that specifies the icon to display.</param>
        /// <param name="defaultResult">A <see cref="MessageBoxResult"/> value that specifies the default result of the message box.</param>
        /// <returns>A <see cref="MessageBoxResult"/> value that specifies which message box button is clicked by the user.</returns>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Pocket PC 2003, Windows Mobile 5.0 and later</description></item>
        /// <item><term>Windows Phone</term><description>Windows Phone 7 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
        /// </list>
        /// </remarks>
        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button,
            MessageBoxImage icon, MessageBoxResult defaultResult)
        {
            int mbtype = (int)button | (int)icon;

            return NativeMethods.MessageBox(IntPtr.Zero, messageBoxText, caption, mbtype);

            /*
            System.Windows.Forms.DialogResult dr = System.Windows.Forms.DialogResult.None;

            if (InTheHand.WindowsCE.Forms.SystemSettings.Platform == WinCEPlatform.PocketPC)// || (InTheHand.WindowsCE.Forms.SystemSettingsHelper.Platform == InTheHand.WindowsCE.Forms.WinCEPlatform.Smartphone && (buttons == MessageBoxButtons.YesNoCancel || buttons == MessageBoxButtons.AbortRetryIgnore)))
            {
                MessageBoxForm mbf = new MessageBoxForm(messageBoxText, caption, button, icon, defaultButtonIndex);
                dr = mbf.ShowDialog();
                mbf.Dispose();
                mbf = null;
            }
            else
            {
                mbbButtons = (System.Windows.Forms.MessageBoxButtons)((int)button);
                mbbIcon = (System.Windows.Forms.MessageBoxIcon)((int)icon);
                dr = System.Windows.Forms.MessageBox.Show(messageBoxText, caption, mbbButtons, mbbIcon, (System.Windows.Forms.MessageBoxDefaultButton)(defaultButtonIndex << 8));
            }
            return (MessageBoxResult)((int)dr);
            */
        }

        private static class NativeMethods
        {
            [DllImport("coredll")]
            internal static extern MessageBoxResult MessageBox(IntPtr hWnd, string text, string caption, int type);
        }
    }
}
