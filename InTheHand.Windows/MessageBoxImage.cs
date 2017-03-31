// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageBoxImage.cs" company="In The Hand Ltd">
// Copyright (c) 2010-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Windows
{
    /// <summary>
    /// Specifies the icon that is displayed by a message box.
    /// </summary>
    /// <remarks>For Windows Phone this affects only the sound played.</remarks>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Pocket PC 2003, Windows Mobile 5.0 and later</description></item>
    /// <item><term>Windows Phone</term><description>Windows Phone 7 and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
    /// </list>
    /// </remarks>
    public enum MessageBoxImage
    {
        /// <summary>
        /// No icon is displayed. No sound is played.
        /// </summary>
        None = 0,

        /// <summary>
        /// The message box displays an <b>error</b> icon.
        /// </summary>
        Error = 0x10,

        /// <summary>
        /// The message box displays a <b>hand</b> icon.
        /// </summary>
        Hand = 0x10,

        /// <summary>
        /// The message box displays a <b>stop</b> icon.
        /// </summary>
        Stop = 0x10,

        /// <summary>
        /// The message box displays a <b>question mark</b> icon.
        /// </summary>
        Question = 0x20,

        /// <summary>
        /// The message box displays a <b>warning</b> icon.
        /// Equivalent to the default MessageBox behaviour on Windows Phone.
        /// </summary>
        Warning = 0x30,

        /// <summary>
        /// The message box displays an <b>exclamation mark</b> icon.
        /// </summary>
        Exclamation = 0x30,

        /// <summary>
        /// The message box displays an <b>information</b> icon.
        /// </summary>
        Information = 0x40,

        /// <summary>
        /// The message box displays an <b>asterisk</b> icon.
        /// </summary>
        Asterisk = 0x40,
    }
}
