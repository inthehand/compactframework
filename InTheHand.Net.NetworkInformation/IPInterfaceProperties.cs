// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.NetworkInformation.IPInterfaceProperties
// 
// Copyright (c) 2003-2010 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Net;

namespace InTheHand.Net.NetworkInformation
{
    /// <summary>
    /// Provides information about network interfaces that support Internet Protocol version 4 (IPv4) or Internet Protocol version 6 (IPv6).
    /// </summary>
    /// <remarks>This class provides access to configuration and address information for network interfaces that support IPv4 or IPv6.
    /// You do not create instances of this class; they are returned by the <see cref="NetworkInterface.GetIPProperties"/> method.
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
    /// </list>
    /// </remarks>
    public sealed class IPInterfaceProperties
    {
        //private IP_ADAPTER_ADDRESSES ipaa;

        internal IPInterfaceProperties(IP_ADAPTER_ADDRESSES addresses)
        {
            //this.ipaa = addresses;
            dnsSuffix = addresses.DnsSuffix;
            isDynamicDnsEnabled = addresses.Flags.HasFlag(IP_ADAPTER.IP_ADAPTER_DDNS_ENABLED);
        }

        private string dnsSuffix;
        /// <summary>
        /// Gets the Domain Name System (DNS) suffix associated with this interface.
        /// </summary>
        /// <value>A <see cref="String"/> that contains the DNS suffix for this interface, or <see cref="String.Empty"/> if there is no DNS suffix for the interface.</value>
        public string DnsSuffix
        {
            get
            {
                return dnsSuffix;
            }

        }

        private bool isDynamicDnsEnabled;
        /// <summary>
        /// Gets a Boolean value that indicates whether this interface is configured to automatically register its IP address information with the Domain Name System (DNS).
        /// </summary>
        public bool IsDynamicDnsEnabled
        {
            get
            {
                return isDynamicDnsEnabled ;
            }
        }

        private IPAddressCollection dnsAddresses = new IPAddressCollection();
        /// <summary>
        /// Gets the addresses of Domain Name System (DNS) servers for this interface.
        /// </summary>
        public IPAddressCollection DnsAddresses
        {
            get
            {
                return dnsAddresses;
            }
        }

        private IPAddressInformationCollection anycastAddresses = new IPAddressInformationCollection();
        /// <summary>
        /// Gets the anycast IP addresses assigned to this interface.
        /// </summary>
        public IPAddressInformationCollection AnycastAddresses
        {
            get
            {
                return anycastAddresses;
            }
        }

        private MulticastIPAddressInformationCollection multicastAddresses = new MulticastIPAddressInformationCollection();
        /// <summary>
        /// Gets the multicast addresses assigned to this interface.
        /// </summary>
        public MulticastIPAddressInformationCollection MulticastAddresses
        {
            get
            {
                return multicastAddresses;
            }
        }

        private UnicastIPAddressInformationCollection unicastAddresses = new UnicastIPAddressInformationCollection();
        /// <summary>
        /// Gets the unicast addresses assigned to this interface.
        /// </summary>
        public UnicastIPAddressInformationCollection UnicastAddresses
        {
            get
            {
                return unicastAddresses;
            }
        }

    }

     [Flags()]
    internal enum GAA_FLAG : int
    {
        SKIP_UNICAST       = 0x0001,
        SKIP_ANYCAST       = 0x0002,
        SKIP_MULTICAST     = 0x0004,
        SKIP_DNS_SERVER    = 0x0008,
        INCLUDE_PREFIX     = 0x0010,
        SKIP_FRIENDLY_NAME = 0x0020,
    }

    [Flags()]
    internal enum IP_ADAPTER : int
    {
        IP_ADAPTER_DDNS_ENABLED = 0x01,
        IP_ADAPTER_REGISTER_ADAPTER_SUFFIX = 0x02,
        IP_ADAPTER_DHCP_ENABLED = 0x04,
        IP_ADAPTER_RECEIVE_ONLY = 0x08,
        IP_ADAPTER_NO_MULTICAST = 0x10,
        IP_ADAPTER_IPV6_OTHER_STATEFUL_CONFIG = 0x20,
    }

