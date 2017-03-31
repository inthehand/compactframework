// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.ConnectionManager.ConnectionManager
// 
// Copyright (c) 2003-2010 In The Hand Ltd, All rights reserved.

using System;
using System.Collections;
using System.Runtime.InteropServices;
using Microsoft.WindowsCE.Forms;

namespace InTheHand.Net.ConnectionManager
{
    /// <summary>
    /// Manages network connections for phone-enabled devices, regardless of the service provider used for establishing the connection. 
    /// ConnectionManager provides a fast and transparent way of making connection choices for an application. 
    /// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile 2003 and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows CE 4.1 and later, Windows Embedded NavReady 2009</description></item>
    /// </list>
    /// </remarks>
    public sealed class ConnectionManager
    {
        private IntPtr handle;
        private CONNMGR_CONNECTIONINFO connectionInfo;
        private ConnectionManagerMessageWindow messageWindow;

        /// <summary>
        /// Initializes a new instance of the ConnectionManager class.
        /// </summary>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile 2003 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows Embedded Compact 6.0 and later, Windows Embedded NavReady 2009</description></item>
        /// </list>
        /// </remarks>
        public ConnectionManager()
        {
            messageWindow = new ConnectionManagerMessageWindow(this);

            connectionInfo = new CONNMGR_CONNECTIONINFO();
            connectionInfo.cbSize = Marshal.SizeOf(connectionInfo);
            connectionInfo.dwFlags = CONNMGR_FLAG.NO_ERROR_MSGS;
            connectionInfo.dwParams = CONNMGR_PARAM.GUIDDESTNET;
            connectionInfo.dwPriority = ConnectionPriority.HighPriorityBackground;
            connectionInfo.hWnd = messageWindow.Hwnd;
            connectionInfo.uMsg = ConnectionManagerMessageWindow.WM_ConnectionManager;
        }

