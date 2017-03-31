// In The Hand - .NET Components for Mobility
//
// InTheHand.ComponentModel.Win32Exception
// 
// Copyright (c) 2010-2014 In The Hand Ltd, All rights reserved.

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace InTheHand.ComponentModel
{
    /// <summary>
    /// Provides helper functions for the <see cref="Win32Exception"/> class.
    /// </summary>
    /// <seealso cref="Win32Exception"/>
    public static class Win32ExceptionInTheHand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Win32Exception"/> class with the last Win32 error that occurred.
        /// The message is retrieved from the system where available.
        /// </summary>
        /// <returns>A <see cref="Win32Exception"/> containing the error code and message.</returns>
        /// <remarks>The detailed description of the error is determined by the Win32 error message associated with the error.
        /// <para>This method uses the <see cref="Marshal.GetLastWin32Error"/> method of <see cref="Marshal"/> to get its error code.</para></remarks>
        /// <example>The following code example shows how to replace regular <see cref="Win32Exception"/> code with this helper method:-
        /// <code lang="cs">
        /// // throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
        /// throw InTheHand.ComponentModel.Win32ExceptionHelper.Create();
        /// </code>
        /// <code lang="vbnet">
        /// ' Throw New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error())
        /// Throw InTheHand.ComponentModel.Win32ExceptionHelper.Create()
        /// </code></example>
        public static Win32Exception Create()
        {
            return Create(Marshal.GetLastWin32Error());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Win32Exception"/> class with the specified error.
        /// The message is retrieved from the system where available.
        /// </summary>
        /// <param name="error">The Win32 error code associated with this exception.</param>
        /// <returns>A <see cref="Win32Exception"/> containing the error code and message.</returns>
        /// <remarks>The detailed description of the error is determined by the Win32 error message associated with the error.</remarks>
        /// <example>The following code example shows how to replace regular <see cref="Win32Exception"/> code with this helper method:-
        /// <code lang="cs">
        /// // throw new System.ComponentModel.Win32Exception(result);
        /// throw InTheHand.ComponentModel.Win32ExceptionHelper.Create(result);
        /// </code>
        /// <code lang="vbnet">
        /// ' Throw New System.ComponentModel.Win32Exception(result)
        /// Throw InTheHand.ComponentModel.Win32ExceptionHelper.Create(result)
        /// </code></example>
        public static Win32Exception Create(int error)
        {
            return new Win32Exception(error, GetErrorMessage(error));
        }

        // Uses FormatMessage to retrieve a string description if possible
        internal static string GetErrorMessage(int error)
        {
            StringBuilder lpBuffer = new StringBuilder(0x200);
            int chars = NativeMethods.FormatMessage(NativeMethods.FORMAT_MESSAGE.FROM_SYSTEM | NativeMethods.FORMAT_MESSAGE.IGNORE_INSERTS | NativeMethods.FORMAT_MESSAGE.ARGUMENT_ARRAY, IntPtr.Zero, error, 0, lpBuffer, lpBuffer.Capacity + 1, IntPtr.Zero);
            if (chars == 0)
            {
                // If it fails construct a generic error message
                // return "Unknown error (0x" + Convert.ToString(error, 0x10) + ")";
                return string.Format(Properties.Resources.Unknown_Error, error);
            }

            return lpBuffer.ToString(0, chars);
        }

        private static class NativeMethods
        {
            [DllImport("coredll")]
            internal static extern int FormatMessage(FORMAT_MESSAGE dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, 
                StringBuilder lpBuffer, int nSize, IntPtr Arguments);

            [Flags()]
            internal enum FORMAT_MESSAGE
            {
                //ALLOCATE_BUFFER = 0x00000100,
                IGNORE_INSERTS = 0x00000200,
                //FROM_STRING = 0x00000400,
                //FROM_HMODULE = 0x00000800,
                FROM_SYSTEM = 0x00001000,
                ARGUMENT_ARRAY = 0x00002000,
                //MAX_WIDTH_MASK = 0x000000FF,
            }
        }
    }
}
