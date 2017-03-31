// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.NetworkInformation.Ping
// 
// Copyright (c) 2003-2010 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Net;

namespace InTheHand.Net.NetworkInformation
{
    /// <summary>
    /// Allows an application to determine whether a remote computer is accessible over the network.
    /// </summary>
    /// <remarks>Applications use the Ping class to detect whether a remote computer is reachable. 
    /// <para>Network topology can determine whether Ping can successfully contact a remote host. 
    /// The presence and configuration of proxies, network address translation (NAT) equipment, or firewalls can prevent Ping from succeeding. 
    /// A successful Ping indicates only that the remote host can be reached on the network; the presence of higher level services (such as a Web server) on the remote host is not guaranteed.</para>
    /// The <see cref="Send(string)"/> method send an Internet Control Message Protocol (ICMP) echo request message to a remote computer and waits for an ICMP echo reply message from that computer. 
    /// For a detailed description of ICMP messages, see RFC 792, available at <a href="http://www.ietf.org">http://www.ietf.org</a>.
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.0 and later</description></item>
    /// </list>
    /// </remarks>
    /// <example>The following code example demonstrates using the Ping class.
    /// <code lang="cs">
    /// using System;
    /// using System.Net;
    /// using InTheHand.Net.NetworkInformation;
    /// using System.Text;
    /// 
    /// namespace Examples.InTheHand.Net.NetworkInformation.PingTest
    /// {
    ///     public class PingExample
    ///     {
    ///         // args[0] can be an IPaddress or host name.
    ///         public static void Main (string[] args)
    ///         {
    ///             Ping pingSender = new Ping ();
    ///             PingOptions options = new PingOptions ();
    ///             // change the fragmentation behavior.
    ///             options.DontFragment = true;
    /// 
    ///             // Create a buffer of 32 bytes of data to be transmitted.
    ///             string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
    ///             byte[] buffer = Encoding.ASCII.GetBytes (data);
    ///             int timeout = 120;
    ///             PingReply reply = pingSender.Send (args[0], timeout, buffer, options);
    ///             if (reply.Status == IPStatus.Success)
    ///             {
    ///                 string message = string.Format("Address: {0}\r\nRoundTrip time: {1}\r\nTime to live: {2}\r\nDon't fragment: {3}\r\nBuffer size: {0}",
    ///                     reply.Address.ToString(),
    ///                     reply.RoundtripTime,
    ///                     reply.Options.Ttl,
    ///                     reply.Options.DontFragment,
    ///                     reply.Buffer.Length);
    ///                 MessageBox.Show(message, "Ping");
    ///             }
    ///             else
    ///             {
    ///                 MessageBox.Show(reply.Status.ToString(), "Ping");
    ///             }
    ///         }
    ///     }
    /// }</code></example>
    public class Ping : Component
    {
        //connection handle
        private IntPtr handle;
        private IntPtr replyBuffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Ping"/> class.
        /// </summary>
        public Ping()
        {
            this.handle = NativeMethods.IcmpCreateFile();

            if (handle == IntPtr.Zero)
            {
                throw InTheHand.ComponentModel.Win32ExceptionInTheHand.Create();
            }

            this.replyBuffer = Marshal.AllocHGlobal(0x100ff);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (handle != IntPtr.Zero)
            {
                bool success = NativeMethods.IcmpCloseHandle(handle);
                handle = IntPtr.Zero;
            }

            if (replyBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(replyBuffer);
                replyBuffer = IntPtr.Zero;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Attempts to send an Internet Control Message Protocol (ICMP) echo message to the specified computer, and receive a corresponding ICMP echo reply message from that computer.
        /// </summary>
        /// <param name="hostNameOrAddress">A <see cref="String"/> that identifies the computer that is the destination for the ICMP echo message.
        /// The value specified for this parameter can be a host name or a string representation of an IP address.</param>
        /// <returns>A <see cref="PingReply"/> object that provides information about the ICMP echo reply message, if one was received, or provides the reason for the failure, if no message was received.</returns>
        /// <exception cref="ArgumentNullException">hostNameOrAddress is a null reference (Nothing in Visual Basic).</exception>
        public PingReply Send(string hostNameOrAddress)
        {
            return Send(hostNameOrAddress, DefaultTimeout, DefaultSendBuffer, null);
        }

        /// <summary>
        /// Attempts to send an Internet Control Message Protocol (ICMP) echo message to the computer that has the specified <see cref="IPAddress"/>, and receive a corresponding ICMP echo reply message from that computer. 
        /// </summary>
        /// <param name="address">An <see cref="IPAddress"/> that identifies the computer that is the destination for the ICMP echo message.</param>
        /// <returns>A <see cref="PingReply"/> object that provides information about the ICMP echo reply message, if one was received, or describes the reason for the failure if no message was received.</returns>
        /// <exception cref="ArgumentNullException">address is a null reference (Nothing in Visual Basic).</exception>
        public PingReply Send(IPAddress address)
        {
            return this.Send(address, DefaultTimeout, this.DefaultSendBuffer, null);
        }

        /// <summary>
        /// Attempts to send an Internet Control Message Protocol (ICMP) echo message with the specified data buffer to the computer that has the specified <see cref="IPAddress"/>, and receive a corresponding ICMP echo reply message from that computer. This overload allows you to specify a time-out value for the operation. 
        /// </summary>
        /// <param name="address">An <see cref="IPAddress"/> that identifies the computer that is the destination for the ICMP echo message.</param>
        /// <param name="timeout">An <see cref="Int32"/> value that specifies the maximum number of milliseconds (after sending the echo message) to wait for the ICMP echo reply message.</param>
        /// <returns>A <see cref="PingReply"/> object that provides information about the ICMP echo reply message if one was received, or provides the reason for the failure if no message was received.</returns>
        /// <exception cref="ArgumentNullException">address is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentOutOfRangeException">timeout is less than zero.</exception>
        public PingReply Send(IPAddress address, int timeout)
        {
            return this.Send(address, timeout, this.DefaultSendBuffer, null);
        }

        /// <summary>
        /// Attempts to send an Internet Control Message Protocol (ICMP) echo message to the specified computer, and receive a corresponding ICMP echo reply message from that computer.
        /// This overload allows you to specify a time-out value for the operation. 
        /// </summary>
        /// <param name="hostNameOrAddress">A <see cref="String"/> that identifies the computer that is the destination for the ICMP echo message.
        /// The value specified for this parameter can be a host name or a string representation of an IP address.</param>
        /// <param name="timeout">An <see cref="Int32"/> value that specifies the maximum number of milliseconds (after sending the echo message) to wait for the ICMP echo reply message.</param>
        /// <returns>A <see cref="PingReply"/> object that provides information about the ICMP echo reply message if one was received, or provides the reason for the failure if no message was received.</returns>
        /// <exception cref="ArgumentNullException">hostNameOrAddress is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentOutOfRangeException">timeout is less than zero.</exception>
        public PingReply Send(string hostNameOrAddress, int timeout)
        {
            return this.Send(hostNameOrAddress, timeout, this.DefaultSendBuffer, null);
        }

        /// <summary>
        /// Attempts to send an Internet Control Message Protocol (ICMP) echo message with the specified data buffer to the computer that has the specified IPAddress, and receive a corresponding ICMP echo reply message from that computer.
        /// This overload allows you to specify a time-out value for the operation. 
        /// </summary>
        /// <param name="address">An <see cref="IPAddress"/> that identifies the computer that is the destination for the ICMP echo message.</param>
        /// <param name="timeout">An <see cref="Int32"/> value that specifies the maximum number of milliseconds (after sending the echo message) to wait for the ICMP echo reply message.</param>
        /// <param name="buffer">A <see cref="Byte"/> array that contains data to be sent with the ICMP echo message and returned in the ICMP echo reply message.
        /// The array cannot contain more than 65,500 bytes.</param>
        /// <returns>A <see cref="PingReply"/> object that provides information about the ICMP echo reply message if one was received, or provides the reason for the failure if no message was received.</returns>
        /// <exception cref="ArgumentNullException">address is a null reference (Nothing in Visual Basic).
        /// <para>-or-</para>
        /// buffer is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentOutOfRangeException">timeout is less than zero.</exception>
        /// <exception cref="ArgumentException">The size of buffer exceeds 65500 bytes.
        /// <para>-or-</para>
        /// address is not a valid IP address.</exception>
        public PingReply Send(IPAddress address, int timeout, byte[] buffer)
        {
            return this.Send(address, timeout, buffer, null);
        }

        /// <summary>
        /// Attempts to send an Internet Control Message Protocol (ICMP) echo message with the specified data buffer to the specified computer, and receive a corresponding ICMP echo reply message from that computer. This overload allows you to specify a time-out value for the operation. 
        /// </summary>
        /// <param name="hostNameOrAddress">A <see cref="String"/> that identifies the computer that is the destination for the ICMP echo message.
        /// The value specified for this parameter can be a host name or a string representation of an IP address.</param>
        /// <param name="timeout">An <see cref="Int32"/> value that specifies the maximum number of milliseconds (after sending the echo message) to wait for the ICMP echo reply message.</param>
        /// <param name="buffer">A <see cref="Byte"/> array that contains data to be sent with the ICMP echo message and returned in the ICMP echo reply message.
        /// The array cannot contain more than 65,500 bytes.</param>
        /// <returns>A <see cref="PingReply"/> object that provides information about the ICMP echo reply message if one was received, or provides the reason for the failure if no message was received.</returns>
        /// <exception cref="ArgumentNullException">hostNameOrAddress is a null reference (Nothing in Visual Basic).
        /// <para>-or-</para>
        /// buffer is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentOutOfRangeException">timeout is less than zero.</exception>
        /// <exception cref="ArgumentException">The size of buffer exceeds 65500 bytes.
        /// <para>-or-</para>
        /// address is not a valid IP address.</exception>
        public PingReply Send(string hostNameOrAddress, int timeout, byte[] buffer)
        {
            return this.Send(hostNameOrAddress, timeout, buffer, null);
        }

        /// <summary>
        /// Attempts to send an Internet Control Message Protocol (ICMP) echo message with the specified data buffer to the computer that has the specified IPAddress and receive a corresponding ICMP echo reply message from that computer. This overload allows you to specify a time-out value for the operation and control fragmentation and Time-to-Live values for the ICMP echo message packet. 
        /// </summary>
        /// <param name="address">An <see cref="IPAddress"/> that identifies the computer that is the destination for the ICMP echo message.</param>
        /// <param name="timeout">An <see cref="Int32"/> value that specifies the maximum number of milliseconds (after sending the echo message) to wait for the ICMP echo reply message.</param>
        /// <param name="buffer">A <see cref="Byte"/> array that contains data to be sent with the ICMP echo message and returned in the ICMP echo reply message.
        /// The array cannot contain more than 65,500 bytes.</param>
        /// <param name="options">A <see cref="PingOptions"/> object used to control fragmentation and Time-to-Live values for the ICMP echo message packet.</param>
        /// <returns>A <see cref="PingReply"/> object that provides information about the ICMP echo reply message, if one was received, or provides the reason for the failure, if no message was received.
        /// The method will return PacketTooBig if the packet exceeds the Maximum Transmission Unit (MTU). </returns>
        /// <exception cref="ArgumentNullException">address is a null reference (Nothing in Visual Basic).
        /// <para>-or-</para>
        /// buffer is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentOutOfRangeException">timeout is less than zero.</exception>
        /// <exception cref="ArgumentException">The size of buffer exceeds 65500 bytes.
        /// <para>-or-</para>
        /// address is not a valid IP address.</exception>
        public PingReply Send(IPAddress address, int timeout, byte[] buffer, PingOptions options)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            if (buffer.Length > 0xffdc)
            {
                throw new ArgumentException(Properties.Resources.net_invalidPingBufferSize, "buffer");
            }

            if (timeout < 0)
            {
                throw new ArgumentOutOfRangeException("timeout");
            }

            if (address == null)
            {
                throw new ArgumentNullException("address");
            }

            if (address.Equals(IPAddress.Any) || address.Equals(IPAddress.IPv6Any))
            {
                throw new ArgumentException(Properties.Resources.net_invalid_ip_addr, "address");
            }

            int a = BitConverter.ToInt32(address.GetAddressBytes(), 0);
            IPOptions options2 = new IPOptions(options);

            int returned = NativeMethods.IcmpSendEcho(handle, a, buffer, (short)buffer.Length, ref options2, replyBuffer, 0x100ff, timeout);

            if (returned == 0)
            {
                returned = Marshal.GetLastWin32Error();
                if (returned != 0)
                {
                    return new PingReply((IPStatus)returned);
                }
            }

            return new PingReply((IcmpEchoReply)Marshal.PtrToStructure(this.replyBuffer, typeof(IcmpEchoReply)));
        }

        /// <summary>
        /// Attempts to send an Internet Control Message Protocol (ICMP) echo message with the specified data buffer to the specified computer, and receive a corresponding ICMP echo reply message from that computer.
        /// This overload allows you to specify a time-out value for the operation and control fragmentation and Time-to-Live values for the ICMP packet. 
        /// </summary>
        /// <param name="hostNameOrAddress">A <see cref="String"/> that identifies the computer that is the destination for the ICMP echo message.
        /// The value specified for this parameter can be a host name or a string representation of an IP address.</param>
        /// <param name="timeout">An <see cref="Int32"/> value that specifies the maximum number of milliseconds (after sending the echo message) to wait for the ICMP echo reply message.</param>
        /// <param name="buffer">A <see cref="Byte"/> array that contains data to be sent with the ICMP echo message and returned in the ICMP echo reply message.
        /// The array cannot contain more than 65,500 bytes.</param>
        /// <param name="options">A <see cref="PingOptions"/> object used to control fragmentation and Time-to-Live values for the ICMP echo message packet.</param>
        /// <returns>A <see cref="PingReply"/> object that provides information about the ICMP echo reply message, if one was received, or provides the reason for the failure, if no message was received.
        /// The method will return PacketTooBig if the packet exceeds the Maximum Transmission Unit (MTU).</returns>
        /// <exception cref="ArgumentNullException">hostNameOrAddress is a null reference (Nothing in Visual Basic).
        /// <para>-or-</para>
        /// buffer is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentOutOfRangeException">timeout is less than zero.</exception>
        /// <exception cref="ArgumentException">The size of buffer exceeds 65500 bytes.
        /// <para>-or-</para>
        /// address is not a valid IP address.</exception>
        public PingReply Send(string hostNameOrAddress, int timeout, byte[] buffer, PingOptions options)
        {
            IPAddress address;
            if (string.IsNullOrEmpty(hostNameOrAddress))
            {
                throw new ArgumentNullException("hostNameOrAddress");
            }

            try
            {
                address = Dns.GetHostEntry(hostNameOrAddress).AddressList[0];
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new PingException("An exception occurred during a Ping request.", exception);
            }

            return this.Send(address, timeout, buffer, options);
        }

        private byte[] defaultSendBuffer;
        private byte[] DefaultSendBuffer
        {
            get
            {
                if (this.defaultSendBuffer == null)
                {
                    this.defaultSendBuffer = new byte[0x20];
                    for (int i = 0; i < 0x20; i++)
                    {
                        this.defaultSendBuffer[i] = (byte)(0x61 + (i % 0x17));
                    }
                }
                return this.defaultSendBuffer;
            }
        }

        private const int DefaultTimeout = 0x1388;

        private static class NativeMethods
        {
            [DllImport("iphlpapi.dll", EntryPoint = "IcmpCreateFile", SetLastError = true)]
            internal static extern IntPtr IcmpCreateFile();

            [DllImport("iphlpapi.dll", EntryPoint = "IcmpCloseHandle")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool IcmpCloseHandle(IntPtr handle);

            [DllImport("iphlpapi.dll", EntryPoint = "IcmpSendEcho", SetLastError = true)]
            internal static extern int IcmpSendEcho(IntPtr icmpHandle, int destinationAddress,
                byte[] requestData, short requestSize,
                ref IPOptions requestOptions,
                IntPtr replyBuffer, int replySize,
                int timeout);
        }

    }

    


    #region IPOptions
    [StructLayout(LayoutKind.Sequential)]
    internal struct IPOptions
    {
        internal byte ttl;
        internal byte tos;
        internal byte flags;
        internal byte optionsSize;
        internal IntPtr optionsData;
        internal IPOptions(PingOptions options)
        {
            ttl = 0x80;
            tos = 0;
            flags = 0;
            optionsSize = 0;
            optionsData = IntPtr.Zero;

            if (options != null)
            {
                this.ttl = (byte)options.Ttl;
                if (options.DontFragment)
                {
                    this.flags = 2;
                }
            }
        }
    }
    #endregion

    #region Icmp Echo Reply
    [StructLayout(LayoutKind.Sequential)]
    internal struct IcmpEchoReply
    {
        internal uint address;
        internal uint status;
        internal uint roundTripTime;
        internal ushort dataSize;
        internal ushort reserved;
        internal IntPtr data;
        internal IPOptions options;
    }
    #endregion


}