    [StructLayout(LayoutKind.Sequential, Size=152)]
    internal struct IP_ADAPTER_ADDRESSES
    {
        internal uint Length;
        internal uint IfIndex;
        internal IntPtr Next;
        //[MarshalAs(UnmanagedType.LPWStr)]
        internal IntPtr AdapterName;
        internal IntPtr FirstUnicastAddress;
        internal IntPtr FirstAnycastAddress;
        internal IntPtr FirstMulticastAddress;
        internal IntPtr FirstDnsServerAddress;
        [MarshalAs(UnmanagedType.LPWStr)]
        internal string DnsSuffix;
        [MarshalAs(UnmanagedType.LPWStr)]
        internal string Description;
        [MarshalAs(UnmanagedType.LPWStr)]
        internal string FriendlyName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=8)]
        internal byte[] PhysicalAddress;
        internal int PhysicalAddressLength;
        internal IP_ADAPTER Flags;
        internal uint Mtu;
        internal NetworkInterfaceType IfType;
        internal OperationalStatus OperStatus;
        internal int Ipv6IfIndex;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=16)]
        internal uint[] ZoneIndices;
        //[MarshalAs(UnmanagedType.LPStruct)]
        internal IntPtr FirstPrefix;
    }






    [Flags()]
    internal enum IP_ADAPTER_ADDRESS
    {
        DNS_ELIGIBLE =0x01,
        TRANSIENT    =0x02,
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct IP_ADAPTER_DNS_SERVER_ADDRESS 
    {
        internal int Length;
        private int Reserved;
        internal IntPtr Next;
        internal SOCKET_ADDRESS Address;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct IP_ADAPTER_PREFIX
    {
        internal int Length;
        internal int Flags;
        internal IntPtr Next;
        internal SOCKET_ADDRESS Address;
        internal uint PrefixLength;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SOCKET_ADDRESS
    {
        internal IntPtr lpSockaddr;
        internal int iSockaddrLength;
    }

    /// <summary>
    /// Specifies how an IP address network prefix was located.
    /// </summary>
    /// <remarks>IP addresses are divided into two parts: the prefix and the suffix.
    /// The address prefix identifies the network portion of an IP address, and the address suffix identifies the host portion.
    /// Prefixes are assigned by global authorities, and suffixes are assigned by local system administrators.
    /// <para>This enumeration is used by the <see cref="UnicastIPAddressInformation"/> and <see cref="MulticastIPAddressInformation"/> classes.
    /// Instances of this class are returned when you retrieve the address information for a <see cref="NetworkInterface"/> object.</para></remarks>
    public enum PrefixOrigin
    {
        /// <summary>
        /// The prefix was located using an unspecified source.
        /// </summary>
        Other,
        /// <summary>
        /// The prefix was manually configured.
        /// </summary>
        Manual,
        /// <summary>
        /// The prefix is a well-known prefix.
        /// Well-known prefixes are specified in standard-track Request for Comments (RFC) documents and assigned by the Internet Assigned Numbers Authority (Iana) or an address registry.
        /// Such prefixes are reserved for special purposes.
        /// </summary>
        WellKnown,
        /// <summary>
        /// The prefix was supplied by a Dynamic Host Configuration Protocol (DHCP) server.
        /// </summary>
        Dhcp,
        /// <summary>
        /// The prefix was supplied by a router advertisement.
        /// </summary>
        RouterAdvertisement
    }

    /// <summary>
    /// Specifies how an IP address host suffix was located.
    /// </summary>
    /// <remarks>IP addresses are divided into two parts: the prefix and the suffix.
    /// The address prefix identifies the network portion of an IP address, and the address suffix identifies the host portion.
    /// Prefixes are assigned by global authorities, and suffixes are assigned by local system administrators.
    /// <para>This enumeration is used by the <see cref="UnicastIPAddressInformation"/> and <see cref="MulticastIPAddressInformation"/> classes.
    /// Instances of this class are returned when you retrieve the address information for a <see cref="NetworkInterface"/> object.</para></remarks>
    public enum SuffixOrigin
    {
        /// <summary>
        /// The suffix was located using an unspecified source.
        /// </summary>
        Other,
        /// <summary>
        /// The suffix was manually configured.
        /// </summary>
        Manual,
        /// <summary>
        /// The suffix is a well-known suffix.
        /// Well-known suffixes are specified in standard-track Request for Comments (RFC) documents and assigned by the Internet Assigned Numbers Authority (Iana) or an address registry.
        /// Such suffixes are reserved for special purposes.
        /// </summary>
        WellKnown,
        /// <summary>
        /// The suffix was supplied by a Dynamic Host Configuration Protocol (DHCP) server.
        /// </summary>
        OriginDhcp,
        /// <summary>
        /// The suffix is a link-local suffix.
        /// </summary>
        LinkLayerAddress,
        /// <summary>
        /// The suffix was randomly assigned.
        /// </summary>
        Random
    }

    /// <summary>
    /// Specifies the current state of an IP address.
    /// </summary>
    public enum DuplicateAddressDetectionState
    {
        /// <summary>
        /// The address is not valid.
        /// A nonvalid address is expired and no longer assigned to an interface; applications should not send data packets to it.
        /// </summary>
        Invalid = 0,
        /// <summary>
        /// The duplicate address detection procedure's evaluation of the address has not completed successfully.
        /// Applications should not use the address because it is not yet valid and packets sent to it are discarded.
        /// </summary>
        Tentative,
        /// <summary>
        /// The address is not unique.
        /// This address should not be assigned to the network interface.
        /// </summary>
        Duplicate,
        /// <summary>
        /// The address is valid, but it is nearing its lease lifetime and should not be used by applications.
        /// </summary>
        Deprecated,
        /// <summary>
        /// The address is valid and its use is unrestricted.
        /// </summary>
        Preferred
    }


}