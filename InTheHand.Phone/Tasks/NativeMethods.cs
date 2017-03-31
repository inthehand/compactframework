// In The Hand - .NET Components for Mobility
//
// InTheHand.Phone.Tasks.NativeMethods
// 
// Copyright (c) 2003-2012 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Text;
using InTheHand.WindowsCE.Forms;
using Microsoft.WindowsCE.Forms;

namespace InTheHand.Phone.Tasks
{

    internal static class NativeMethods
    {
        internal static readonly bool UseTMail = (System.Environment.OSVersion.Version < new Version(5, 1) && SystemSettingsInTheHand.Platform != WinCEPlatform.WinCEGeneric) | (System.Environment.OSVersion.Version < new Version(7,0) && SystemSettingsInTheHand.Platform == WinCEPlatform.WinCEGeneric);
       
        #region Choose Contact

        [DllImport("pimstore", EntryPoint = "ChooseContact", SetLastError = false)]
        internal static extern int ChooseContact(ref CHOOSECONTACT lpcc);

        [StructLayout(LayoutKind.Sequential)]
        internal struct CHOOSECONTACT
        {
            public int cbSize;

            public IntPtr hwndOwner;

            public CCF dwFlags;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpstrTitle;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpstrChoosePropertyText;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpstrRestrictContacts;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpstrIncrementalFilter;

            public int cRequiredProperties;

            public IntPtr rgpropidRequiredProperties;

            public int oidContactID;

            public IntPtr bstrContactName;

            public int propidSelected;

            public IntPtr bstrPropertyValueSelected;
        }

        [Flags()]
        internal enum CCF : int
        {
            DEFAULT = 0x0000,//Default view. The Property Chooser dialog box will be used when the selected Contact has values for two or more properties specified in CHOOSECONTACT::rgpropidRequiredProperties. The "New" button is displayed.
            HIDENEW = 0x0001,// Allows the user to create a new Contact from the Contact Chooser control.
            CHOOSECONTACTONLY = 0x0002,// When set, the Property Chooser dialog box does not appear.
//The first non-blank property specified in CHOOSECONTACT::rgpropidRequiredProperties is chosen.
            CHOOSEPROPERTYONLY = 0x0004,//When set, the Contact Chooser dialog box does not appear.
//The OID of the Contact to use is retrieved from the CHOOSECONTACT::oidContactID property.
//Note: When you set both the CCF_RETURNCONTACTNAME and CCF_CHOOSEPROPERTYONLY flags, the contact name is not returned.  
            RETURNCONTACTNAME = 0x0008,// When set, the Contact name is returned in the CHOOSECONTACT::bstrContactName property.
//Note: When you set both the CCF_RETURNCONTACTNAME and CCF_CHOOSEPROPERTYONLY flags, the contact name is not returned.  
            RETURNPROPERTYVALUE = 0x0010,// When set, the property value is returned in the CHOOSECONTACT::bstrPropertyValueSelected property.
            FILTERREQUIREDPROPERTIES = 0x0020,// Filter required properties.
            NOUIONSINGLEORNOMATCH = 0x0040,// When set, neither the Contact Chooser, nor the Property Chooser dialog boxes display for a single match, or for no matches.
            NOUI = 0x0080,// When set, neither the Contact Chooser, nor the Property Chooser dialog boxes display.
            ENABLEGAL = 0x0100,//When set, gives users the ability to search for contacts in the Global Address Book (GAL).
//Note: When the user views contacts in the Contacts application, and views them by Company, the Find Online option becomes unavailable (appears dimmed). For the Find Online option to reappear, the user must switch to View by Name (Menu > View By > Name).  
            ALLOWNEWCONTACTSELECTION = 0x0200,// When set, adds a New Contact option to the top of the Contact Chooser Listview. SIM Contact functionality is disabled. The SK2 menu is replaced with a Cancel button.
            INCLUDESIM = 0x8000,// When set, contacts stored on the SIM are also included as candidates for selection. SIM Contact items have a PIMPR_CONTACT_TYPE property value of PIMPR_CONTACTTYPE::PIMPR_CONTACTTYPE_SIM. For more information, see IPOlItems3::IncludeSimContacts.
        }
        #endregion

