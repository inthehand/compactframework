// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.NetworkInformation.IPGlobalProperties
// 
// Copyright (c) 2003-2010 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Net;

namespace InTheHand.Net.NetworkInformation
{
    /// <summary>
    /// Provides information about the network connectivity of the local computer.
    /// </summary>
    /// <remarks>This class provides configuration and statistical information about the local computer's network interfaces and network connections.
    /// <para>The information provided by this class is similar to that provided by the Internet Protocol Helper API functions. 
    /// For information about the Internet Protocol Helper, see the documentation in the MSDN Library.</para>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// The following code example displays information about the local computer using an instance of this class.
    /// <code lang="vbnet">
    /// Public Shared Sub ShowInboundIPStatistics() 
    ///     Dim properties As IPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties()
    ///     Dim ipstat As IPGlobalStatistics = properties.GetIPv4GlobalStatistics()
    ///     Console.WriteLine("  Inbound Packet Data:")
    ///     Console.WriteLine("      Received ............................ : {0}", ipstat.ReceivedPackets)
    ///     Console.WriteLine("      Forwarded ........................... : {0}", ipstat.ReceivedPacketsForwarded)
    ///     Console.WriteLine("      Delivered ........................... : {0}", ipstat.ReceivedPacketsDelivered)
    ///     Console.WriteLine("      Discarded ........................... : {0}", ipstat.ReceivedPacketsDiscarded)
    /// End Sub 'ShowInboundIPStatistics
    /// </code>
    /// <code lang="cs">
    /// public static void ShowInboundIPStatistics()
    /// {
    ///     IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
    ///     IPGlobalStatistics ipstat = properties.GetIPv4GlobalStatistics();
    ///     Console.WriteLine("  Inbound Packet Data:");
    ///     Console.WriteLine("      Received ............................ : {0}", 
    ///     ipstat.ReceivedPackets);
    ///     Console.WriteLine("      Forwarded ........................... : {0}", 
    ///     ipstat.ReceivedPacketsForwarded);
    ///     Console.WriteLine("      Delivered ........................... : {0}", 
    ///     ipstat.ReceivedPacketsDelivered);
    ///     Console.WriteLine("      Discarded ........................... : {0}", 
    ///     ipstat.ReceivedPacketsDiscarded);   
    /// }
    /// </code>
    /// </example>
    public sealed class IPGlobalProperties
    {
        private static object syncObject = new object();
        private bool fixedInit = false;
        private string hostName;
        private string domainName;
        private bool isWinsProxy;
        private NetBiosNodeType nodeType;
        private string scopeName;

        internal IPGlobalProperties() {}

        /// <summary>
        /// Gets an object that provides information about the local computer's network connectivity and traffic statistics. 
        /// </summary>
        /// <returns>A <see cref="IPGlobalProperties"/> object that contains information about the local computer.</returns>
        public static IPGlobalProperties GetIPGlobalProperties()
        {
            if (!NativeMethods.hasIphlp)
            {
                throw new PlatformNotSupportedException();
            }

            return new IPGlobalProperties();
        }
        
        internal void GetFixedInfo()
        {
            if (!fixedInit)
            {
                lock (syncObject)
                {
                    byte[] fixedInfo = null;
                    int outBufLen = 0;
                    int result = NativeMethods.GetNetworkParams(null, ref outBufLen);
                    while (result == 0x6f)
                    {
                        fixedInfo = new byte[outBufLen];

                        result = NativeMethods.GetNetworkParams(fixedInfo, ref outBufLen);
                    }

                    if (result != 0)
                    {
                        throw new NetworkInformationException(result);
                    }

                    hostName = System.Text.Encoding.ASCII.GetString(fixedInfo, 0, 80);
                    int nullIndex = hostName.IndexOf("\0");
                    if (nullIndex > -1)
                    {
                        hostName = hostName.Substring(0, nullIndex);
                    }

                    domainName = System.Text.Encoding.ASCII.GetString(fixedInfo, 84, 80);
                    nullIndex = domainName.IndexOf("\0");
                    if (nullIndex > -1)
                    {
                        domainName = domainName.Substring(0, nullIndex);
                    }

                    scopeName = System.Text.Encoding.ASCII.GetString(fixedInfo, 312, 256);
                    nullIndex = scopeName.IndexOf("\0");
                    if (nullIndex > -1)
                    {
                        scopeName = scopeName.Substring(0, nullIndex);
                    }

                    nodeType = (NetBiosNodeType)BitConverter.ToInt32(fixedInfo, 308);
                    isWinsProxy = (BitConverter.ToInt32(fixedInfo, 576) != 0);

                    fixedInit = true;
                }
            }
        }

