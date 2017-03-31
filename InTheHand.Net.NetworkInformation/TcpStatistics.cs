// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.NetworkInformation.TcpStatistics
// 
// Copyright (c) 2003-2009 In The Hand Ltd, All rights reserved.

using System;

namespace InTheHand.Net.NetworkInformation
{
    /// <summary>
    /// Provides Internet Protocol (IP) statistical data.
    /// </summary>
    /// <remarks><para>Equivalent to System.Net.NetworkInformation.TcpStatistics</para>
    /// Instances of this class are returned by the <see cref="IPGlobalProperties.GetTcpIPv4Statistics"/> and <see cref="IPGlobalProperties.GetTcpIPv6Statistics"/> methods, to give applications access to TCP traffic information.
    /// The information in this class correlates to the management information objects described in <a href="http://www.ietf.org/rfc/rfc2012.txt"/>.</remarks>
    public sealed class TcpStatistics
    {
#pragma warning disable 0649, 0169
        private int dwRtoAlgorithm;
        private int dwRtoMin;//
        private int dwRtoMax;//
        private int dwMaxConn;//
        private int dwActiveOpens;//
        private int dwPassiveOpens;//
        private int dwAttemptFails;//
        private int dwEstabResets;//
        private int dwCurrEstab;//
        private int dwInSegs;//
        private int dwOutSegs;//
        private int dwRetransSegs;//
        private int dwInErrs;//
        private int dwOutRsts;//
        private int dwNumConns; //

#pragma warning restore 0649, 0169

        internal TcpStatistics(){}

        /// <summary>
        /// Gets the number of accepted Transmission Control Protocol (TCP) connection requests.
        /// </summary>
        /// <value>An <see cref="Int64"/> value that specifies the total number of TCP connection requests accepted.</value>
        /// <remarks>IETF RFC 2012 formally defines this value as, "The number of times TCP connections have made a direct transition to the SYN-RCVD state from the LISTEN state."</remarks>
        public long ConnectionsAccepted
        {
            get
            {
                return dwPassiveOpens;
            }
        }

        /// <summary>
        /// Gets the number of Transmission Control Protocol (TCP) connection requests made by clients.
        /// </summary>
        /// <value>An <see cref="Int64"/> value that specifies the total number of TCP connections initiated by clients.</value>
        /// <remarks>IETF RFC 2012 formally defines this value as, "The number of times TCP connections have made a direct transition to the SYN-SENT state from the CLOSED state."</remarks>
        public long ConnectionsInitiated
        {
            get
            {
                return dwActiveOpens;
            }
        }

        /// <summary>
        /// Specifies the total number of Transmission Control Protocol (TCP) connections established.
        /// </summary>
        /// <value>An <see cref="Int64"/> value that specifies the total number of connections established.</value>
        public long CumulativeConnections
        {
            get
            {
                return dwNumConns;
            }
        }

        /// <summary>
        /// Gets the number of current Transmission Control Protocol (TCP) connections.
        /// </summary>
        /// <value>An <see cref="Int64"/> value that specifies the total number of current TCP connections.</value>
        /// <remarks>IETF RFC 2012 formally defines this value as, "The number of TCP connections for which the current state is either ESTABLISHED or CLOSE-WAIT."</remarks>
        public long CurrentConnections
        {
            get
            {
                return dwCurrEstab;
            }
        }

        /// <summary>
        /// Gets the number of Transmission Control Protocol (TCP) errors received.
        /// </summary>
        /// <value>An <see cref="Int64"/> value that specifies the total number of TCP errors received.</value>
        public long ErrorsReceived
        {
            get
            {
                return dwInErrs;
            }
        }

        /// <summary>
        /// Gets the number of failed Transmission Control Protocol (TCP) connection attempts.
        /// </summary>
        /// <value>An <see cref="Int64"/> value that specifies the total number of failed TCP connection attempts.</value>
        /// <remarks>IETF RFC 2012 formally defines this value as, "The number of times TCP connections have made a direct transition to the CLOSED state from either the SYN-SENT state or the SYN-RCVD state, plus the number of times TCP connections have made a direct transition to the LISTEN state from the SYN-RCVD state."</remarks>
        public long FailedConnectionAttempts
        {
            get
            {
                return dwAttemptFails;
            }
        }

