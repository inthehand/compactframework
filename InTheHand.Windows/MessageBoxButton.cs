// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageBoxButton.cs" company="In The Hand Ltd">
// Copyright (c) 2010-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Windows
{
    /// <summary>
    /// Specifies the buttons that are displayed on a message box.
    /// Used as an argument of the <see cref="MessageBox.Show(string,string,MessageBoxButton)"/> method.
    /// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Pocket PC 2003, Windows Mobile 5.0 and later</description></item>
    /// <item><term>Windows Phone</term><description>Windows Phone 7 and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
    /// </list>
    /// </remarks>
    public enum MessageBoxButton
    {
        /// <summary>
        /// The message box displays an <b>OK</b> button.
        /// </summary>
        OK = 0,

        /// <summary>
        /// The message box displays <b>OK</b> and <b>Cancel</b> buttons.
        /// </summary>
        OKCancel = 1,

        /// <summary>
        /// The message box displays <b>Abort</b>, <b>Retry</b> and <b>Ignore</b> buttons.
        /// </summary>
        AbortRetryIgnore = 2,

        /// <summary>
        /// The message box displays <b>Yes</b>, <b>No</b> and <b>Cancel</b> buttons.
        /// </summary>
        YesNoCancel = 3,

        /// <summary>
        /// The message box displays <b>Yes</b> and <b>No</b> buttons.
        /// </summary>
        YesNo = 4,

        /// <summary>
        /// The message box displays <b>Retry</b> and <b>Cancel</b> buttons.
        /// </summary>
        RetryCancel = 5,
    }
}