        /// <summary>
        /// Gets the host name for the local computer.
        /// </summary>
        /// <value>A <see cref="String"/> instance that contains the computer's NetBIOS name.</value>
        /// <exception cref="NetworkInformationException">A Win32 function call failed.</exception>
        /// <remarks>A computer's NetBIOS name must be unique within a network and is not fully qualified by the domain name.</remarks>
        public string HostName
        {
            get
            {
                GetFixedInfo();

                return hostName;
            }
        }

        /// <summary>
        /// Gets the domain in which the local computer is registered.
        /// </summary>
        /// <value>A <see cref="String"/> instance that contains the computer's domain name.
        /// If the computer does not belong to a domain, returns <see cref="String.Empty"/>.</value>
        /// <exception cref="NetworkInformationException">A Win32 function call failed.</exception>
        public string DomainName
        {
            get
            {
                GetFixedInfo();

                return domainName;
            }
        }

        /// <summary>
        /// Gets the Dynamic Host Configuration Protocol (DHCP) scope name.
        /// </summary>
        /// <value>A <see cref="String"/> instance that contains the computer's DHCP scope name.</value>
        /// <exception cref="NetworkInformationException">A Win32 function call failed.</exception>
        /// <remarks>A DHCP scope is an administrative grouping of networked computers that are on the same subnet.</remarks>
        public string DhcpScopeName
        {
            get
            {
                GetFixedInfo();

                return scopeName;
            }
        }

        /// <summary>
        /// Gets a <see cref="Boolean"/> value that specifies whether the local computer is acting as a Windows Internet Name Service (WINS) proxy.
        /// </summary>
        /// <value>true if the local computer is a WINS proxy; otherwise, false.</value>
        /// <exception cref="NetworkInformationException">A Win32 function call failed.</exception>
        /// <remarks>WINS provides a distributed database for registering and querying dynamic NetBIOS names to IP address mappings.</remarks>
        public bool IsWinsProxy
        {
            get
            {
                GetFixedInfo();

                return isWinsProxy;
            }
        }

        /// <summary>
        /// Gets the Network Basic Input/Output System (NetBIOS) node type of the local computer.
        /// </summary>
        /// <value>A <see cref="NetBiosNodeType"/> value.</value>
        /// <exception cref="NetworkInformationException">A Win32 function call failed.</exception>
        /// <remarks>The node type determines the way in which NetBIOS names are resolved to IP addresses. 
        /// For additional information, see the <see cref="NetBiosNodeType"/> class overview.</remarks>
        public NetBiosNodeType NodeType
        {
            get
            {
                GetFixedInfo();

                return nodeType;
            }
        }


        /// <summary>
        /// Provides Internet Control Message Protocol (ICMP) version 4 statistical data for the local computer.
        /// </summary>
        /// <returns>An <see cref="IcmpV4Statistics"/> object that provides ICMP version 4 traffic statistics for the local computer.</returns>
        public IcmpV4Statistics GetIcmpV4Statistics()
        {
            IcmpV4Statistics icmp4stat = new IcmpV4Statistics();
            int result = NativeMethods.GetIcmpStatisticsEx(ref icmp4stat.icmpstatistics, System.Net.Sockets.AddressFamily.InterNetwork);
            return icmp4stat;
        }

        /// <summary>
        /// Provides Internet Control Message Protocol (ICMP) version 6 statistical data for the local computer.
        /// </summary>
        /// <returns>An <see cref="IcmpV6Statistics"/> object that provides ICMP version 6 traffic statistics for the local computer.</returns>
        public IcmpV6Statistics GetIcmpV6Statistics()
        {
            IcmpV6Statistics icmp6stat = new IcmpV6Statistics();
            int result = NativeMethods.GetIcmpStatisticsEx(ref icmp6stat.icmpstatistics, System.Net.Sockets.AddressFamily.InterNetworkV6);
            return icmp6stat;
        }

        /// <summary>
        /// Provides Internet Protocol version 4 (IPv4) statistical data for the local computer. 
        /// </summary>
        /// <returns></returns>
        public IPGlobalStatistics GetIPv4GlobalStatistics()
        {
            IPGlobalStatistics ip4stat = new IPGlobalStatistics();
            int result = NativeMethods.GetIpStatisticsEx(ip4stat, System.Net.Sockets.AddressFamily.InterNetwork);
            return ip4stat;
        }

