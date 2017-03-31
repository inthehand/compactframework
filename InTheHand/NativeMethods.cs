// In The Hand - .NET Components for Mobility
//
// InTheHand.NativeMethods
// 
// Copyright (c) 2003-2012 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;
using InTheHand.Text;
using InTheHand.WindowsCE.Forms;
using Microsoft.WindowsCE.Forms;

namespace InTheHand
{
    internal static class NativeMethods
    {
        internal static readonly bool IsMobile2003 = (System.Environment.OSVersion.Version >= new Version(4, 2) && SystemSettingsInTheHand.Platform != WinCEPlatform.WinCEGeneric);
        internal static readonly bool IsMobile5 = (System.Environment.OSVersion.Version >= new Version(5, 1) && SystemSettingsInTheHand.Platform != WinCEPlatform.WinCEGeneric);
        internal static readonly bool IsMobile6 = (System.Environment.OSVersion.Version >= new Version(5, 2) && SystemSettingsInTheHand.Platform != WinCEPlatform.WinCEGeneric);
        internal static readonly bool IsMobile61 = (System.Environment.OSVersion.Version >= new Version(5, 2, 19000) && SystemSettingsInTheHand.Platform != WinCEPlatform.WinCEGeneric);
        internal static readonly bool IsMobile611 = (System.Environment.OSVersion.Version >= new Version(5, 2, 20000) && SystemSettingsInTheHand.Platform != WinCEPlatform.WinCEGeneric);
        internal static readonly bool IsMobile65 = (System.Environment.OSVersion.Version >= new Version(5, 2, 21000) && SystemSettingsInTheHand.Platform != WinCEPlatform.WinCEGeneric);
        internal static readonly bool IsMobile653 = (System.Environment.OSVersion.Version >= new Version(5, 2, 23090) && SystemSettingsInTheHand.Platform != WinCEPlatform.WinCEGeneric);

        // Handle
        [DllImport("coredll", EntryPoint = "CreateFile")]
        internal static extern IntPtr CreateFile(
  string lpFileName,
  uint dwDesiredAccess,
  uint dwShareMode,
  IntPtr lpSecurityAttributes,
  uint dwCreationDisposition,
  uint dwFlagsAndAttributes,
  uint hTemplateFile
);

        // Handle
        [DllImport("coredll", EntryPoint="CloseHandle")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseHandle(IntPtr hObject);

        internal enum SPI
        {
            GETPLATFORMVERSION = 224,
            GETPLATFORMTYPE = 257,
            GETOEMINFO = 258,
            GETPROJECTNAME=259,
            GETPLATFORMNAME=260,
            //GETBOOTMENAME = 261,
            GETPLATFORMMANUFACTURER=262,
            GETUUID=263,
            //GETGUIDPATTERN=264,
        }


        [DllImport("coredll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SystemParametersInfo(
            SPI uiAction,
            int uiParam,
            byte[] pvParam,
            int fWinIni);

        internal static bool SystemParametersInfoString(SPI parameter, out string parameterValue)
        {
            parameterValue = null;
            byte[] buffer = new byte[EnvironmentInTheHand.MaxPath];
            bool success = SystemParametersInfo(parameter, buffer.Length, buffer, 0);
            if (success)
            {
                parameterValue = System.Text.Encoding.Unicode.GetString(buffer);
                int nullpos = parameterValue.IndexOf('\0');
                if (nullpos > -1)
                {
                    parameterValue = parameterValue.Substring(0, nullpos);
                }

                return true;
            }

            return false;
        }
        
        
       
        
        // Resources
        [DllImport("coredll", EntryPoint = "LoadString", SetLastError = true)]
        internal static extern IntPtr LoadString(IntPtr hInstance, int uID, byte[] buffer, int cchBufferMax);
        
        [DllImport("coredll", EntryPoint = "LoadLibraryExW", SetLastError = true)]
        internal static extern IntPtr LoadLibrary(string lpLibFileName, IntPtr hFile, int flags);

        [DllImport("coredll", EntryPoint = "LoadIcon", SetLastError = true)]
        internal static extern IntPtr LoadIcon(IntPtr hInstance, int iconName);

        [DllImport("coredll", EntryPoint = "FreeLibrary", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool FreeLibrary(IntPtr hLibModule);
        

        [DllImport("coredll")]
        internal static extern int MessageBox(IntPtr hWnd, string text, string caption, int uType);
        
	}
}
