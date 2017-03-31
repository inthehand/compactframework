// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Clipboard.cs" company="In The Hand Ltd">
// Copyright (c) 2010-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Windows
{
    /// <summary>
    /// Provides static methods that facilitate transferring data to and from the system Clipboard.
    /// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Pocket PC 2003, Windows Mobile 5.0 and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
    /// </list>
    /// </remarks>
    public static class Clipboard
    {
        /// <summary>
        /// Queries the clipboard for the presence of data in the UnicodeText format.
        /// </summary>
        /// <returns>true if the system clipboard contains Unicode text data; otherwise, false.</returns>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Pocket PC 2003, Windows Mobile 5.0 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
        /// </list>
        /// </remarks>
        public static bool ContainsText()
        {
            NativeMethods.OpenClipboard(IntPtr.Zero);

            try
            {
                return NativeMethods.IsClipboardFormatAvailable(NativeMethods.CF_UNICODETEXT);
            }
            finally
            {
                NativeMethods.CloseClipboard();
            }
        }

        /// <summary>
        /// Retrieves Unicode text data from the system clipboard, if Unicode text data exists.
        /// </summary>
        /// <returns>If Unicode text data is present on the system clipboard, returns a string that contains the Unicode text data.
        /// Otherwise, returns an empty string.</returns>
        /// <seealso cref="SetText"/>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Pocket PC 2003, Windows Mobile 5.0 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
        /// </list>
        /// </remarks>
        public static string GetText()
        {
            string s = string.Empty;
                
            NativeMethods.OpenClipboard(IntPtr.Zero);
            try
            {
                IntPtr ptr = NativeMethods.GetClipboardData(NativeMethods.CF_UNICODETEXT);
                if (ptr != IntPtr.Zero)
                {
                    s = Marshal.PtrToStringUni(ptr);
                }
            }
            finally
            {
                NativeMethods.CloseClipboard();
            }

            return s;
        }

        /// <summary>
        /// Sets Unicode text data to store on the clipboard, for later access with <see cref="GetText"/>.
        /// </summary>
        /// <param name="text">A string that contains the Unicode text data to store on the clipboard.</param>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Pocket PC 2003, Windows Mobile 5.0 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
        /// </list>
        /// </remarks>
        public static void SetText(string text)
        {
            NativeMethods.OpenClipboard(IntPtr.Zero);
            try
            {
                IntPtr ptr = InTheHand.Runtime.InteropServices.MarshalInTheHand.StringToHGlobalUni(text + "\0");
                NativeMethods.SetClipboardData(NativeMethods.CF_UNICODETEXT, ptr);
            }
            finally
            {
                NativeMethods.CloseClipboard();
            }
        }

        private static class NativeMethods
        {
            internal const int CF_UNICODETEXT = 13;

            [DllImport("coredll")]
            internal static extern bool OpenClipboard(IntPtr hWndNewOwner); 

            [DllImport("coredll")]
            internal static extern bool CloseClipboard();

            [DllImport("coredll")]
            internal static extern bool IsClipboardFormatAvailable(int format); 

            [DllImport("coredll")]
            internal static extern IntPtr GetClipboardData(int uFormat);
 
            [DllImport("coredll")]
            internal static extern IntPtr SetClipboardData(int uFormat, IntPtr hMem); 
        }
    }
}
