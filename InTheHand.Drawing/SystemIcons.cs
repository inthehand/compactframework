// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemIcons.cs" company="In The Hand Ltd">
// Copyright (c) 2010-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace InTheHand.Drawing
{
    /// <summary>
    /// Each property of the SystemIcons class is an <see cref="Icon"/> object for Windows system-wide icons.
    /// </summary>
    /// <remarks>Equivalent to System.Drawing.SystemIcons</remarks>
    public static class SystemIcons
    {
        private enum IDI
        {
            APPLICATION = 32512,
            HAND = 32513,
            QUESTION = 32514,
            EXCLAMATION = 32515,
            ASTERISK = 32516,
            WINLOGO = 32517,
        }
        static System.Collections.Generic.Dictionary<int, IntPtr> icons = new System.Collections.Generic.Dictionary<int, IntPtr>();

        static SystemIcons()
        {
            IntPtr hGwes = InTheHand.NativeMethods.LoadLibrary("\\windows\\gwes.exe", IntPtr.Zero, 2);
            if (hGwes != IntPtr.Zero)
            {
                try
                {
                    for (int iIcon = 16; iIcon <= 64; iIcon = iIcon + 16)
                    {
                        IntPtr pIcon = NativeMethods.LoadIcon(hGwes, iIcon);

                        if (pIcon != IntPtr.Zero)
                        {
                            icons.Add(iIcon, pIcon);
                        }
                    }
                }
                finally
                {
                    InTheHand.NativeMethods.FreeLibrary(hGwes);
                }
            }
            
        }
        /// <summary>
        /// Gets an <see cref="Icon"/> object that contains the default application icon (WIN32: IDI_APPLICATION).
        /// </summary>
        public static Icon Application
        {
            get
            {
                return IconInTheHand.ExtractAssociatedIcon(InTheHand.Reflection.AssemblyInTheHand.GetModuleFileName());
            }
        }

        /// <summary>
        /// Gets an <see cref="Icon"/> object that contains the system hand icon (WIN32: IDI_HAND).
        /// </summary>
        public static Icon Hand
        {
            get
            {
                return Icon.FromHandle(icons[16]);
            }
        }
        /// <summary>
        /// Gets an <see cref="Icon"/> object that contains the system error icon (WIN32: IDI_ERROR).
        /// </summary>
        public static Icon Error
        {
            get
            {
                return Icon.FromHandle(icons[16]);
            }
        }

        /// <summary>
        /// Gets an <see cref="Icon"/> object that contains the system question icon (WIN32: IDI_QUESTION).
        /// </summary>
        public static Icon Question
        {
            get
            {
                return Icon.FromHandle(icons[32]);
            }
        }
        /// <summary>
        /// Gets an <see cref="Icon"/> object that contains the system exclamation icon (WIN32: IDI_EXCLAMATION).
        /// </summary>
        public static Icon Exclamation
        {
            get
            {
                return Icon.FromHandle(icons[48]);
            }
        }
        /// <summary>
        /// Gets an <see cref="Icon"/> object that contains the system warning icon (WIN32: IDI_WARNING).
        /// </summary>
        public static Icon Warning
        {
            get
            {
                return Icon.FromHandle(icons[48]);
            }
        }
        /// <summary>
        /// Gets an <see cref="Icon"/> object that contains the system asterisk icon (WIN32: IDI_ASTERISK).
        /// </summary>
        public static Icon Asterisk
        {
            get
            {
                return Icon.FromHandle(icons[64]);
            }
        }

        /// <summary>
        /// Gets an <see cref="Icon"/> object that contains the system information icon (WIN32: IDI_INFORMATION).
        /// </summary>
        public static Icon Information
        {
            get
            {
                return Icon.FromHandle(icons[64]);
            }
        }

        /*public static Icon WinLogo
        {
            get
            {
                return Icon.FromHandle(NativeMethods.LoadIcon(InTheHand.Runtime.InteropServices.MarshalHelper.GetHINSTANCE(InTheHand.Reflection.AssemblyHelper.GetEntryAssembly().GetModules()[0]), (int)IDI.WINLOGO));
            }
        }*/
    }
}