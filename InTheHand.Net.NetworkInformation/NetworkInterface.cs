// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.NetworkInformation.NetworkInterface
// 
// Copyright (c) 2003-2010 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Net;

namespace InTheHand.Net.NetworkInformation
{
    /// <summary>
    /// Provides configuration and statistical information for a network interface.
    /// </summary>
    /// <remarks>This class encapsulates data for network interfaces, also known as adapters, on the local computer.
    /// You do not create instances of this class; the <see cref="GetAllNetworkInterfaces"/> method returns an array that contains one instance of this class for each network interface on the local computer.
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
    /// </list>
    /// </remarks>
    public sealed class NetworkInterface
    {
        private NativeMethods.MIB_IFROW row;

        private NetworkInterface(string name, NativeMethods.MIB_IFROW row)
        {
            this.row = row;
            this.name = name;

            if (row.dwDescrLen > 0)
            {
                this.description = System.Text.Encoding.ASCII.GetString(row.bDescr, 0, row.dwDescrLen);
                int nullIndex = this.description.IndexOf('\0');
                if (nullIndex > -1)
                {
                    this.description = this.description.Substring(0, nullIndex);
                }
            }
        }

        /// <summary>
        /// Returns objects that describe the network interfaces on the local computer.
        /// </summary>
        /// <returns>A <see cref="NetworkInterface"/> array that contains objects that describe the available network interfaces, or an empty array if no interfaces are detected.</returns>
        /// <remarks>
        /// The network interfaces on a computer provide network connectivity.
        /// Network interfaces are also known as network adapters.
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
        /// </list>
        /// </remarks>
        public static NetworkInterface[] GetAllNetworkInterfaces()
        {
            if (!NativeMethods.hasIphlp)
            {
                return new NetworkInterface[0] { };
            }

            int size = 0;
            int result = NativeMethods.GetInterfaceInfo(IntPtr.Zero, ref size);

            IntPtr ptr = Marshal.AllocHGlobal(size);
            result = NativeMethods.GetInterfaceInfo(ptr, ref size);
            int numAdapters = Marshal.ReadInt32(ptr);
            NetworkInterface[] interfaces = new NetworkInterface[numAdapters];
            for (int i = 0; i < numAdapters; i++)
            {
                NativeMethods.IP_ADAPTER_INDEX_MAP aim = (NativeMethods.IP_ADAPTER_INDEX_MAP)Marshal.PtrToStructure(IntPtrInTheHand.Add(ptr, 4 + (i * 260)), typeof(NativeMethods.IP_ADAPTER_INDEX_MAP));
                NativeMethods.MIB_IFROW row = new NativeMethods.MIB_IFROW();
                row.dwIndex = aim.Index;
                result = NativeMethods.GetIfEntry(ref row);
                interfaces[i] = new NetworkInterface(aim.Name, row);
            }

            Marshal.FreeHGlobal(ptr);

            return interfaces;
        }

        /// <summary>
        /// Indicates whether any network connection is available.
        /// </summary>
        /// <returns>true if a network connection is available; otherwise, false.</returns>
        /// <remarks>A network connection is considered to be available if any network interface is marked "up" and is not a loopback interface.
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
        /// </list>
        /// </remarks>
        public static bool GetIsNetworkAvailable()
        {
            foreach (NetworkInterface ni in GetAllNetworkInterfaces())
            {
                if (((ni.OperationalStatus == OperationalStatus.Up) || (ni.OperationalStatus == OperationalStatus.Dormant)) && (ni.NetworkInterfaceType != NetworkInterfaceType.Loopback))
                {
                    return true;
                }
            }
            return false;
        }


        #region Name
        private string name = string.Empty;
        /// <summary>
        /// Gets the name of the network adapter.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }
        #endregion

        #region Description
        private string description = string.Empty;       
        /// <summary>
        /// Gets the description of the interface.
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
        }
        #endregion

        #region Type
        /// <summary>
        /// Gets the interface type.
        /// </summary>
        public NetworkInterfaceType NetworkInterfaceType
        {
            get
            {
                return row.dwType;
            }
        }
        #endregion

        #region Operational Status
        /// <summary>
        /// Gets the current operational state of the network connection.
        /// </summary>
        public OperationalStatus OperationalStatus
        {
            get
            {
                if (row.dwOperStatus == 0)
                {
                    return OperationalStatus.Unknown;
                }
                return this.row.dwOperStatus;
            }
        }
        #endregion

        #region Speed
        /// <summary>
        /// Gets the speed of the network interface.
        /// </summary>
        public long Speed
        {
            get
            {
                return row.dwSpeed;
            }
        }
        #endregion


        #region Get Physical Address
        /// <summary>
        /// Returns the Media Access Control (MAC) address for this adapter.
        /// </summary>
        /// <returns>A <see cref="PhysicalAddress"/> object that contains the physical address.</returns>
        /// <remarks>The object returned by this method contains an address that is appropriate to the media used to transport data at the data link layer.
        /// For example, on an Ethernet network, this method returns the Ethernet address.
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
        /// </list>
        /// </remarks>
        public PhysicalAddress GetPhysicalAddress()
        {
            byte[] address = new byte[this.row.dwPhysAddrLen];
            Array.Copy(this.row.bPhysAddr, address, this.row.dwPhysAddrLen);
            return new PhysicalAddress(address);
        }
        #endregion

