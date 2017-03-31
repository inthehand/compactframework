// In The Hand - .NET Components for Mobility
//
// InTheHand.Phone.Tasks.PhoneCallTask
// 
// Copyright (c) 2010-2011 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Phone.Tasks
{
    /// <summary>
    /// Allows an application to launch the Phone application.
    /// Use this to allow users to make a phone call from your application.
    /// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Pocket PC 2003 Phone Edition and later, Windows Mobile Version 5.0 and later</description></item>
    /// <item><term>Windows Embedded</term><description>Windows Embedded CE 6.0 and later</description></item>
    /// </list>
    /// </remarks>
    public sealed class PhoneCallTask
    {
        internal static class NativeMethods
        {
            internal static bool hasPhone = System.IO.File.Exists("\\windows\\phone.dll");
            internal static readonly bool hasCellcore = System.IO.File.Exists("\\windows\\cellcore.dll");

            [DllImport("cellcore.dll", EntryPoint = "tapiRequestMakeCall", SetLastError = true)]
            internal extern static int RequestMakeCall(string lpszDestAddress, IntPtr lpszAppName, string lpszCalledParty, IntPtr lpszComment);

            [DllImport("phone.dll", EntryPoint = "PhoneMakeCall")]
            internal static extern int MakeCall(ref PHONEMAKECALLINFO ppmci);

            [StructLayout(LayoutKind.Sequential)]
            internal struct PHONEMAKECALLINFO
            {
                public int cbSize;
                public int dwFlags;

                [MarshalAs(UnmanagedType.LPWStr)]
                public string pszDestAddress;

                private IntPtr pszAppName;

                [MarshalAs(UnmanagedType.LPWStr)]
                public string pszCalledParty;

                private IntPtr pszComment;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneCallTask"/> class.
        /// </summary>
        public PhoneCallTask()
        {
            PromptUser = true;
        }

        /// <summary>
        /// Gets/Sets a value which indicates whether to ask the user to confirm the action.
        /// The default value is True.
        /// </summary>
        public bool PromptUser
        {
            get;
            set;
        }

        /// <summary>
        /// Shows the Phone application.
        /// </summary>
        public void Show()
        {
            int hresult;

            if (string.IsNullOrEmpty(PhoneNumber))
            {
                throw new ArgumentNullException("PhoneNumber");
            }

            if (NativeMethods.hasPhone)
            {
                NativeMethods.PHONEMAKECALLINFO pmc = new NativeMethods.PHONEMAKECALLINFO();
                pmc.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(pmc);
                pmc.pszCalledParty = DisplayName;
                pmc.pszDestAddress = PhoneNumber;
                pmc.dwFlags = PromptUser ? 2 : 1;// Prompt user

                hresult = NativeMethods.MakeCall(ref pmc);
                if (hresult < 0)
                {
                    throw InTheHand.ComponentModel.Win32ExceptionInTheHand.Create(hresult);
                }
            }
            else if (NativeMethods.hasCellcore)
            {
                hresult = NativeMethods.RequestMakeCall(PhoneNumber, IntPtr.Zero, DisplayName, IntPtr.Zero);
                if (hresult < 0)
                {
                    throw InTheHand.ComponentModel.Win32ExceptionInTheHand.Create(hresult);
                }
            }
            else
            {
                string message = null;

                if(!string.IsNullOrEmpty(DisplayName))
                {
                    message = string.Format(Properties.Resources.PhoneCallTask_ShowName, DisplayName, PhoneNumber);
                }
                else
                {
                    message = string.Format(Properties.Resources.PhoneCallTask_Show, PhoneNumber);
                }
                InTheHand.Windows.MessageBox.Show(message, Properties.Resources.Phone);
            }
        }

        /// <summary>
        /// Gets or sets the phone number that is dialed when the Phone application is launched.
        /// </summary>
        /// <value>The phone number that is dialed when the Phone application is launched.</value>
        public string PhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name that is displayed when the Phone application is launched.
        /// </summary>
        /// <value>The name that is displayed when the Phone application is launched.</value>
        public string DisplayName
        {
            get;
            set;
        }
    }
}
