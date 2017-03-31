// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.NetworkInformation.MulticastIPAddressInformation
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
    /// Provides information about a network interface's multicast address.
    /// </summary>
    public sealed class MulticastIPAddressInformation : IPAddressInformation
    {
        private IP_ADAPTER_MULTICAST_ADDRESS iama;

        internal MulticastIPAddressInformation(IP_ADAPTER_MULTICAST_ADDRESS multicastAddress)
        {
            this.iama = multicastAddress;
            address = GetAddressFromSocketAddress(iama.Address.lpSockaddr);
            isDnsEligible = iama.Flags.HasFlag(IP_ADAPTER_ADDRESS.DNS_ELIGIBLE);
            isTransient = iama.Flags.HasFlag(IP_ADAPTER_ADDRESS.TRANSIENT);
        }

        /*public long AddressPreferredLifetime
        {
            get
            {
                return 0;
            }
        }

        public long AddressValidLifetime
        {
            get
            {
                return 0;
            }
        }

        public long DhcpLeaseLifetime
        {
            get
            {
                return 0;
            }
        }

        public DuplicateAddressDetectionState DuplicateAddressDetectionState
        {
            get
            {
                return DuplicateAddressDetectionState.Invalid;
            }
        }

        public PrefixOrigin PrefixOrigin
        {
            get
            {
                return PrefixOrigin.Other;
            }
        }

        public SuffixOrigin SuffixOrigin
        {
            get
            {
                return SuffixOrigin.Other;
            }
        }*/
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct IP_ADAPTER_MULTICAST_ADDRESS
    {
        internal int Length;
        internal IP_ADAPTER_ADDRESS Flags;
        internal IntPtr Next;
        internal SOCKET_ADDRESS Address;
    }
}