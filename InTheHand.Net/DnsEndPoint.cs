// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DnsEndPoint.cs" company="In The Hand Ltd">
// Copyright (c) 2010-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Net;
using System.Net.Sockets;

namespace InTheHand.Net
{
    /// <summary>
    /// Represents a network endpoint as a host name or a string representation of an IP address and a port number.
    /// </summary>
    /// <remarks>The <see cref="DnsEndPoint"/> class contains a host name or an IP address and remote port information needed by an application to connect to a service on a host.
    /// By combining the host name or IP address and port number of a service, the <see cref="DnsEndPoint"/> class forms a connection point to a service.</remarks>
    public sealed class DnsEndPoint : EndPoint
    {
        private string host;
        private IPAddress resolvedHost = IPAddress.None;
        private int port;
        private AddressFamily addressFamily = AddressFamily.Unknown;

        /// <summary>
        /// Gets the Internet Protocol (IP) address family.
        /// </summary>
        public new AddressFamily AddressFamily
        { 
            get
            {
                return addressFamily;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DnsEndPoint"/> class with the host name or string representation of an IP address and a port number.
        /// </summary>
        /// <param name="host">The host name or a string representation of the IP address.</param>
        /// <param name="port">The port number associated with the address, or 0 to specify any available port. port is in host order.</param>
        /// <remarks>The DnsEndPoint(String, Int32) constructor can be used to initialize a <see cref="DnsEndPoint"/> class using either a host name or a string that represents an IP address and a port.
        /// This constructor sets the <see cref="AddressFamily"/> property to <see cref="System.Net.Sockets.AddressFamily.Unknown"/>. 
        /// When using this constructor with a host name rather than a string representation of an IP address, the address family of the <see cref="DnsEndPoint"/> will remain <see cref="System.Net.Sockets.AddressFamily.Unknown"/> even after use.
        /// The <see cref="AddressFamily"/> property of any <see cref="Socket"/> that is created by calls to the ConnectAsync method will be the address family of the first address to which a connection can be successfully established (not necessarily the first address to be resolved).</remarks>
        public DnsEndPoint(string host, int port) : this(host, port, AddressFamily.Unknown) {}
        /// <summary>
        /// Initializes a new instance of the <see cref="DnsEndPoint"/> class with the host name or string representation of an IP address and a port number.
        /// </summary>
        /// <param name="host">The host name or a string representation of the IP address.</param>
        /// <param name="port">The port number associated with the address, or 0 to specify any available port. port is in host order.</param>
        /// <param name="addressFamily">The address family of the address.</param>
        /// <remarks>The DnsEndPoint(String, Int32) constructor can be used to initialize a <see cref="DnsEndPoint"/> class using either a host name or a string that represents an IP address and a port.
        /// When using this constructor with a host name rather than a string representation of an IP address, the address family of the <see cref="DnsEndPoint"/> will remain <see cref="System.Net.Sockets.AddressFamily.Unknown"/> even after use.
        /// The <see cref="AddressFamily"/> property of any <see cref="Socket"/> that is created by calls to the ConnectAsync method will be the address family of the first address to which a connection can be successfully established (not necessarily the first address to be resolved).</remarks>
        public DnsEndPoint(string host, int port, AddressFamily addressFamily)
        {
            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentException("host");
            }
            if (port > IPEndPoint.MaxPort || port < IPEndPoint.MinPort)
            {
                throw new ArgumentOutOfRangeException("port");
            }
            

            this.host = host;
            this.port = port;

            this.addressFamily = addressFamily;
        }

        /// <summary>
        /// Gets the host name or string representation of the Internet Protocol (IP) address of the host.
        /// </summary>
        public string Host
        {
            get
            {
                return this.host;
            }
        }

        /// <summary>
        /// Gets the port number of the <see cref="DnsEndPoint"/>.
        /// </summary>
        public int Port
        {
            get
            {
                return this.port;
            }
        }

        /// <summary>
        /// Serializes endpoint information into a <see cref="SocketAddress"/> instance.
        /// </summary>
        /// <returns></returns>
        public override SocketAddress Serialize()
        {
            if (resolvedHost == IPAddress.None)
            {
                IPAddress[] IPs = Dns.GetHostEntry(host).AddressList;
                if (IPs.Length > 0)
                {
                    resolvedHost = IPs[0];
                    this.addressFamily = IPs[0].AddressFamily;
                }
                else
                {
                    resolvedHost = IPAddress.None;
                }
            }
            SocketAddress sa = new SocketAddress(this.addressFamily,16);

            byte[] addressBytes = resolvedHost.GetAddressBytes();
            for (int i = 0; i < 4; i++)
            {
                sa[i+4] = addressBytes[i];
            }
            byte[] portBytes = BitConverter.GetBytes(port);
            sa[3] = portBytes[0];
            sa[2] = portBytes[1];
            return sa;
        }

        /// <summary>
        /// Creates an endpoint from a socket address.
        /// </summary>
        /// <param name="socketAddress"></param>
        /// <returns></returns>
        public override EndPoint Create(SocketAddress socketAddress)
        {
            byte[] addressBytes = new byte[4]; 
            byte[] portBytes = new byte[2];
            for (int i = 0; i < 2; i++)
            {
                portBytes[i] = socketAddress[i + 2];
            }
            for (int i = 0; i < 4; i++)
            {
                addressBytes[i] = socketAddress[i+4];
            }
            int port = System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt16(portBytes, 0));
            IPAddress address = new IPAddress(addressBytes);

            return new DnsEndPoint(address.ToString(), port, socketAddress.Family);
        }
    }
}
