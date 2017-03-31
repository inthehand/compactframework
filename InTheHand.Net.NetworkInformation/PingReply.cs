// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.NetworkInformation.PingReply
// 
// Copyright (c) 2003-2014 In The Hand Ltd, All rights reserved.

using System.Net;
using System.Runtime.InteropServices;

namespace InTheHand.Net.NetworkInformation
{
    /// <summary>
    /// Provides information about the status and data resulting from a <see cref="Ping.Send(string)"/> operation.
    /// </summary>
    public class PingReply
    {
        private IPAddress address;
        private byte[] buffer;
        private IPStatus ipStatus;
        private PingOptions options;
        private long rtt;


        internal PingReply(IcmpEchoReply reply)
        {
            this.address = new IPAddress((long)reply.address);
            this.ipStatus = (IPStatus)reply.status;
            if (this.ipStatus == IPStatus.Success)
            {
                this.rtt = reply.roundTripTime;
                this.buffer = new byte[reply.dataSize];
                Marshal.Copy(reply.data, this.buffer, 0, reply.dataSize);
                this.options = new PingOptions(reply.options);
            }
            else
            {
                this.buffer = new byte[0];
            }
        }

        internal PingReply(IPStatus ipStatus)
        {
            this.ipStatus = ipStatus;
            this.buffer = new byte[0];
        }

        /// <summary>
        /// Gets the address of the host that sends the Internet Control Message Protocol (ICMP) echo reply.
        /// </summary>
        public IPAddress Address
        {
            get
            {
                return this.address;
            }
        }

        /// <summary>
        /// Gets the buffer of data received in an Internet Control Message Protocol (ICMP) echo reply message. 
        /// </summary>
        public byte[] Buffer
        {
            get
            {
                return this.buffer;
            }
        }

        /// <summary>
        /// Gets the options used to transmit the reply to an Internet Control Message Protocol (ICMP) echo request. 
        /// </summary>
        public PingOptions Options
        {
            get
            {
                return this.options;
            }
        }

        /// <summary>
        /// Gets the number of milliseconds taken to send an Internet Control Message Protocol (ICMP) echo request and receive the corresponding ICMP echo reply message. 
        /// </summary>
        public long RoundtripTime
        {
            get
            {
                return this.rtt;
            }
        }

        /// <summary>
        /// Gets the status of an attempt to send an Internet Control Message Protocol (ICMP) echo request and receive the corresponding ICMP echo reply message. 
        /// </summary>
        public IPStatus Status
        {
            get
            {
                return this.ipStatus;
            }
        }
    }
}
