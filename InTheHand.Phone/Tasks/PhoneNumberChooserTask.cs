// In The Hand - .NET Components for Mobility
//
// InTheHand.Phone.Tasks.PhoneNumberChooserTask
// 
// Copyright (c) 2010-2012 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Phone.Tasks
{
    /// <summary>
    /// Allows an application to launch the Contacts application.
    /// Use this to obtain the phone number of a contact selected by the user.
    /// </summary>
    /// <remarks>Launch the Contacts application by calling the <see cref="Show"/> method of the <see cref="PhoneNumberChooserTask"/> object.
    /// Obtain the result of the chooser operation by handling the <see cref="ChooserBase{T}.Completed"/> event.</remarks>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows Embedded Compact 7</description></item>
    /// </list>
    /// </remarks>  
    public sealed class PhoneNumberChooserTask : ChooserBase<PhoneNumberResult>
    {
        private const int ALL_PHONE = 0x1800001f;

        /// <summary>
        /// Shows the Contacts application.
        /// </summary>
        /// <remarks>Obtain the result of the chooser operation by handling the <see cref="ChooserBase{T}.Completed"/> event.</remarks>
        public new void Show()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(Worker), null);
            base.Show();
        }

        private void Worker(object state)
        {
            PhoneNumberResult pr = new PhoneNumberResult();
            NativeMethods.CHOOSECONTACT cc = new NativeMethods.CHOOSECONTACT();

            try
            {
                cc.cbSize = Marshal.SizeOf(cc);
                cc.dwFlags = NativeMethods.CCF.RETURNPROPERTYVALUE | NativeMethods.CCF.HIDENEW;
                cc.cRequiredProperties = 1;
                cc.rgpropidRequiredProperties = Marshal.AllocHGlobal(4);
                Marshal.WriteInt32(cc.rgpropidRequiredProperties, ALL_PHONE);

                int hresult = NativeMethods.ChooseContact(ref cc);

                switch (hresult)
                {
                    case 0:
                        pr.PhoneNumber = Marshal.PtrToStringUni(cc.bstrPropertyValueSelected);
                        pr.TaskResult = TaskResult.OK;
                        break;

                    case unchecked((int)0x80004004):
                        pr.TaskResult = TaskResult.Cancel;
                        break;
                }

            }
            catch (Exception ex)
            {
                pr.TaskResult = TaskResult.None;
                pr.Error = ex;
            }
            finally
            {
                if (cc.rgpropidRequiredProperties != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(cc.rgpropidRequiredProperties);
                    cc.rgpropidRequiredProperties = IntPtr.Zero;
                }
            }

            base.FireCompleted(this, pr, null);
        }
    }
}