        internal static void ComposeMessage(string toAddresses, string ccAddresses, string bccAddresses, string subject, string body, string[] attachments, string accountName, string messageClass)
        {
            if (!UseTMail)
            {
                NativeMethods.MAILCOMPOSEFIELDS mcf = new NativeMethods.MAILCOMPOSEFIELDS();
                mcf.cbSize = Marshal.SizeOf(typeof(NativeMethods.MAILCOMPOSEFIELDS));
                if (!string.IsNullOrEmpty(accountName))
                {
                    mcf.dwFlags = NativeMethods.MCF.ACCOUNT_IS_NAME;
                    mcf.pszAccount = accountName;
                }
                mcf.pszTo = toAddresses;
                mcf.pszCc = ccAddresses;
                mcf.pszBcc = bccAddresses;
                mcf.pszSubject = subject;
                mcf.pszBody = body;
                mcf.pszMsgClass = messageClass;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (attachments != null)
                {
                    foreach (string a in attachments)
                    {
                        sb.Append(a + "\0");
                    }
                    sb.Append("\0");
                    mcf.pszAttachments = sb.ToString();
                    mcf.cAttachments = attachments.Length;
                }


                int result = MailComposeMessage(ref mcf);

                Marshal.ThrowExceptionForHR(result);
            }
            else
            {
                System.Text.StringBuilder sbArguments = new System.Text.StringBuilder();
                sbArguments.Append("-service \"" + accountName + "\"");

                if (!String.IsNullOrEmpty(toAddresses))
                {
                    sbArguments.Append(" -to \"" + toAddresses + "\"");
                }
                if (!String.IsNullOrEmpty(ccAddresses))
                {
                    sbArguments.Append(" -cc \"" + ccAddresses + "\"");
                }
                if (!String.IsNullOrEmpty(bccAddresses))
                {
                    sbArguments.Append(" -bcc \"" + bccAddresses + "\"");
                }
                if (!String.IsNullOrEmpty(subject))
                {
                    sbArguments.Append(" -subject \"" + subject + "\"");
                }
                if (!String.IsNullOrEmpty(body))
                {
                    sbArguments.Append(" -body \"" + body + "\"");
                }
                if (attachments != null)
                {
                    if (attachments.Length > 0)
                    {
                        if (!String.IsNullOrEmpty(attachments[0]))
                        {
                            sbArguments.Append(" -attach \"" + attachments[0] + "\"");
                        }
                    }
                }
                System.Diagnostics.Process.Start("tmail.exe", sbArguments.ToString()); //"-service \"" + accountName + "\" -to \"" + toAddresses + "\" -cc \"" + ccAddresses + "\" -bcc \"" + bccAddresses + "\" -subject \"" + subject + "\" -body \"" + body + "\" -attach \"" + attachments[0] + "\"");
            }
        }

        [DllImport("cemapi")]
        internal static extern int MailComposeMessage(ref MAILCOMPOSEFIELDS pmcf);

        [StructLayout(LayoutKind.Sequential)]
        internal struct MAILCOMPOSEFIELDS
        {
            public int cbSize;
            public MCF dwFlags;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszTo;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszCc;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszBcc;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszSubject;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszBody;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszAttachments;

            public int cAttachments;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszAccount;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszMsgClass;
        }

        [Flags()]
        internal enum MCF : int
        {
            ACCOUNT_IS_NAME = 0x01, //The name of the account, contained the MAILCOMPOSEFIELDS structure pszAccount member.
            ACCOUNT_IS_TRANSPORT = 0x02, // The name of the transport, contained the MAILCOMPOSEFIELDS structure pszAccount member.
            MAILTO_FORMAT = 0x04, // The recipient address in "mailto" format, contained the MAILCOMPOSEFIELDS structure pszTo member.
            RUN_IN_BACKGROUND = 0x08, // Run the messaging application in the background. Used with MailSyncMessages.
        }
    }
}
