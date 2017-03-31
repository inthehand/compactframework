// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.NetworkInformation.IPGlobalStatistics
// 
// Copyright (c) 2003-2010 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Net;

namespace InTheHand.Net.NetworkInformation
{
    /// <summary>
    /// Provides Internet Protocol (IP) statistical data.
    /// </summary>
    /// <remarks>
    /// This class is used by the <see cref="IPGlobalProperties.GetIPv4GlobalStatistics"/> and <see cref="IPGlobalProperties.GetIPv6GlobalStatistics"/> methods to return IP traffic information.
    /// The Internet protocol is used to move IP packets from a source computer to a destination computer. 
    /// IP also handles dividing a packet that is too large into multiple packets that are small enough for transport, in a process known as fragmentation.
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
    /// </list>
    /// </remarks>
    /// <example>The following code code example displays IP statistics.
    /// <code lang="cs">
    /// public static void ShowIPStatistics(NetworkInterfaceComponent version)
    /// {
    ///     IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
    ///     IPGlobalStatistics ipstat = null;
    ///     switch (version)
    ///     {
    ///         case NetworkInterfaceComponent.IPv4:
    ///             ipstat = properties.GetIPv4GlobalStatistics();
    ///             Console.WriteLine("{0}IPv4 Statistics ",Environment.NewLine);
    ///             break;
    ///         case NetworkInterfaceComponent.IPv6:
    ///             ipstat = properties.GetIPv4GlobalStatistics();
    ///             Console.WriteLine("{0}IPv6 Statistics ",Environment.NewLine);
    ///             break;
    ///         default:
    ///             throw new ArgumentException("version");
    ///             break;
    ///     }
    ///     Console.WriteLine("  Forwarding enabled ...................... : {0}", 
    ///         ipstat.ForwardingEnabled);
    ///     Console.WriteLine("  Interfaces .............................. : {0}", 
    ///         ipstat.NumberOfInterfaces);
    ///     Console.WriteLine("  IP addresses ............................ : {0}", 
    ///         ipstat.NumberOfIPAddresses);
    ///     Console.WriteLine("  Routes .................................. : {0}", 
    ///         ipstat.NumberOfRoutes);
    ///     Console.WriteLine("  Default TTL ............................. : {0}", 
    ///         ipstat.DefaultTtl);
    ///     Console.WriteLine("");    
    ///     Console.WriteLine("  Inbound Packet Data:");
    ///     Console.WriteLine("      Received ............................ : {0}", 
    ///         ipstat.ReceivedPackets);
    ///     Console.WriteLine("      Forwarded ........................... : {0}", 
    ///         ipstat.ReceivedPacketsForwarded);
    ///     Console.WriteLine("      Delivered ........................... : {0}", 
    ///         ipstat.ReceivedPacketsDelivered);
    ///     Console.WriteLine("      Discarded ........................... : {0}", 
    ///         ipstat.ReceivedPacketsDiscarded);
    ///     Console.WriteLine("      Header Errors ....................... : {0}", 
    ///         ipstat.ReceivedPacketsWithHeadersErrors);
    ///     Console.WriteLine("      Address Errors ...................... : {0}", 
    ///         ipstat.ReceivedPacketsWithAddressErrors);
    ///     Console.WriteLine("      Unknown Protocol Errors ............. : {0}", 
    ///         ipstat.ReceivedPacketsWithUnknownProtocol);
    ///     Console.WriteLine("");    
    ///     Console.WriteLine("  Outbound Packet Data:");
    ///     Console.WriteLine("      Requested ........................... : {0}", 
    ///         ipstat.OutputPacketRequests);
    ///     Console.WriteLine("      Discarded ........................... : {0}", 
    ///         ipstat.OutputPacketsDiscarded);
    ///     Console.WriteLine("      No Routing Discards ................. : {0}", 
    ///         ipstat.OutputPacketsWithNoRoute);
    ///     Console.WriteLine("      Routing Entry Discards .............. : {0}", 
    ///         ipstat.OutputPacketRoutingDiscards);
    ///     Console.WriteLine("");    
    ///     Console.WriteLine("  Reassembly Data:");
    ///     Console.WriteLine("      Reassembly Timeout .................. : {0}", 
    ///         ipstat.PacketReassemblyTimeout);
    ///     Console.WriteLine("      Reassemblies Required ............... : {0}", 
    ///         ipstat.PacketReassembliesRequired);
    ///     Console.WriteLine("      Packets Reassembled ................. : {0}", 
    ///         ipstat.PacketsReassembled);
    ///     Console.WriteLine("      Packets Fragmented .................. : {0}", 
    ///         ipstat.PacketsFragmented);
    ///     Console.WriteLine("      Fragment Failures ................... : {0}", 
    ///         ipstat.PacketFragmentFailures);
    ///     Console.WriteLine("");
    /// }</code>
    /// </example>
    public sealed class IPGlobalStatistics
    {
#pragma warning disable 0169, 0649
        private int dwForwarding;
        private int dwDefaultTTL;
        private int dwInReceives;
        private int dwInHdrErrors;
        private int dwInAddrErrors;
        private int dwForwDatagrams;
        private int dwInUnknownProtos;
        private int dwInDiscards;
        private int dwInDelivers;
        private int dwOutRequests;
        private int dwRoutingDiscards;
        private int dwOutDiscards;
        private int dwOutNoRoutes;
        private int dwReasmTimeout;
        private int dwReasmReqds;
        private int dwReasmOks;
        private int dwReasmFails;
        private int dwFragOks;
        private int dwFragFails;
        private int dwFragCreates;
        private int dwNumIf;
        private int dwNumAddr;
        private int dwNumRoutes;

#pragma warning restore 0169, 0649