        #region Get IPv4Statistics
        /// <summary>
        /// Gets the IPv4 statistics.
        /// </summary>
        /// <returns>An <see cref="IPv4InterfaceStatistics"/> object.</returns>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
        /// </list>
        /// </remarks>
        public IPv4InterfaceStatistics GetIPv4Statistics()
        {
            return new IPv4InterfaceStatistics(this.row);
        }
        #endregion

        #region Get IPProperties
        /// <summary>
        /// Returns an object that describes the configuration of this network interface.
        /// </summary>
        /// <returns>An <see cref="IPInterfaceProperties"/> object that describes this network interface.</returns>
        /// <remarks>Note that the information in the object returned by this method reflects the interfaces as of the time the array is created.
        /// This information is not updated dynamically.
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
        /// </list></remarks>
        public IPInterfaceProperties GetIPProperties()
        {
            IPInterfaceProperties ipp = null;
            
            IntPtr buffer = IntPtr.Zero;
            int bufferlen = 0;
            int result = NativeMethods.GetAdaptersAddresses(0, (GAA_FLAG)0, IntPtr.Zero, buffer, ref bufferlen);
            buffer = Marshal.AllocHGlobal(bufferlen);
            try
            {
                result = NativeMethods.GetAdaptersAddresses(0, (GAA_FLAG)0, IntPtr.Zero, buffer, ref bufferlen);
                if (result == 0)
                {
                    IP_ADAPTER_ADDRESSES ipaa = (IP_ADAPTER_ADDRESSES)Marshal.PtrToStructure(buffer, typeof(IP_ADAPTER_ADDRESSES));
                    while ((ipaa.IfIndex != this.row.dwIndex) && (ipaa.Next != IntPtr.Zero))
                    {
                        ipaa = (IP_ADAPTER_ADDRESSES)Marshal.PtrToStructure(ipaa.Next, typeof(IP_ADAPTER_ADDRESSES));
                    }
                    if (ipaa.IfIndex != this.row.dwIndex)
                    {
                        return null;
                    }

                    ipp = new IPInterfaceProperties(ipaa);
                    if (ipaa.FirstAnycastAddress != IntPtr.Zero)
                    {
                        IP_ADAPTER_ANYCAST_ADDRESS aa = (IP_ADAPTER_ANYCAST_ADDRESS)Marshal.PtrToStructure(ipaa.FirstAnycastAddress, typeof(IP_ADAPTER_ANYCAST_ADDRESS));
                        ipp.AnycastAddresses.InternalAdd(new IPAddressInformation(aa));
                        while (aa.Next != IntPtr.Zero)
                        {
                            aa = (IP_ADAPTER_ANYCAST_ADDRESS)Marshal.PtrToStructure(aa.Next, typeof(IP_ADAPTER_ANYCAST_ADDRESS));
                            ipp.AnycastAddresses.InternalAdd(new IPAddressInformation(aa));

                        }
                    }
                    if (ipaa.FirstMulticastAddress != IntPtr.Zero)
                    {
                        
                        IP_ADAPTER_MULTICAST_ADDRESS ma = (IP_ADAPTER_MULTICAST_ADDRESS)Marshal.PtrToStructure(ipaa.FirstMulticastAddress, typeof(IP_ADAPTER_MULTICAST_ADDRESS));
                        ipp.MulticastAddresses.InternalAdd(new MulticastIPAddressInformation(ma));

                        while(ma.Next != IntPtr.Zero)
                        {
                            ma = (IP_ADAPTER_MULTICAST_ADDRESS)Marshal.PtrToStructure(ma.Next, typeof(IP_ADAPTER_MULTICAST_ADDRESS));
                            ipp.MulticastAddresses.InternalAdd(new MulticastIPAddressInformation(ma));
                        
                        }
                    }
                    if (ipaa.FirstUnicastAddress != IntPtr.Zero)
                    {
                        IP_ADAPTER_UNICAST_ADDRESS ua = (IP_ADAPTER_UNICAST_ADDRESS)Marshal.PtrToStructure(ipaa.FirstUnicastAddress, typeof(IP_ADAPTER_UNICAST_ADDRESS));
                        ipp.UnicastAddresses.InternalAdd(new UnicastIPAddressInformation(ua));

                        while (ua.Next != IntPtr.Zero)
                        {
                            ua = (IP_ADAPTER_UNICAST_ADDRESS)Marshal.PtrToStructure(ua.Next, typeof(IP_ADAPTER_UNICAST_ADDRESS));
                            ipp.UnicastAddresses.InternalAdd(new UnicastIPAddressInformation(ua));

                        }
                    }
                    if (ipaa.FirstDnsServerAddress != IntPtr.Zero)
                    {
                        IP_ADAPTER_DNS_SERVER_ADDRESS da = (IP_ADAPTER_DNS_SERVER_ADDRESS)Marshal.PtrToStructure(ipaa.FirstDnsServerAddress, typeof(IP_ADAPTER_DNS_SERVER_ADDRESS));
                        ipp.DnsAddresses.InternalAdd(IPAddressInformation.GetAddressFromSocketAddress(da.Address.lpSockaddr));

                        while (da.Next != IntPtr.Zero)
                        {
                            da = (IP_ADAPTER_DNS_SERVER_ADDRESS)Marshal.PtrToStructure(ipaa.FirstDnsServerAddress, typeof(IP_ADAPTER_DNS_SERVER_ADDRESS)); 
                            ipp.DnsAddresses.InternalAdd(IPAddressInformation.GetAddressFromSocketAddress(da.Address.lpSockaddr));

                        }
                    }
                    
                }
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
            
            return ipp;
        }
        #endregion
    }

}
