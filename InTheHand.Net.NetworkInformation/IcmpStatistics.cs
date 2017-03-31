// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.NetworkInformation.IcmpStatistics
// 
// Copyright (c) 2003-2009 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Net;

namespace InTheHand.Net.NetworkInformation
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MIBICMPSTATS
    {
        internal int dwMsgs;
        internal int dwErrors;
        internal int dwDestUnreachs;
        internal int dwTimeExcds;
        internal int dwParmProbs;
        internal int dwSrcQuenchs;
        internal int dwRedirects;
        internal int dwEchos;
        internal int dwEchoReps;
        internal int dwTimestamps;
        internal int dwTimestampReps;
        internal int dwAddrMasks;
        internal int dwAddrMaskReps;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MIBICMPINFO
    {
        internal MIBICMPSTATS icmpInStats;
        internal MIBICMPSTATS icmpOutStats;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MIB_ICMP
    {
        internal MIBICMPINFO stats;
    }

    /// <summary>
    /// Provides Internet Control Message Protocol for IPv4 (ICMPv4) statistical data for the local computer.
    /// </summary>
    /// <remarks>
    /// ICMPv4 is a set of error and informational messages for use with Internet Protocol version 4. 
    /// ICMP version 4 is defined in IETF RFC 792.
    /// This class is used by the <see cref="IPGlobalProperties.GetIcmpV4Statistics"/> method to return ICMPv4 traffic information.
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// The following example displays the current ICMPv4 statistics.
    /// <code lang="cs">
    /// public static void ShowIcmpV4Statistics()
    /// {
    ///     IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
    ///     IcmpV4Statistics stat = properties.GetIcmpV4Statistics();
    ///     Console.WriteLine("ICMP V4 Statistics:");
    /// 
    ///     Console.WriteLine("  Messages ............................ Sent: {0,-10}   Received: {1,-10}",
    ///         stat.MessagesSent, stat.MessagesReceived);
    ///     Console.WriteLine("  Errors .............................. Sent: {0,-10}   Received: {1,-10}",
    ///         stat.ErrorsSent, stat.ErrorsReceived);
    /// 
    ///     Console.WriteLine("  Echo Requests ....................... Sent: {0,-10}   Received: {1,-10}",
    ///         stat.EchoRequestsSent, stat.EchoRequestsReceived);
    ///     Console.WriteLine("  Echo Replies ........................ Sent: {0,-10}   Received: {1,-10}",
    ///         stat.EchoRepliesSent, stat.EchoRepliesReceived);
    /// 
    ///     Console.WriteLine("  Destination Unreachables ............ Sent: {0,-10}   Received: {1,-10}",
    ///         stat.DestinationUnreachableMessagesSent, stat.DestinationUnreachableMessagesReceived);
    /// 
    ///     Console.WriteLine("  Source Quenches ..................... Sent: {0,-10}   Received: {1,-10}",
    ///         stat.SourceQuenchesSent, stat.SourceQuenchesReceived);
    /// 
    ///     Console.WriteLine("  Redirects ........................... Sent: {0,-10}   Received: {1,-10}",
    ///         stat.RedirectsSent, stat.RedirectsReceived);  
    /// 
    ///     Console.WriteLine("  TimeExceeded ........................ Sent: {0,-10}   Received: {1,-10}",
    ///         stat.TimeExceededMessagesSent, stat.TimeExceededMessagesReceived);
    /// 
    ///     Console.WriteLine("  Parameter Problems .................. Sent: {0,-10}   Received: {1,-10}",
    ///         stat.ParameterProblemsSent, stat.ParameterProblemsReceived);   
    /// 
    ///     Console.WriteLine("  Timestamp Requests .................. Sent: {0,-10}   Received: {1,-10}",
    ///         stat.TimestampRequestsSent, stat.TimestampRequestsReceived);    
    ///     Console.WriteLine("  Timestamp Replies ................... Sent: {0,-10}   Received: {1,-10}",
    ///         stat.TimestampRepliesSent, stat.TimestampRepliesReceived);    
    ///     
    ///     Console.WriteLine("  Address Mask Requests ............... Sent: {0,-10}   Received: {1,-10}",
    ///         stat.AddressMaskRequestsSent, stat.AddressMaskRequestsReceived);    
    ///     Console.WriteLine("  Address Mask Replies ................ Sent: {0,-10}   Received: {1,-10}",
    ///         stat.AddressMaskRepliesSent, stat.AddressMaskRepliesReceived);                    
    ///     Console.WriteLine("");    
    /// }</code></example>
    public sealed class IcmpV4Statistics
    {
        internal IcmpV4Statistics(){}

        internal MIBICMPINFO icmpstatistics;

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Address Mask Reply messages that were received.
        /// </summary>
        public long AddressMaskRepliesReceived
        {
            get
            {
                return icmpstatistics.icmpInStats.dwAddrMaskReps;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Address Mask Reply messages that were sent.
        /// </summary>
        public long AddressMaskRepliesSent
        {
            get
            {
                return icmpstatistics.icmpOutStats.dwAddrMaskReps;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Address Mask Request messages that were received.
        /// </summary>
        public long AddressMaskRequestsReceived
        {
            get
            {
                return icmpstatistics.icmpInStats.dwAddrMasks;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Address Mask Request messages that were sent.
        /// </summary>
        public long AddressMaskRequestsSent 
        { 
            get
            {
                return icmpstatistics.icmpOutStats.dwAddrMasks;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) messages that were received because of a packet having an unreachable address in its destination.
        /// </summary>
        public long DestinationUnreachableMessagesReceived
        {
            get
            {
                return icmpstatistics.icmpInStats.dwDestUnreachs;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) messages that were sent because of a packet having an unreachable address in its destination.
        /// </summary>
        public long DestinationUnreachableMessagesSent
        {
            get
            {
                return icmpstatistics.icmpOutStats.dwDestUnreachs;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Echo Reply messages that were received.
        /// </summary>
        public long EchoRepliesReceived
        {
            get
            {
                return icmpstatistics.icmpInStats.dwEchoReps;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Echo Reply messages that were sent.
        /// </summary>
        public long EchoRepliesSent
        {
            get
            {
                return icmpstatistics.icmpOutStats.dwEchoReps;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Echo Request messages that were received.
        /// </summary>
        public long EchoRequestsReceived { 
            get
            {
                return icmpstatistics.icmpInStats.dwEchos;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Echo Request messages that were sent.
        /// </summary>
        public long EchoRequestsSent
        {
            get
            {
                return icmpstatistics.icmpOutStats.dwEchos;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) error messages that were received.
        /// </summary>
        public long ErrorsReceived
        {
            get
            {
                return icmpstatistics.icmpInStats.dwErrors;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) error messages that were sent.
        /// </summary>
        public long ErrorsSent
        {
            get
            {
                return icmpstatistics.icmpOutStats.dwErrors;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol messages that were received.
        /// </summary>
        public long MessagesReceived
        {
            get
            {
                return icmpstatistics.icmpInStats.dwMsgs;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) messages that were sent.
        /// </summary>
        public long MessagesSent
        {
            get
            {
                return icmpstatistics.icmpOutStats.dwMsgs;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Parameter Problem messages that were received.
        /// </summary>
        public long ParameterProblemsReceived
        {
            get
            {
                return icmpstatistics.icmpInStats.dwParmProbs;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Parameter Problem messages that were sent.
        /// </summary>
        public long ParameterProblemsSent
        {
            get
            {
                return icmpstatistics.icmpOutStats.dwParmProbs;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Redirect messages that were received.
        /// </summary>
        public long RedirectsReceived
        {
            get
            {
                return icmpstatistics.icmpInStats.dwRedirects;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Redirect messages that were sent.
        /// </summary>
        public long RedirectsSent
        {
            get
            {
                return icmpstatistics.icmpOutStats.dwRedirects;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Source Quench messages that were received.
        /// </summary>
        public long SourceQuenchesReceived
        {
            get
            {
                return icmpstatistics.icmpInStats.dwSrcQuenchs;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Source Quench messages that were sent.
        /// </summary>
        public long SourceQuenchesSent
        {
            get
            {
                return icmpstatistics.icmpOutStats.dwSrcQuenchs;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Time Exceeded messages that were received.
        /// </summary>
        public long TimeExceededMessagesReceived
        {
            get
            {
                return icmpstatistics.icmpInStats.dwTimeExcds;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Time Exceeded messages that were sent.
        /// </summary>
        public long TimeExceededMessagesSent 
        {
            get
            {
                return icmpstatistics.icmpOutStats.dwTimeExcds;
            } 
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Timestamp Reply messages that were received.
        /// </summary>
        public long TimestampRepliesReceived
        {
            get
            {
                return icmpstatistics.icmpInStats.dwTimestampReps;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Timestamp Reply messages that were sent.
        /// </summary>
        public long TimestampRepliesSent
        {
            get
            {
                return icmpstatistics.icmpOutStats.dwTimestampReps;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Timestamp Request messages that were received.
        /// </summary>
        public long TimestampRequestsReceived
        {
            get
            {
                return icmpstatistics.icmpInStats.dwTimestamps;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Timestamp Request messages that were sent.
        /// </summary>
        public long TimestampRequestsSent
        {
            get
            {
                return icmpstatistics.icmpOutStats.dwTimestamps;
            }
        }
    }

    /// <summary>
    /// Provides Internet Control Message Protocol for Internet Protocol version 6 (ICMPv6) statistical data for the local computer.
    /// </summary>
    /// <remarks><para>Equivalent to <b>System.Net.NetworkInformation.IcmpV6Statistics</b></para>
    /// ICMPV6 is a set of error and informational messages for use with Internet Protocol version 6 (IPv6). 
    /// This class is used by the <see cref="IPGlobalProperties.GetIcmpV6Statistics"/> method to return ICMPV6 traffic information.
    /// <para>ICMPv6 is defined in RFC 2463.</para>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
    /// </list>
    /// </remarks>
    /// <example>The following example displays the current ICMPv6 statistics.
    /// <code lang="cs">
    /// public static void ShowIcmpV6Statistics()
    /// {
    ///     IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
    ///     IcmpV6Statistics stat = properties.GetIcmpV6Statistics();
    ///     Console.WriteLine("ICMP V6 Statistics:");
    ///     Console.WriteLine("  Messages ............................ Sent: {0,-10}   Received: {1,-10}",
    ///         stat.MessagesSent, stat.MessagesReceived);
    ///     Console.WriteLine("  Errors .............................. Sent: {0,-10}   Received: {1,-10}",
    ///         stat.ErrorsSent, stat.ErrorsReceived);
    /// 
    ///     Console.WriteLine("  Echo Requests ....................... Sent: {0,-10}   Received: {1,-10}",
    ///         stat.EchoRequestsSent, stat.EchoRequestsReceived);
    ///     Console.WriteLine("  Echo Replies ........................ Sent: {0,-10}   Received: {1,-10}",
    ///         stat.EchoRepliesSent, stat.EchoRepliesReceived);
    /// 
    ///     Console.WriteLine("  Destination Unreachables ............ Sent: {0,-10}   Received: {1,-10}",
    ///         stat.DestinationUnreachableMessagesSent, stat.DestinationUnreachableMessagesReceived);
    /// 
    ///     Console.WriteLine("  Parameter Problems .................. Sent: {0,-10}   Received: {1,-10}",
    ///         stat.ParameterProblemsSent, stat.ParameterProblemsReceived);       
    /// 
    ///     Console.WriteLine("  Packets Too Big ..................... Sent: {0,-10}   Received: {1,-10}",
    ///         stat.PacketTooBigMessagesSent, stat.PacketTooBigMessagesReceived);
    /// 
    ///     Console.WriteLine("  Redirects ........................... Sent: {0,-10}   Received: {1,-10}",
    ///         stat.RedirectsSent, stat.RedirectsReceived);
    /// 
    ///     Console.WriteLine("  Router Advertisements ............... Sent: {0,-10}   Received: {1,-10}",
    ///         stat.RouterAdvertisementsSent, stat.RouterAdvertisementsReceived);                    
    ///     Console.WriteLine("  Router Solicitations ................ Sent: {0,-10}   Received: {1,-10}",
    ///         stat.RouterSolicitsSent, stat.RouterSolicitsReceived);   
    /// 
    ///     Console.WriteLine("  Time Exceeded ....................... Sent: {0,-10}   Received: {1,-10}",
    ///         stat.TimeExceededMessagesSent, stat.TimeExceededMessagesReceived);
    /// 
    ///     Console.WriteLine("  Neighbor Advertisements ............. Sent: {0,-10}   Received: {1,-10}",
    ///         stat.NeighborAdvertisementsSent, stat.NeighborAdvertisementsReceived);        
    ///     Console.WriteLine("  Neighbor Solicitations .............. Sent: {0,-10}   Received: {1,-10}",
    ///         stat.NeighborSolicitsSent, stat.NeighborSolicitsReceived);    
    /// 
    ///     Console.WriteLine("  Membership Queries .................. Sent: {0,-10}   Received: {1,-10}",
    ///         stat.MembershipQueriesSent, stat.MembershipQueriesReceived);    
    ///     Console.WriteLine("  Membership Reports .................. Sent: {0,-10}   Received: {1,-10}",
    ///         stat.MembershipReportsSent, stat.MembershipReportsReceived);    
    ///     Console.WriteLine("  Membership Reductions ............... Sent: {0,-10}   Received: {1,-10}",
    ///         stat.MembershipReductionsSent, stat.MembershipReductionsReceived);    
    /// 
    ///     Console.WriteLine("");
    /// }</code></example>
    public sealed class IcmpV6Statistics
    {
        internal IcmpV6Statistics() { }

        internal MIBICMPINFO icmpstatistics;

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 6 (ICMPv6) messages that were received because of a packet having an unreachable address in its destination.
        /// </summary>
        public long DestinationUnreachableMessagesReceived
        {
            get
            {
                return icmpstatistics.icmpInStats.dwDestUnreachs;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 6 (ICMPv6) messages that were sent because of a packet having an unreachable address in its destination.
        /// </summary>
        public long DestinationUnreachableMessagesSent
        {
            get
            {
                return icmpstatistics.icmpOutStats.dwDestUnreachs;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Echo Reply messages that were received.
        /// </summary>
        public long EchoRepliesReceived
        {
            get
            {
                return icmpstatistics.icmpInStats.dwEchoReps;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Echo Reply messages that were sent.
        /// </summary>
        public long EchoRepliesSent
        {
            get
            {
                return icmpstatistics.icmpOutStats.dwEchoReps;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Echo Request messages that were received.
        /// </summary>
        public long EchoRequestsReceived
        {
            get
            {
                return icmpstatistics.icmpInStats.dwEchos;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Echo Request messages that were sent.
        /// </summary>
        public long EchoRequestsSent
        {
            get
            {
                return icmpstatistics.icmpOutStats.dwEchos;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 6 (ICMPv6) error messages that were received.
        /// </summary>
        public long ErrorsReceived
        {
            get
            {
                return icmpstatistics.icmpInStats.dwErrors;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 6 (ICMPv6) error messages that were sent.
        /// </summary>
        public long ErrorsSent
        {
            get
            {
                return icmpstatistics.icmpOutStats.dwErrors;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 6 (ICMPv6) messages that were received.
        /// </summary>
        public long MessagesReceived
        {
            get
            {
                return icmpstatistics.icmpInStats.dwMsgs;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 6 (ICMPv6) messages that were sent.
        /// </summary>
        public long MessagesSent
        {
            get
            {
                return icmpstatistics.icmpOutStats.dwMsgs;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Parameter Problem messages that were received.
        /// </summary>
        public long ParameterProblemsReceived
        {
            get
            {
                return icmpstatistics.icmpInStats.dwParmProbs;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Parameter Problem messages that were sent.
        /// </summary>
        public long ParameterProblemsSent
        {
            get
            {
                return icmpstatistics.icmpOutStats.dwParmProbs;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Redirect messages that were received.
        /// </summary>
        public long RedirectsReceived
        {
            get
            {
                return icmpstatistics.icmpInStats.dwRedirects;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Redirect messages that were sent.
        /// </summary>
        public long RedirectsSent
        {
            get
            {
                return icmpstatistics.icmpOutStats.dwRedirects;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Time Exceeded messages that were received.
        /// </summary>
        public long TimeExceededMessagesReceived
        {
            get
            {
                return icmpstatistics.icmpInStats.dwTimeExcds;
            }
        }
        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Time Exceeded messages that were sent.
        /// </summary>
        public long TimeExceededMessagesSent
        {
            get
            {
                return icmpstatistics.icmpOutStats.dwTimeExcds;
            }
        }
    }
}
