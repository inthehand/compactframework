// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Icon.cs" company="In The Hand Ltd">
// Copyright (c) 2008-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Drawing;
using System.Runtime.InteropServices;

namespace InTheHand.Drawing
{
    /// <summary>
    /// Provides supporting methods for <see cref="Icon"/>.
    /// </summary>
    /// <seealso cref="Icon"/>
    public static class IconInTheHand
    {
        /// <summary>
        /// Returns an icon representation of an image contained in the specified file.
        /// </summary>
        /// <param name="filename">The path to the file that contains an image.</param>
        /// <returns>The Icon representation of the image contained in the specified file.</returns>
        public static Icon ExtractAssociatedIcon(string filename)
        {
            return ExtractAssociatedIcon(filename, true);
        }

        /// <summary>
        /// Returns an icon representation of an image contained in the specified file.
        /// </summary>
        /// <param name="filename">The path to the file that contains an image.</param>
        /// <param name="largeIcon">Specifies whether to retrieve the large (Default) or small size icon.</param>
        /// <returns>The Icon representation of the image contained in the specified file.</returns>
        public static Icon ExtractAssociatedIcon(string filename, bool largeIcon)
        {
            NativeMethods.SHFILEINFO shfi = new NativeMethods.SHFILEINFO();
            int result = NativeMethods.GetFileInfo(filename, 0, ref shfi, Marshal.SizeOf(shfi), NativeMethods.SHGFI.ICON | (largeIcon ? 0 : NativeMethods.SHGFI.SMALLICON));
            if (result == 0)
            {
                throw InTheHand.ComponentModel.Win32ExceptionInTheHand.Create();
            }
            return Icon.FromHandle(shfi.hIcon);
        }
    }
}