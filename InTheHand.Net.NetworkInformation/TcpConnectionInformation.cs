// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.NetworkInformation.TcpConnectionInformation
// 
// Copyright (c) 2007-2010 In The Hand Ltd, All rights reserved.

using System.Net;
using System.Runtime.InteropServices;

namespace InTheHand.Net.NetworkInformation
{
    /// <summary>
    /// Provides information about the Transmission Control Protocol (TCP) connections on the local computer.
    /// </summary>
    public sealed class TcpConnectionInformation
    {
        private IPEndPoint localEndPoint;
        private IPEndPoint remoteEndPoint;
        private TcpState state;

        internal TcpConnectionInformation(MIB_TCPROW row)
        {
            this.state = (TcpState)row.dwState;
            int localPort = (int)(((row.dwLocalPort & 0xff000000) >> 8) | ((row.dwLocalPort & 0xff0000) << 8) | ((row.dwLocalPort & 0xFF00) >> 8) | ((row.dwLocalPort & 0xff) << 8));//((ushort)Marshal.ReadInt16(bufferPtr, 8 + (i * 8)) >> 8) | (ushort)Marshal.ReadInt16(bufferPtr, 10 + (i * 8));  row.dwLocalPort;// (((row.localPort3 << 0x18) | (row.localPort4 << 0x10)) | (row.localPort1 << 8)) | row.localPort2;
            int remotePort = (int)((this.state == TcpState.Listen) ? 0 : (int)(((row.dwRemotePort & 0xff000000) >> 8) | ((row.dwRemotePort & 0xff0000) << 8) | ((row.dwRemotePort & 0xFF00) >> 8) | ((row.dwRemotePort & 0xff) << 8)));//((row.dwRemotePort & 0xFFFF0000) >> 16) | ((row.dwRemotePort & 0xffff) << 16));// ((((row.remotePort3 << 0x18) | (row.remotePort4 << 0x10)) | (row.remotePort1 << 8)) | row.remotePort2);
            this.localEndPoint = new IPEndPoint((long) row.dwLocalAddr, localPort);
            this.remoteEndPoint = new IPEndPoint((long) row.dwRemoteAddr, remotePort);
        }

        /// <summary>
        /// Gets the local endpoint of a Transmission Control Protocol (TCP) connection.
        /// </summary>
        public IPEndPoint LocalEndPoint
        {
            get
            {
                return this.localEndPoint;
            }
        }

        /// <summary>
        /// Gets the remote endpoint of a Transmission Control Protocol (TCP) connection.
        /// </summary>
        public IPEndPoint RemoteEndPoint
        {
            get
            {
                return this.remoteEndPoint;
            }
        }

        /// <summary>
        /// Gets the state of this Transmission Control Protocol (TCP) connection.
        /// </summary>
        public TcpState State
        {
            get
            {
                return this.state;
            }
        }
    }

    

    [StructLayout(LayoutKind.Sequential, Size=20)]
    internal struct MIB_TCPROW
    {
        internal uint dwState;
        internal uint dwLocalAddr;
        internal uint dwLocalPort;
        internal uint dwRemoteAddr;
        internal uint dwRemotePort;
    }
 

}
