// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.NetworkInformation.PingOptions
// 
// Copyright (c) 2003-2014 In The Hand Ltd, All rights reserved.

using System;

namespace InTheHand.Net.NetworkInformation
{
    /// <summary>
    /// Used to control how <see cref="Ping"/> data packets are transmitted.
    /// </summary>
    /// <remarks>The <see cref="PingOptions"/> class provides the <see cref="Ttl"/> and <see cref="DontFragment"/> properties to control how Internet Control Message Protocol (ICMP) echo request packets are transmitted.
    /// <para>The <see cref="Ttl"/> property specifies the Time to Live for packets sent by the <see cref="Ping"/> class.
    /// This value indicates the number of routing nodes that can forward a Ping packet before it is discarded.
    /// Setting this option is useful if you want to test the number of forwards, also known as hops, are required to send a packet from a source computer to a destination computer.</para>
    /// <para>The <see cref="DontFragment"/> property controls whether data sent to a remote host can be divided into multiple packets.
    /// This option is useful if you want to test the maximum transmission unit (MTU) of the routers and gateways used to transmit the packet.</para>
    /// <para>Instances of the <see cref="PingOptions"/> class are passed to the <see cref="Ping.Send(string)"/> method, and the <see cref="PingReply"/> class returns instances of <see cref="PingOptions"/> via the <see cref="PingReply.Options"/> property.</para>
    /// <para>For a list of initial property values for an instance of <see cref="PingOptions"/>, see the PingOptions constructor.</para></remarks>
    public class PingOptions
    {
        private bool dontFragment;
        private const int DontFragmentFlag = 2;
        private int ttl;

        /// <summary>
        /// Initializes a new instance of the <see cref="PingOptions"/> class.
        /// </summary>
        /// <remarks>The following table shows initial property values for an instance of PingOptions.
        /// <list><listheader><term>Property</term><description>Initial Value</description></listheader>
        /// <item><term>Ttl</term><description>128</description></item>
        /// <item><term>DontFragment</term><description>false</description></item></list>
        /// You can set the properties to new values before calling <see cref="Ping.Send(string)"/>.</remarks>
        public PingOptions()
        {
            this.ttl = 0x80;
        }

        internal PingOptions(IPOptions options)
        {
            this.ttl = 0x80;
            this.ttl = options.ttl;
            this.dontFragment = (options.flags & 2) > 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PingOptions"/> class and sets the Time to Live and fragmentation values. 
        /// </summary>
        /// <param name="ttl">An Int32 value greater than zero that specifies the number of times that the <see cref="Ping"/> data packets can be forwarded.</param>
        /// <param name="dontFragment">true to prevent data sent to the remote host from being fragmented; otherwise, false.</param>
        public PingOptions(int ttl, bool dontFragment)
        {
            this.ttl = 0x80;
            if (ttl <= 0)
            {
                throw new ArgumentOutOfRangeException("ttl");
            }
            this.ttl = ttl;
            this.dontFragment = dontFragment;
        }

        /// <summary>
        /// Gets or sets a <see cref="Boolean"/> value that controls fragmentation of the data sent to the remote host. 
        /// </summary>
        /// <remarks>Applications use this property to control whether data sent to a remote host by the Ping class can be divided into multiple packets.
        /// This option is useful if you want to test the maximum transmission unit (MTU) of the routers and gateways used to transmit the packet.
        /// If this property is true and the data sent to the remote host is larger then the MTU of a gateway or router between the sender and the remote host, the ping operation fails with status <see cref="IPStatus.PacketTooBig"/>.</remarks>
        public bool DontFragment
        {
            get
            {
                return this.dontFragment;
            }
            set
            {
                this.dontFragment = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of routing nodes that can forward the <see cref="Ping"/> data before it is discarded. 
        /// </summary>
        /// <remarks>As gateways and routers transmit packets through a network, they decrement the current Time-to-Live (TTL) value found in the packet header.
        /// If the TTL value reaches zero, the packet is deemed undeliverable and is discarded.
        /// This option is useful if you want to test the number of routers and gateways used to transmit the data.</remarks>
        public int Ttl
        {
            get
            {
                return this.ttl;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                this.ttl = value;
            }
        }
    }
}
