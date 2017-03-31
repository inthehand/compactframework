// In The Hand - .NET Components for Mobility
//
// InTheHand.Phone.Tasks.EmailComposeTask
// 
// Copyright (c) 2010-12 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Phone.Tasks
{
    /// <summary>
    /// Allows an application to launch the email application with a new message displayed.
    /// Use this to allow users to send email from your application.
    /// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Pocket PC 2003 and later, Windows Mobile Version 5.0 and later</description></item>
    /// <item><term>Windows Embedded</term><description>Windows CE 4.1 and later</description></item>
    /// </list>
    /// </remarks>  
    public sealed class EmailComposeTask
    {
        /// <summary>
        /// Shows the email application with a new message displayed.
        /// </summary>
        public void Show()
        {
            NativeMethods.ComposeMessage(To, Cc, Bcc, Subject, Body, null, null, "IPM.Note");
        }

        /// <summary>
        /// The body of the new email message.
        /// </summary>
        /// <remarks>The Body property does not support HTML formatting.
        /// If the content assigned to the Body property happens to work with the codepage that maps to the current locale settings for the device, it will be sent using that codepage. 
        /// If the codepage mapped to the current locale setting cannot render the content, UTF-8 encoding will be used.  </remarks>
        public string Body
        {
            get;
            set;
        }

        /// <summary>
        /// The recipients on the To line of the new email message.
        /// </summary>
        public string To
        {
            get;
            set;
        }

        /// <summary>
        /// The recipients on the cc line of the new email message.
        /// </summary>
        public string Cc
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the recipients on the Bcc line of the new email message.
        /// </summary>
        public string Bcc
        {
            get;
            set;
        }

        /// <summary>
        /// The subject of the new email message.
        /// </summary>
        public string Subject
        {
            get;
            set;
        }
    }
}