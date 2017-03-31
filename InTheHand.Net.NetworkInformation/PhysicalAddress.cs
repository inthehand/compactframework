// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.NetworkInformation.PhysicalAddress
// 
// Copyright (c) 2003-2010 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Net;
using System.Text;

namespace InTheHand.Net.NetworkInformation
{
    /// <summary>
    /// Provides the Media Access Control (MAC) address for a network interface (adapter).  
    /// </summary>
    /// <remarks><para>Equivalent to System.Net.NetworkInformation.PhysicalAddress</para>
    /// The MAC address, or physical address, is a hardware address that uniquely identifies each node, such as a computer or printer, on a network.
    /// Instances of this class are returned by the <see cref="NetworkInterface.GetPhysicalAddress"/> method.</remarks>
    public class PhysicalAddress
    {
        private byte[] address;
        private bool changed = true;
        private int hash;

        /// <summary>
        /// Returns a new <see cref="PhysicalAddress"/> instance with a zero length address.
        /// </summary>
        /// <remarks>The Parse method returns None if you specify a null reference (Nothing in Visual Basic) address.</remarks>
        public static readonly PhysicalAddress None = new PhysicalAddress(new byte[0]);

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicalAddress"/> class.
        /// </summary>
        /// <param name="address">A Byte array containing the address.</param>
        /// <remarks>In common scenarios, applications do not need to call this constructor; instances of this class are returned by the <see cref="NetworkInterface.GetPhysicalAddress()"/> method.
        /// Note that you can also use the <see cref="Parse"/> method to create a new instance of <see cref="PhysicalAddress"/>.</remarks>
        public PhysicalAddress(byte[] address)
        {
            this.address = address;
        }
        /// <summary>
        /// Compares two <see cref="PhysicalAddress"/> instances.
        /// </summary>
        /// <param name="comparand"></param>
        /// <returns></returns>
        public override bool Equals(object comparand)
        {
            PhysicalAddress address = (PhysicalAddress)comparand;
            if (this.address.Length != address.address.Length)
            {
                return false;
            }

            for (int i = 0; i < address.address.Length; i++)
            {
                if (this.address[i] != address.address[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns the address of the current instance.
        /// </summary>
        /// <returns>A <see cref="Byte"/> array containing the address.</returns>
        public byte[] GetAddressBytes()
        {
            byte[] dst = new byte[this.address.Length];
            Buffer.BlockCopy(this.address, 0, dst, 0, this.address.Length);
            return dst;
        }

        /// <summary>
        /// Returns the hash value of a physical address.
        /// </summary>
        /// <returns>An integer hash value.</returns>
        public override int GetHashCode()
        {
            if (this.changed)
            {
                this.changed = false;
                this.hash = 0;
                int num2 = this.address.Length & -4;
                int index = 0;
                while (index < num2)
                {
                    this.hash ^= ((this.address[index] | (this.address[index + 1] << 8)) | (this.address[index + 2] << 0x10)) | (this.address[index + 3] << 0x18);
                    index += 4;
                }

                if ((this.address.Length & 3) != 0)
                {
                    int num3 = 0;
                    int num4 = 0;
                    while (index < this.address.Length)
                    {
                        num3 |= this.address[index] << num4;
                        num4 += 8;
                        index++;
                    }

                    this.hash ^= num3;
                }
            }

            return this.hash;
        }

        /// <summary>
        /// Parses the specified <see cref="String"/> and stores its contents as the address bytes of the <see cref="PhysicalAddress"/> returned by this method.
        /// </summary>
        /// <param name="address">A <see cref="String"/> containing the address that will be used to initialize the <see cref="PhysicalAddress"/> instance returned by this method.</param>
        /// <returns>A <see cref="PhysicalAddress"/> instance with the specified address.</returns>
        /// <remarks>Use the <see cref="GetAddressBytes"/> method to retrieve the address from an existing <see cref="PhysicalAddress"/> instance.</remarks>
        public static PhysicalAddress Parse(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                return PhysicalAddress.None;
            }

            int num = 0;
            bool flag = false;
            byte[] buffer = null;
            if (address == null)
            {
                return None;
            }

            if (address.IndexOf('-') >= 0)
            {
                flag = true;
                buffer = new byte[(address.Length + 1) / 3];
            }
            else
            {
                if ((address.Length % 2) > 0)
                {
                    throw new FormatException(Properties.Resources.net_bad_mac_address);
                }
                buffer = new byte[address.Length / 2];
            }

            int index = 0;
            for (int i = 0; i < address.Length; i++)
            {
                int num4 = address[i];
                if ((num4 >= 0x30) && (num4 <= 0x39))
                {
                    num4 -= 0x30;
                }
                else if ((num4 >= 0x41) && (num4 <= 70))
                {
                    num4 -= 0x37;
                }
                else
                {
                    if (num4 != 0x2d)
                    {
                        throw new FormatException(Properties.Resources.net_bad_mac_address);
                    }
                    if (num != 2)
                    {
                        throw new FormatException(Properties.Resources.net_bad_mac_address);
                    }
                    num = 0;
                    continue;
                }

                if (flag && (num >= 2))
                {
                    throw new FormatException(Properties.Resources.net_bad_mac_address);
                }

                if ((num % 2) == 0)
                {
                    buffer[index] = (byte)(num4 << 4);
                }
                else
                {
                    buffer[index++] = (byte)(buffer[index++] | ((byte)num4));
                }

                num++;
            }

            if (num < 2)
            {
                throw new FormatException(Properties.Resources.net_bad_mac_address);
            }

            return new PhysicalAddress(buffer);
        }

        /// <summary>
        /// Returns the <see cref="String"/> representation of the address of this instance.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (byte num in this.address)
            {
                int num2 = (num >> 4) & 15;
                for (int i = 0; i < 2; i++)
                {
                    if (num2 < 10)
                    {
                        builder.Append((char)(num2 + 0x30));
                    }
                    else
                    {
                        builder.Append((char)(num2 + 0x37));
                    }

                    num2 = num & 15;
                }
            }

            return builder.ToString();
        }
    }
}