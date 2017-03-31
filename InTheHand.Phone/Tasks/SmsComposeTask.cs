// In The Hand - .NET Components for Mobility
//
// InTheHand.Phone.Tasks.SmsComposeTask
// 
// Copyright (c) 2010-2012 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Phone.Tasks
{
    /// <summary>
    /// Launches the Messaging application with a new SMS message displayed.
    /// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Pocket PC 2003 Phone Edition and later, Windows Mobile Version 5.0 and later</description></item>
    /// <item><term>Windows Embedded</term><description>Windows Embedded CE 6 and later</description></item>
    /// </list>
    /// </remarks>  
    public sealed class SmsComposeTask
    {
        /// <summary>
        /// Shows the Messaging application.
        /// </summary>
        public void Show()
        {
            NativeMethods.ComposeMessage(To, null, null, null, Body, null, "SMS", "IPM.SMStext");
        }

        /// <summary>
        /// Gets or sets the body text of the new SMS message.
        /// </summary>
        public string Body
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the recipient list for the new SMS message.
        /// </summary>
        public string To
        {
            get;
            set;
        }
    }
}