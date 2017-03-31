// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageBoxResult.cs" company="In The Hand Ltd">
// Copyright (c) 2010-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Windows
{
    /// <summary>
    /// Specifies which message box button that a user clicks.
    /// MessageBoxResult is returned by the <see cref="MessageBox.Show(string,string,MessageBoxButton,MessageBoxImage,MessageBoxResult)"/> method.
    /// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Pocket PC 2003, Windows Mobile 5.0 and later</description></item>
    /// <item><term>Windows Phone</term><description>Windows Phone 7 and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
    /// </list>
    /// </remarks>
    public enum MessageBoxResult
    {
        /// <summary>
        /// The message box returns no result.
        /// </summary>
        None = 0,

        /// <summary>
        /// The result value of the message box is <b>OK</b>.
        /// </summary>
        OK = 1,

        /// <summary>
        /// The result value of the message box is <b>Cancel</b>.
        /// </summary>
        Cancel = 2,

        /// <summary>
        /// The result value of the message box is <b>Abort</b>.
        /// </summary>
        Abort = 3,

        /// <summary>
        /// The result value of the message box is <b>Retry</b>.
        /// </summary>
        Retry = 4,

        /// <summary>
        /// The result value of the message box is <b>Ignore</b>.
        /// </summary>
        Ignore = 5,

        /// <summary>
        /// The result value of the message box is <b>Yes</b>.
        /// </summary>
        Yes = 6,

        /// <summary>
        /// The result value of the message box is <b>No</b>.
        /// </summary>
        No = 7,
    }
}