        internal IPGlobalStatistics(){}

        /// <summary>
        /// Gets the default time-to-live (TTL) value for Internet Protocol (IP) packets.
        /// </summary>
        public int DefaultTtl
        {
            get
            {
                return dwDefaultTTL;
            }
        }

        /// <summary>
        /// Gets a <see cref="Boolean"/> value that specifies whether Internet Protocol (IP) packet forwarding is enabled.
        /// </summary>
        public bool ForwardingEnabled
        {
            get
            {
                return dwForwarding != 0;
            }
        }

        /// <summary>
        /// Gets the number of network interfaces.
        /// </summary>
        public int NumberOfInterfaces
        {
            get
            {
                return dwNumIf;
            }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) addresses assigned to the local computer.
        /// </summary>
        public int NumberOfIPAddresses
        {
            get
            {
                return dwNumAddr;
            }
        }

        /// <summary>
        /// Gets the number of routes in the Internet Protocol (IP) routing table.
        /// </summary>
        public int NumberOfRoutes
        {
            get
            {
                return dwNumRoutes;
            }
        }

        /// <summary>
        /// Gets the number of outbound Internet Protocol (IP) packets.
        /// </summary>
        public long OutputPacketRequests
        {
            get
            {
                return dwOutRequests;
            }
        }

        /// <summary>
        /// Gets the number of routes that have been discarded from the routing table.
        /// </summary>
        public long OutputPacketRoutingDiscards
        {
            get
            {
                return dwRoutingDiscards;
            }
        }

        /// <summary>
        /// Gets the number of transmitted Internet Protocol (IP) packets that have been discarded.
        /// </summary>
        public long OutputPacketsDiscarded
        {
            get
            {
                return dwOutDiscards;
            }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets for which the local computer could not determine a route to the destination address.
        /// </summary>
        public long OutputPacketsWithNoRoute
        {
            get
            {
                return dwOutNoRoutes;
            }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets that could not be fragmented.
        /// </summary>
        public long PacketFragmentFailures
        {
            get
            {
                return dwFragFails;
            }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets that required reassembly.
        /// </summary>
        public long PacketReassembliesRequired
        {
            get
            {
                return dwReasmReqds;
            }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets that were not successfully reassembled.
        /// </summary>
        public long PacketReassemblyFailures
        {
            get
            {
                return dwReasmFails;
            }
        }

        /// <summary>
        /// Gets the maximum amount of time within which all fragments of an Internet Protocol (IP) packet must arrive.
        /// </summary>
        public long PacketReassemblyTimeout
        {
            get
            {
                return dwReasmTimeout;
            }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets fragmented.
        /// </summary>
        public long PacketsFragmented
        {
            get
            {
                return dwFragOks;
            }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets reassembled.
        /// </summary>
        public long PacketsReassembled
        {
            get
            {
                return dwReasmOks;
            }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets received.
        /// </summary>
        public long ReceivedPackets
        {
            get
            {
                return dwInReceives;
            }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets delivered.
        /// </summary>
        public long ReceivedPacketsDelivered
        {
            get
            {
                return dwInDelivers;
            }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets that have been received and discarded.
        /// </summary>
        public long ReceivedPacketsDiscarded
        {
            get
            {
                return dwInDiscards;
            }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets forwarded.
        /// </summary>
        public long ReceivedPacketsForwarded 
        { 
            get
            {
                return dwForwDatagrams;
            }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets with address errors that were received.
        /// </summary>
        public long ReceivedPacketsWithAddressErrors
        {
            get
            {
                return dwInAddrErrors;
            }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets with header errors that were received.
        /// </summary>
        public long ReceivedPacketsWithHeadersErrors
        {
            get
            {
                return dwInHdrErrors;
            }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets received on the local machine with an unknown protocol in the header.
        /// </summary>
        public long ReceivedPacketsWithUnknownProtocol
        {
            get
            {
                return dwInUnknownProtos;
            }
        }
    }
}
