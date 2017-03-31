// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.NetworkInformation.UnicastIPAddressInformation
// 
// Copyright (c) 2010 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Net.NetworkInformation
{
    /// <summary>
    /// Provides information about a network interface's unicast address.
    /// </summary>
    public sealed class UnicastIPAddressInformation : IPAddressInformation
    {
        private IP_ADAPTER_UNICAST_ADDRESS iaua;

        internal UnicastIPAddressInformation(IP_ADAPTER_UNICAST_ADDRESS unicastAddress)
        {
            this.iaua = unicastAddress;
            address = GetAddressFromSocketAddress(iaua.Address.lpSockaddr);
            isDnsEligible = iaua.Flags.HasFlag(IP_ADAPTER_ADDRESS.DNS_ELIGIBLE);
            isTransient = iaua.Flags.HasFlag(IP_ADAPTER_ADDRESS.TRANSIENT);
        }

        /// <summary>
        /// Gets the number of seconds remaining during which this address is the preferred address.
        /// </summary>
        public long AddressPreferredLifetime
        {
            get
            {
                
                return (long)iaua.PreferredLifetime;
            }
        }

        /// <summary>
        /// Gets the number of seconds remaining during which this address is valid.
        /// </summary>
        public long AddressValidLifetime
        {
            get
            {
                return (long)iaua.ValidLifetime;
            }
        }

        /// <summary>
        /// Specifies the amount of time remaining on the Dynamic Host Configuration Protocol (DHCP) lease for this IP address.
        /// </summary>
        public long DhcpLeaseLifetime
        {
            get
            {
                return (long)iaua.LeaseLifetime;
            }
        }

        /// <summary>
        /// Gets a value that indicates the state of the duplicate address detection algorithm.
        /// </summary>
        public DuplicateAddressDetectionState DuplicateAddressDetectionState
        {
            get
            {
                return iaua.DadState;
            }
        }

        /// <summary>
        /// Gets a value that identifies the source of a unicast Internet Protocol (IP) address prefix.
        /// </summary>
        public PrefixOrigin PrefixOrigin
        {
            get
            {
                return iaua.PrefixOrigin;
            }
        }

        /// <summary>
        /// Gets a value that identifies the source of a unicast Internet Protocol (IP) address suffix.
        /// </summary>
        public SuffixOrigin SuffixOrigin
        {
            get
            {
                return iaua.SuffixOrigin;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct IP_ADAPTER_UNICAST_ADDRESS
    {
        internal int Length;
        internal IP_ADAPTER_ADDRESS Flags;
        internal IntPtr Next;
        internal SOCKET_ADDRESS Address;
        internal PrefixOrigin PrefixOrigin;
        internal SuffixOrigin SuffixOrigin;
        internal DuplicateAddressDetectionState DadState;
        internal uint ValidLifetime;
        internal uint PreferredLifetime;
        internal uint LeaseLifetime;
    }
}