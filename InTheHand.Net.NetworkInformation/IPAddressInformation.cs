// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.NetworkInformation.IPAddressInformation
// 
// Copyright (c) 2010 In The Hand Ltd, All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Net;

namespace InTheHand.Net.NetworkInformation
{
    /// <summary>
    /// Provides information about a network interface address.
    /// </summary>
    public class IPAddressInformation
    {
        private IP_ADAPTER_ANYCAST_ADDRESS iaaa;

        internal IPAddressInformation() { }

        internal IPAddressInformation(IP_ADAPTER_ANYCAST_ADDRESS anycastAddress)
        {
            iaaa = anycastAddress;
            address = GetAddressFromSocketAddress(iaaa.Address.lpSockaddr);
            isDnsEligible = iaaa.Flags.HasFlag(IP_ADAPTER_ADDRESS.DNS_ELIGIBLE);
            isTransient = iaaa.Flags.HasFlag(IP_ADAPTER_ADDRESS.TRANSIENT);
        }

        internal static IPAddress GetAddressFromSocketAddress(IntPtr sa)
        {
            System.Net.Sockets.AddressFamily af = (System.Net.Sockets.AddressFamily)Marshal.ReadInt16(sa, 0);
            IPAddress address = null;
            switch (af)
            {
                case System.Net.Sockets.AddressFamily.InterNetwork:
                    address = new IPAddress(Marshal.ReadInt64(sa, 4));
                    break;

                case System.Net.Sockets.AddressFamily.InterNetworkV6:
                    byte[] addressBytes = new byte[16];
                    Marshal.Copy(IntPtrInTheHand.Add(sa, 8), addressBytes, 0, 16);
                    address = new IPAddress(addressBytes);
                    break;
            }

            return address;
        }

        internal IPAddress address;
        /// <summary>
        /// Gets the Internet Protocol (IP) address.
        /// </summary>
        public IPAddress Address
        {
            get
            {
                return address;
            }
        }

        internal bool isDnsEligible;
        /// <summary>
        /// Gets a Boolean value that indicates whether the Internet Protocol (IP) address is valid to appear in a Domain Name System (DNS) server database.
        /// </summary>
        public bool IsDnsEligible
        {
            get
            {
                return isDnsEligible;
            }
        }

        internal bool isTransient;
        /// <summary>
        /// Gets a Boolean value that indicates whether the Internet Protocol (IP) address is transient (a cluster address).
        /// </summary>
        public bool IsTransient
        {
            get
            {
                return isTransient;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct IP_ADAPTER_ANYCAST_ADDRESS
    {
        internal int Length;
        internal IP_ADAPTER_ADDRESS Flags;
        internal IntPtr Next;
        internal SOCKET_ADDRESS Address;
    }
}