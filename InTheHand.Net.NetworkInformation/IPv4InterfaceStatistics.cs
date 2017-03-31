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
    /// Provides statistical data for a network interface on the local computer.
    /// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
    /// </list>
    /// </remarks>
    public sealed class IPv4InterfaceStatistics
    {
        private NativeMethods.MIB_IFROW ifRow;

        private IPv4InterfaceStatistics()
        {
            this.ifRow = new NativeMethods.MIB_IFROW();
        }

        internal IPv4InterfaceStatistics(NativeMethods.MIB_IFROW row)
        {
            this.ifRow = row;
        }

        internal IPv4InterfaceStatistics(uint index)
        {
            this.ifRow = new NativeMethods.MIB_IFROW();
            this.ifRow.dwIndex = index;
            int result = NativeMethods.GetIfEntry(ref this.ifRow);

            if (result < 0)
            {
                throw new NetworkInformationException(result);
            }
        }

        /// <summary>
        /// Gets the number of bytes that were received on the interface.
        /// </summary>
        public long BytesReceived
        {
            get
            {
                return (long)this.ifRow.dwInOctets;
            }
        }
        /// <summary>
        /// Gets the number of bytes that were sent on the interface.
        /// </summary>
        public long BytesSent
        {
            get
            {
                return (long)this.ifRow.dwOutOctets;
            }
        }
        /// <summary>
        /// Gets the number of incoming packets that were discarded.
        /// </summary>
        public long IncomingPacketsDiscarded
        {
            get
            {
                return (long)this.ifRow.dwInDiscards;
            }
        }
        /// <summary>
        /// Gets the number of incoming packets with errors.
        /// </summary>
        public long IncomingPacketsWithErrors
        {
            get
            {
                return (long)this.ifRow.dwInErrors;
            }
        }
        /// <summary>
        /// Gets the number of incoming packets with an unknown protocol.
        /// </summary>
        public long IncomingUnknownProtocolPackets
        {
            get
            {
                return (long)this.ifRow.dwInUnknownProtos;
            }
        }

        /// <summary>
        /// Gets the number of non-unicast packets that were received on the interface.
        /// </summary>
        public long NonUnicastPacketsReceived
        {
            get
            {
                return (long)this.ifRow.dwInNUcastPkts;
            }
        }
        /// <summary>
        /// Gets the number of non-unicast packets that were sent on the interface.
        /// </summary>
        public long NonUnicastPacketsSent
        {
            get
            {
                return (long)this.ifRow.dwOutNUcastPkts;
            }
        }
        /*
        // <summary>
        // 
        // </summary>
        internal OperationalStatus OperationalStatus
        {
            get
            {
                return this.ifRow.dwOperStatus;
            }
        }*/

        /// <summary>
        /// Gets the number of outgoing packets that were discarded.
        /// </summary>
        public long OutgoingPacketsDiscarded
        {
            get
            {
                return (long)this.ifRow.dwOutDiscards;
            }
        }

        /// <summary>
        /// Gets the number of outgoing packets with errors.
        /// </summary>
        public long OutgoingPacketsWithErrors
        {
            get
            {
                return (long)this.ifRow.dwOutErrors;
            }
        }

        /// <summary>
        /// Gets the length of the output queue.
        /// </summary>
        public long OutputQueueLength
        {
            get
            {
                return (long)this.ifRow.dwOutQLen;
            }
        }

        /// <summary>
        /// Gets the number of unicast packets that were received on the interface.
        /// </summary>
        public long UnicastPacketsReceived
        {
            get
            {
                return (long)this.ifRow.dwInUcastPkts;
            }
        }

        /// <summary>
        /// Gets the number of unicast packets that were sent on the interface.
        /// </summary>
        public long UnicastPacketsSent
        {
            get
            {
                return (long)this.ifRow.dwOutUcastPkts;
            }
        }
    }
}