        /// <summary>
        /// Provides Internet Protocol version 6 (IPv6) statistical data for the local computer. 
        /// </summary>
        /// <returns></returns>
        public IPGlobalStatistics GetIPv6GlobalStatistics()
        {
            IPGlobalStatistics ip6stat = new IPGlobalStatistics();
            int result = NativeMethods.GetIpStatisticsEx(ip6stat, System.Net.Sockets.AddressFamily.InterNetworkV6);
            return ip6stat;
        }

        /// <summary>
        /// Provides Transmission Control Protocol/Internet Protocol version 4 (TCP/IPv4) statistical data for the local computer. 
        /// </summary>
        /// <returns>A <see cref="TcpStatistics"/> object that provides TCP/IPv4 traffic statistics for the local computer. </returns>
        public TcpStatistics GetTcpIPv4Statistics()
        {
            TcpStatistics tcp4stat = new TcpStatistics();
            int result = NativeMethods.GetTcpStatisticsEx(tcp4stat, System.Net.Sockets.AddressFamily.InterNetwork);
            return tcp4stat;
        }

        /// <summary>
        /// Provides Transmission Control Protocol/Internet Protocol version 6 (TCP/IPv6) statistical data for the local computer. 
        /// </summary>
        /// <returns>A <see cref="TcpStatistics"/> object that provides TCP/IPv6 traffic statistics for the local computer. </returns>
        public TcpStatistics GetTcpIPv6Statistics()
        {
            TcpStatistics tcp6stat = new TcpStatistics();
            int result = NativeMethods.GetTcpStatisticsEx(tcp6stat, System.Net.Sockets.AddressFamily.InterNetworkV6);
            return tcp6stat;
        }


        private TcpConnectionInformation[] GetAllTcpConnections()
        {
            int bufferLen = 0;
            IntPtr bufferPtr = IntPtr.Zero;
            TcpConnectionInformation[] informationArray = null;
            int result = NativeMethods.GetTcpTable(bufferPtr, ref bufferLen, true);
            while (result == 0x7a)
            {
                try
                {
                    bufferPtr = Marshal.AllocHGlobal(bufferLen);
                    result = NativeMethods.GetTcpTable(bufferPtr, ref bufferLen, true);
                    if (result == 0)
                    {
                        int numberOfEntries = Marshal.ReadInt32(bufferPtr);

                        if (numberOfEntries > 0)
                        {
                            informationArray = new TcpConnectionInformation[numberOfEntries];
                            for (int i = 0; i < numberOfEntries; i++)
                            {
                                MIB_TCPROW row = (MIB_TCPROW)Marshal.PtrToStructure(IntPtrInTheHand.Add(bufferPtr, 4 + (i * 20)), typeof(MIB_TCPROW));
                                informationArray[i] = new TcpConnectionInformation(row);
                            }
                        }
                    }
                    continue;
                }
                finally
                {
                    if(bufferPtr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(bufferPtr);
                        bufferPtr = IntPtr.Zero;
                    }
                }
            }

            if ((result != 0) && (result != 0xe8))
            {
                throw new NetworkInformationException((int)result);
            }

            if (informationArray == null)
            {
                return new TcpConnectionInformation[0];
            }

            return informationArray;
        }

        /// <summary>
        /// Returns information about the Internet Protocol version 4 (IPV4) Transmission Control Protocol (TCP) connections on the local computer.
        /// </summary>
        /// <returns>A <see cref="TcpConnectionInformation"/> array that contains objects that describe the active TCP connections, or an empty array if no active TCP connections are detected.</returns>
        /// <remarks>The objects returned by this method include listeners in all TCP states except the <see cref="TcpState.Listen"/> state. 
        /// The TCP protocol is defined in IETF RFC 793.
        /// Note that the objects returned by this method reflect the connections as of the time the array is created.
        /// This information is not updated dynamically.</remarks>
        public TcpConnectionInformation[] GetActiveTcpConnections()
        {
            System.Collections.Generic.List<TcpConnectionInformation> connections = new System.Collections.Generic.List<TcpConnectionInformation>();

            foreach (TcpConnectionInformation information in this.GetAllTcpConnections())
            {
                if (information.State != TcpState.Listen)
                {
                    connections.Add(information);
                }
            }

            return connections.ToArray();
        }

