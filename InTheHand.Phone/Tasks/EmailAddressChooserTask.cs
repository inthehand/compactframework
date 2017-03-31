// In The Hand - .NET Components for Mobility
//
// InTheHand.Phone.Tasks.EmailAddressChooserTask
// 
// Copyright (c) 2010-2012 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Phone.Tasks
{
    /// <summary>
    /// Allows an application to launch the Contacts application.
    /// Use this to obtain the email address of a contact selected by the user.
    /// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows Embedded Compact 7</description></item>
    /// </list>
    /// </remarks>  
    public sealed class EmailAddressChooserTask : ChooserBase<EmailResult>
    {
        private const int ALL_EMAIL = 0x1801001f;
        /// <summary>
        /// Shows the email address chooser application.
        /// </summary>
        public new void Show()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(Worker), null);
            base.Show();
        }

        private void Worker(object state)
        {
            EmailResult er = new EmailResult();
            NativeMethods.CHOOSECONTACT cc = new NativeMethods.CHOOSECONTACT();

            try
            {
                cc.cbSize = Marshal.SizeOf(cc);
                cc.dwFlags = NativeMethods.CCF.RETURNPROPERTYVALUE | NativeMethods.CCF.HIDENEW;
                cc.cRequiredProperties = 1;
                cc.rgpropidRequiredProperties = Marshal.AllocHGlobal(4);
                Marshal.WriteInt32(cc.rgpropidRequiredProperties, ALL_EMAIL);

                int hresult = NativeMethods.ChooseContact(ref cc);

                switch (hresult)
                {
                    case 0:
                        er.Email = Marshal.PtrToStringUni(cc.bstrPropertyValueSelected);
                        er.TaskResult = TaskResult.OK;
                        break;

                    case unchecked((int)0x80004004):
                        er.TaskResult = TaskResult.Cancel;
                        break;
                }

            }
            catch (Exception ex)
            {
                er.TaskResult = TaskResult.None;
                er.Error = ex;
            }
            finally
            {
                if (cc.rgpropidRequiredProperties != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(cc.rgpropidRequiredProperties);
                    cc.rgpropidRequiredProperties = IntPtr.Zero;
                }
            }

            base.FireCompleted(this, er, null);
        }
    }
}