        /// <summary>
        /// Gets the maximum number of supported Transmission Control Protocol (TCP) connections.
        /// </summary>
        /// <value>An <see cref="Int64"/> value that specifies the total number of TCP connections that can be supported.</value>
        /// <remarks>If the maximum number of connections is dynamic, this property returns -1.</remarks>
        public long MaximumConnections
        {
            get
            {
                return dwMaxConn;
            }
        }

        /// <summary>
        /// Gets the maximum retransmission time-out value for Transmission Control Protocol (TCP) segments.
        /// </summary>
        /// <value>An <see cref="Int64"/> value that specifies the maximum number of milliseconds permitted by a TCP implementation for the retransmission time-out value.</value>
        /// <remarks>TCP starts a retransmission timer when each outbound segment is passed to the network layer (IP).
        /// If no acknowledgement is received for the data in the segment before the timer expires, the segment is retransmitted.
        /// The timer can be set to any value between the <see cref="MinimumTransmissionTimeout"/> value and the <see cref="MaximumTransmissionTimeout"/> value.</remarks>
        public long MaximumTransmissionTimeout
        {
            get
            {
                return dwRtoMax;
            }
        }

        /// <summary>
        /// Gets the minimum retransmission time-out value for Transmission Control Protocol (TCP) segments.
        /// </summary>
        /// <value>An <see cref="Int64"/> value that specifies the minimum number of milliseconds permitted by a TCP implementation for the retransmission time-out value.</value>
        /// <remarks>TCP starts a retransmission timer when each outbound segment is passed to the network layer (IP). 
        /// If no acknowledgement is received for the data in the segment before the timer expires, the segment is retransmitted. 
        /// The timer can be set to any value between the <see cref="MinimumTransmissionTimeout"/> value and the <see cref="MaximumTransmissionTimeout"/> value.</remarks>
        public long MinimumTransmissionTimeout
        {
            get
            {
                return dwRtoMin;
            }
        }

        /// <summary>
        /// Gets the number of RST packets received by Transmission Control Protocol (TCP) connections.
        /// </summary>
        /// <value>An <see cref="Int64"/> value that specifies the total number of reset TCP connections.</value>
        /// <remarks>IETF RFC 2012 formally defines this value as, "The number of times TCP connections have made a direct transition to the CLOSED state from either the ESTABLISHED state or the CLOSE-WAIT state."</remarks>
        public long ResetConnections
        {
            get
            {
                return dwEstabResets;
            }
        }

        /// <summary>
        /// Gets the number of Transmission Control Protocol (TCP) segments sent with the reset flag set.
        /// </summary>
        /// <value>An <see cref="Int64"/> value that specifies the total number of TCP segments sent with the reset flag set.</value>
        /// <remarks>TCP resets are specified using the reset (RST) control bit in the TCP header.</remarks>
        public long ResetsSent
        {
            get
            {
                return dwOutRsts;
            }
        }

        /// <summary>
        /// Gets the number of Transmission Control Protocol (TCP) segments received.
        /// </summary>
        /// <value>An <see cref="Int64"/> value that specifies the total number of TCP segments received.</value>
        /// <remarks>This data includes segments with errors and segments received on currently established connections.</remarks>
        public long SegmentsReceived
        {
            get
            {
                return dwInSegs;
            }
        }

        /// <summary>
        /// Gets the number of Transmission Control Protocol (TCP) segments re-sent.
        /// </summary>
        /// <value>An <see cref="Int64"/> value that specifies the total number of TCP segments retransmitted.</value>
        /// <remarks>TCP segments that are not acknowledged as being received at the destination are retransmitted.</remarks>
        public long SegmentsResent
        {
            get
            {
                return dwRetransSegs;
            }
        }

        /// <summary>
        /// Gets the number of Transmission Control Protocol (TCP) segments sent.
        /// </summary>
        /// <value>An <see cref="Int64"/> value that specifies the total number of TCP segments sent.</value>
        /// <remarks>This data includes segments sent for currently established connections but does not include segments containing only retransmitted octets.</remarks>
        public long SegmentsSent
        {
            get
            {
                return dwOutSegs;
            }
        }
    }
}