        /// <summary>
        /// Returns endpoint information about the Internet Protocol version 4 (IPV4) Transmission Control Protocol (TCP) listeners on the local computer.
        /// </summary>
        /// <returns>A <see cref="IPEndPoint"/> array that contains objects that describe the active TCP listeners, or an empty array, if no active TCP listeners are detected.</returns>
        /// <remarks>The objects returned by this method include listeners in the <see cref="TcpState.Listen"/> state. 
        /// The TCP protocol is defined in IETF RFC 793.
        /// Note that the objects returned by this method reflect the connections as of the time the array is created.
        /// This information is not updated dynamically.</remarks>
        public IPEndPoint[] GetActiveTcpListeners()
        {

            System.Collections.Generic.List<IPEndPoint> listeners = new System.Collections.Generic.List<IPEndPoint>();

            foreach (TcpConnectionInformation information in this.GetAllTcpConnections())
            {
                if (information.State == TcpState.Listen)
                {
                    listeners.Add(information.LocalEndPoint);
                }
            }

            return listeners.ToArray();
        }

        /// <summary>
        /// Returns information about the Internet Protocol version 4 (IPv4) User Datagram Protocol (UDP) listeners on the local computer.
        /// </summary>
        /// <returns>An IPEndPoint array that contains objects that describe the UDP listeners, or an empty array if no UDP listeners are detected.</returns>
        /// <remarks>UDP is a connectionless transport layer protocol that is responsible for sending and receiving datagrams.
        /// It is defined in IETF RFC 768.
        /// A UDP listener is an open socket that waits for and receives UDP datagrams.
        /// Because UDP is a connectionless protocol, the listener does not maintain a connection to a remote endpoint.</remarks>
        public IPEndPoint[] GetActiveUdpListeners()
        {
            int bufferLen = 0;
            IntPtr bufferPtr = IntPtr.Zero;
            IPEndPoint[] endPoints = null;
            int result = NativeMethods.GetUdpTable(bufferPtr, ref bufferLen, true);
            while (result == 0x7a)
            {
                try
                {
                    bufferPtr = Marshal.AllocHGlobal(bufferLen);
                    result = NativeMethods.GetUdpTable(bufferPtr, ref bufferLen, true);
                    if (result == 0)
                    {
                        int numberOfEntries = Marshal.ReadInt32(bufferPtr);

                        if (numberOfEntries > 0)
                        {
                            endPoints = new IPEndPoint[numberOfEntries];

                            for (int i = 0; i < numberOfEntries; i++)
                            {
                                NativeMethods.MIB_UDPROW ur = (NativeMethods.MIB_UDPROW)Marshal.PtrToStructure(IntPtrInTheHand.Add(bufferPtr, 4 + (i * 8)), typeof(NativeMethods.MIB_UDPROW));
                                endPoints[i] = new IPEndPoint(ur.dwLocalAddr, IPAddress.NetworkToHostOrder((short)ur.dwLocalPort));
                            }
                        }
                    }
                    continue;
                }
                finally
                {
                    if (bufferPtr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(bufferPtr);
                        bufferPtr = IntPtr.Zero;
                    }
                }
            }
            if ((result != 0) && (result != 0xe8))
            {
                throw new NetworkInformationException((int)result);
            }

            if (endPoints == null)
            {
                return new IPEndPoint[0];
            }

            return endPoints;
        }


        /// <summary>
        /// Provides User Datagram Protocol/Internet Protocol version 4 (UDP/IPv4) statistical data for the local computer. 
        /// </summary>
        /// <returns>A <see cref="UdpStatistics"/> object that provides UDP/IPv4 traffic statistics for the local computer.</returns>
        public UdpStatistics GetUdpIPv4Statistics()
        {
            UdpStatistics udp4stat = new UdpStatistics();
            int result = NativeMethods.GetUdpStatisticsEx(udp4stat, System.Net.Sockets.AddressFamily.InterNetwork);
            return udp4stat;
        }

        /// <summary>
        /// Provides User Datagram Protocol/Internet Protocol version 6 (UDP/IPv6) statistical data for the local computer. 
        /// </summary>
        /// <returns>A <see cref="UdpStatistics"/> object that provides UDP/IPv6 traffic statistics for the local computer.</returns>
        public UdpStatistics GetUdpIPv6Statistics()
        {
            UdpStatistics udp6stat = new UdpStatistics();
            int result = NativeMethods.GetUdpStatisticsEx(udp6stat, System.Net.Sockets.AddressFamily.InterNetworkV6);
            return udp6stat;
        }
    }
}