// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.NetworkInformation.UdpStatistics
// 
// Copyright (c) 2003-2011 In The Hand Ltd, All rights reserved.

using System;

namespace InTheHand.Net.NetworkInformation
{
    /// <summary>
    /// Provides User Datagram Protocol (UDP) statistical data. 
    /// </summary>
    /// <remarks><para>Equivalent to System.Net.NetworkInformation.UdpStatistics</para>
    /// Instances of this class are returned by the <see cref="IPGlobalProperties.GetUdpIPv4Statistics"/> and <see cref="IPGlobalProperties.GetUdpIPv6Statistics"/> methods to give applications access to UDP traffic information.
    /// The information in this class correlates to the management information objects described in <a href="http://www.ietf.org/rfc/rfc2013.txt"/>.
    /// </remarks>
    public sealed class UdpStatistics
    {
#pragma warning disable 0649, 0169
        private int dwInDatagrams; //
        private int dwNoPorts; //
        private int dwInErrors; //
        private int dwOutDatagrams; //
        private int dwNumAddrs;
#pragma warning restore 0649, 0169

        internal UdpStatistics(){}

        /// <summary>
        /// Gets the number of User Datagram Protocol (UDP) datagrams that were received.
        /// </summary>
        /// <value>An <see cref="Int64"/> value that specifies the total number of datagrams that were delivered to UDP users.</value>
        /// <remarks>The number returned by this property does not include datagrams that were received but not deliverable.</remarks>
        public long DatagramsReceived
        {
            get
            {
                return dwInDatagrams;
            }
        }

        /// <summary>
        /// Gets the number of User Datagram Protocol (UDP) datagrams that were sent.
        /// </summary>
        /// <value>An <see cref="Int64"/> value that specifies the total number of datagrams that were sent.</value>
        public long DatagramsSent
        {
            get
            {
                return dwOutDatagrams;
            }
        }

        /// <summary>
        /// Gets the number of User Datagram Protocol (UDP) datagrams that were received and discarded because of port errors.
        /// </summary>
        /// <value>An <see cref="Int64"/> value that specifies the total number of received UDP datagrams that were discarded because there was no listening application at the destination port.</value>
        /// <remarks>To find the total number of datagrams that could not be delivered, add the values that were returned by this property and the <see cref="IncomingDatagramsWithErrors"/> property.</remarks>
        public long IncomingDatagramsDiscarded
        {
            get
            {
                return dwNoPorts;
            }
        }

        /// <summary>
        /// Gets the number of User Datagram Protocol (UDP) datagrams that were received and discarded because of errors other than bad port information.
        /// </summary>
        /// <value>An <see cref="Int64"/> value that specifies the total number of received UDP datagrams that could not be delivered for reasons other than the lack of an application at the destination port.</value>
        /// <remarks>To find the total number of datagrams that could not be delivered, add the values that were returned by this property and the <see cref="IncomingDatagramsDiscarded"/> property.</remarks>
        public long IncomingDatagramsWithErrors
        {
            get
            {
                return dwInErrors;
            }
        }

        /// <summary>
        /// Gets the number of local endpoints that are listening for User Datagram Protocol (UDP) datagrams.
        /// </summary>
        /// <value>An <see cref="Int64"/> value that specifies the total number of sockets that are listening for UDP datagrams.</value>
        /// <remarks>You can use the <see cref="System.Net.Sockets.UdpClient"/> and <see cref="System.Net.Sockets.Socket"/> classes to create UDP listener applications.</remarks>
        public int UdpListeners
        {
            get
            {
                return dwNumAddrs;
            }
        }
    }
}
