// In The Hand - .NET Components for Mobility
//
// InTheHand.WindowsMobile.Net.InternetSharing
// 
// Copyright (c) 2008-2010 In The Hand Ltd, All rights reserved.

using System;

using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace InTheHand.WindowsMobile.Net
{
    /// <summary>
    /// Specifies the local connection over which to share the cellular connection.
    /// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 AKU 3 and later</description></item>
    /// </list>
    /// </remarks>
    public enum SharingConnection
    {
        /// <summary>
        /// Shares the cellular connection over the USB interface.
        /// </summary>
        Usb,
        /// <summary>
        /// Shares the cellular connection over Bluetooth using the PAN profile.
        /// </summary>
        Bluetooth,
    }

    /// <summary>
    /// Enables you to programmatically enable and disable Internet Sharing via USB or Bluetooth.
    /// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 AKU 3 and later</description></item>
    /// </list>
    /// </remarks>
    public static class InternetSharing
    {
        /// <summary>
        /// Enables Internet Sharing.
        /// </summary>
        /// <param name="sharingConnection">Type of connection to share over.</param>
        /// <param name="cellularConnection">Name of the cellular (e.g. GPRS) connection to share.</param>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 AKU 3 and later</description></item>
        /// </list>
        /// </remarks>
        public static void Enable(SharingConnection sharingConnection, string cellularConnection)
        {
            NativeMethods.InternetSharingConnectionType ict = NativeMethods.InternetSharingConnectionType.BTPan;
            string localConnection = "BTPAN1";
            switch (sharingConnection)
            {
                case SharingConnection.Usb:
                    localConnection = "USB";
                    ict = NativeMethods.InternetSharingConnectionType.Rndis;
                    break;
                case SharingConnection.Bluetooth:
                    localConnection = "BTPAN1";
                    ict = NativeMethods.InternetSharingConnectionType.BTPan;
                    break;
            }

            // rndis "USB"
            int result = NativeMethods.InternetSharingEnable(ict, localConnection, NativeMethods.InternetSharingConnectionType.CellularData, cellularConnection);
            if(result != 0)
            {
                throw InTheHand.ComponentModel.Win32ExceptionInTheHand.Create(result);
            }
        }

        /// <summary>
        /// Disables Internet Sharing.
        /// </summary>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 AKU 3 and later</description></item>
        /// </list>
        /// </remarks>
        public static void Disable()
        {
            int result = NativeMethods.InternetSharingDisable();
            if (result != 0)
            {
                throw InTheHand.ComponentModel.Win32ExceptionInTheHand.Create(result);
            }
        }

        internal static class NativeMethods
        {
            [DllImport("intshare.dll")]
            internal static extern int InternetSharingEnable(
                InternetSharingConnectionType privateType, 
                string szPrivateInstance, 
                InternetSharingConnectionType publicType, 
                string szPublicConnection);

            [DllImport("intshare.dll")]
            internal static extern int InternetSharingDisable();

            internal enum InternetSharingConnectionType
            {
                Rndis = 0,
                BTPan,
                GenericNdis,
                CellularData,
                Undefined = 0xff
            } 
        }
    }
}