        /// <summary>
        /// Creates an asynchronous connection request.
        /// </summary>
        /// <param name="destination">Guid of the destination network or specific network connection to use.</param>
        /// <seealso cref="Destination"/>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile 2003 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE 4.1 and later, Windows Embedded NavReady 2009</description></item>
        /// </list>
        /// </remarks>
        public void EstablishConnection(Guid destination)
        {
            connectionInfo.guidDestNet = destination;
            if (NativeMethods.hasCellcore)
            {
                int hresult = NativeMethods.EstablishConnection(ref connectionInfo, out handle);
                Marshal.ThrowExceptionForHR(hresult);
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        /// <summary>
        /// Creates a connection request but returns information only after the connection has been established or has failed.
        /// </summary>
        /// <param name="destination"><see cref="Guid"/> of the destination network or specific network connection to use.</param>
        /// <param name="timeout">Specifies a timeout, for connection establishment.</param>
        /// <returns>Resulting <see cref="ConnectionStatus"/> after success or expiration of the <paramref name="timeout"/>.</returns>
        /// <seealso cref="Destination"/> 
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile 2003 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows Embedded Compact 6.0 and later, Windows Embedded NavReady 2009</description></item>
        /// </list>
        /// </remarks>
        public ConnectionStatus EstablishConnectionSync(Guid destination, TimeSpan timeout)
        {
            if (!NativeMethods.hasCellcore)
            {
                throw new PlatformNotSupportedException();
            }

            ConnectionStatus status = ConnectionStatus.Unknown;
            connectionInfo.guidDestNet = destination;
            int hresult = NativeMethods.EstablishConnectionSync(ref connectionInfo, out handle, Convert.ToInt32(timeout.TotalMilliseconds), out status);
            Marshal.ThrowExceptionForHR(hresult);
            
            return status;
        }

        /// <summary>
        /// Returns the details of available networks.
        /// </summary>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile 2003 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows Embedded Compact 6.0 and later, Windows Embedded NavReady 2009</description></item>
        /// </list>
        /// </remarks>
        public static DestinationInfo[] Destinations
        {
            get
            {
                if (NativeMethods.hasCellcore)
                {
                    int idest = 0;

                    System.Collections.Generic.List<DestinationInfo> dests = new System.Collections.Generic.List<DestinationInfo>();
                    CONNMGR_DESTINATION_INFO di;
                    int result = NativeMethods.EnumDestinations(idest, out di);

                    while (result == 0)
                    {
                        dests.Add(new DestinationInfo(di));
                        idest++;
                        //get next

                        result = NativeMethods.EnumDestinations(idest, out di);
                    }


                    return dests.ToArray();
                }

                return null;
            }
        }

        private ConnectionStatus status;
        /// <summary>
        /// Returns the status of the current connection.
        /// </summary>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile 2003 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE.NET 4.1 and later, Windows Embedded NavReady 2009</description></item>
        /// </list>
        /// </remarks>
        public ConnectionStatus ConnectionStatus
        {
            get
            {
                if (NativeMethods.hasCellcore)
                {
                    status = ConnectionStatus.Unknown;
                    if (handle != IntPtr.Zero)
                    {
                        int hresult = NativeMethods.ConnectionStatus(handle, out status);
                        Marshal.ThrowExceptionForHR(hresult);
                    }
                }
                return status;
            }
        }

        /// <summary>
        /// Returns an array of <see cref="ConnectionDetailedStatus"/> objects describing the state of all the available network connections.
        /// </summary>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile 2003 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows Embedded Compact 6.0 and later, Windows Embedded NavReady 2009</description></item>
        /// </list>
        /// </remarks>
        public static ConnectionDetailedStatus[] DetailedStatus
        {
            get
            {
                if (NativeMethods.hasCellcore)
                {
                    int bufferLen = 0;
                    IntPtr buffer = IntPtr.Zero;

                    int result = NativeMethods.QueryDetailedStatus(buffer, ref bufferLen);
                    buffer = Marshal.AllocHGlobal(bufferLen);
                    try
                    {

                        result = NativeMethods.QueryDetailedStatus(buffer, ref bufferLen);
                        CONNMGR_CONNECTION_DETAILED_STATUS cds = (CONNMGR_CONNECTION_DETAILED_STATUS)Marshal.PtrToStructure(buffer, typeof(CONNMGR_CONNECTION_DETAILED_STATUS));

                        System.Collections.Generic.List<ConnectionDetailedStatus> al = new System.Collections.Generic.List<ConnectionDetailedStatus>();

                        bool moreConnections = true;
                        while (moreConnections)
                        {
                            al.Add(new ConnectionDetailedStatus(cds));
                            if (cds.pNext != IntPtr.Zero)
                            {
                                cds = (CONNMGR_CONNECTION_DETAILED_STATUS)Marshal.PtrToStructure(cds.pNext, typeof(CONNMGR_CONNECTION_DETAILED_STATUS));
                            }
                            else
                            {
                                moreConnections = false;
                            }
                        }

                        return al.ToArray();
                    }
                    finally
                    {
                        if (buffer != IntPtr.Zero)
                        {
                            Marshal.FreeHGlobal(buffer);
                        }
                    }
                }

                return null;
            }
        }

        //raises the connectionstatuschanged event
        internal void OnConnectionStatusChanged(ConnectionStatusChangedEventArgs e)
        {
            if (ConnectionStatusChanged != null)
            {
                ConnectionStatusChanged(e);
            }
        }

        /// <summary>
        /// Occurs when the connection status changes.
        /// </summary>
        public event ConnectionStatusChangedEventHandler ConnectionStatusChanged;

        /// <summary>
        /// Deletes the specified connection request, which could drop the physical connection.
        /// </summary>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile 2003 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE.NET 4.1 and later, Windows Embedded NavReady 2009</description></item>
        /// </list>
        /// </remarks>
        public void ReleaseConnection()
        {
            if (NativeMethods.hasCellcore)
            {
                if (handle != IntPtr.Zero)
                {
                    int hresult = NativeMethods.ReleaseConnection(handle, 0);
                    handle = IntPtr.Zero;
                    Marshal.ThrowExceptionForHR(hresult);
                }
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        /// <summary>
        /// Gets or sets the priority of the current connection request.
        /// </summary>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile 2003 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows Embedded Compact 6.0 and later, Windows Embedded NavReady 2009</description></item>
        /// </list>
        /// </remarks>
        public ConnectionPriority ConnectionPriority
        {
            get
            {
                if (!NativeMethods.hasCellcore)
                {
                    return ConnectionPriority.HighPriorityBackground;
                }

                return connectionInfo.dwPriority;
            }
            set
            {
                if (!NativeMethods.hasCellcore)
                {
                    throw new PlatformNotSupportedException();
                }

                connectionInfo.dwPriority = value;
                if (handle != IntPtr.Zero)
                {
                    int hresult = NativeMethods.SetConnectionPriority(handle, value);
                    Marshal.ThrowExceptionForHR(hresult);
                }
            }
        }

        /// <summary>
        /// Maps a Url to the globally unique identifier (<see cref="Guid"/>) of the network to which the device is connected.
        /// </summary>
        /// <param name="url">The URL to be mapped.</param>
        /// <returns>Array of destination network identifiers.</returns>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile 2003 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows Embedded Compact 6.0 and later, Windows Embedded NavReady 2009</description></item>
        /// </list>
        /// </remarks>
        public static Guid[] MapUrl(Uri url)
        {
            System.Collections.Generic.List<Guid> guids = new System.Collections.Generic.List<Guid>();

            if (NativeMethods.hasCellcore)
            {
                Guid g;
                int index = 0;

                int hresult = 0;
                while (hresult == 0)
                {
                    hresult = NativeMethods.MapURL(url.ToString(), out g, ref index);
                    if (hresult == 0)
                    {
                        guids.Add(g);
                    }
                }
            }

            return guids.ToArray();
        }

        /// <summary>
        /// Maps a connection reference to its corresponding <see cref="Guid"/>.
        /// </summary>
        /// <param name="connectionReference">Connection reference to map.
        /// Here are some example connection references:
        /// A PPP connection, such as a CSD connection on a GSM or CDMA network 
        /// A GPRS connection 
        /// A RAS connection 
        /// A 1xRTT connection 
        /// A Desktop Passthrough (DTPT) connection 
        /// A proxy object 
        /// A modem connection </param>
        /// <returns>Reference <see cref="Guid"/> for the connection.</returns>
        /// <remarks>This function enables you to bypass Connection Planner by explicitly providing a <see cref="Guid"/> that will map to the connection.
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile 2003 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows Embedded Compact 6.0 and later, Windows Embedded NavReady 2009</description></item>
        /// </list>
        /// </remarks>
        public static Guid MapConnectionReference(string connectionReference)
        {
            Guid g = Guid.Empty;

            if (NativeMethods.hasCellcore)
            {
                int hresult = NativeMethods.MapConRef(ConnMgrConRefType.NAP, connectionReference, out g);
                Marshal.ThrowExceptionForHR(hresult);
            }

            return g;
        }

        private sealed class ConnectionManagerMessageWindow : MessageWindow
        {
            internal static int WM_ConnectionManager = NativeMethods.RegisterWindowMessage("ConnectionManagerEvent");
            internal const int WM_NETCONNECT = 0x03FE;

            private ConnectionManager parent;

            public ConnectionManagerMessageWindow(ConnectionManager parent)
            {
                this.parent = parent;
            }

            protected override void WndProc(ref Message m)
            {
                /*if (!NativeMethods.hasCellcore)
                {
                    if (m.Msg == WM_NETCONNECT)
                    {
                        bool success = m.WParam.ToInt32() == 1;
                        RNAAppInfo info = (RNAAppInfo)Marshal.PtrToStructure(m.LParam, typeof(RNAAppInfo));
                        if (info.context == this.GetHashCode())
                        {
                            this.parent.OnCEConnectionStatusChanged(success, info);
                        }
                    }
                }*/

                if (m.Msg == WM_ConnectionManager)
                {
                    // this is a connection manager message

                    // convert the status
                    ConnectionStatus status = (ConnectionStatus)m.WParam.ToInt32();

                    // raise the event on the parent ConnectionManager class
                    this.parent.OnConnectionStatusChanged(new ConnectionStatusChangedEventArgs(status));
                }

                base.WndProc(ref m);
            }
        }
    }
}