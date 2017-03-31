// In The Hand - .NET Components for Mobility
//
// InTheHand.Security.Cryptography.ProtectedData
// 
// Copyright (c) 2011-12 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace InTheHand.Security.Cryptography
{
    /// <summary>
    /// Provides methods for protecting and unprotecting data.
    /// </summary>
    /// <remarks>This class provides access to the Data Protection API (DPAPI).
    /// This is a service that is provided by the operating system and does not require additional libraries.
    /// It provides protection using the user or machine credentials to encrypt or decrypt data.
    /// <para>The class consists of two wrappers for the unmanaged DPAPI, <see cref="Protect"/> and <see cref="Unprotect"/>.
    /// These two methods can be used to encrypt and decrypt data such as passwords, keys, and connection strings.</para>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile 2003 and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
    /// </list></remarks>
    public static class ProtectedData
    {
        /// <summary>
        /// Encrypts the data in a specified byte array and returns a byte array that contains the encrypted data.
        /// </summary>
        /// <param name="userData">A byte array that contains data to encrypt.</param>
        /// <param name="optionalEntropy">An optional additional byte array used to increase the complexity of the encryption, or Nothing for no additional complexity.</param>
        /// <returns>A byte array representing the encrypted data.</returns>
        /// <remarks>This method can be used to encrypt data such as passwords, keys, or connection strings.
        /// The optionalEntropy parameter enables you to add data to increase the complexity of the encryption; specify Nothing for no additional complexity.
        /// If provided, this information must also be used when decrypting the data using the <see cref="Unprotect"/> method.</remarks>
        /// <exception cref="ArgumentNullException">The userData parameter is Nothing.</exception>
        /// <exception cref="CryptographicException">The encryption failed.</exception>
        /// <exception cref="OutOfMemoryException">The system ran out of memory while encrypting the data.</exception>
        public static byte[] Protect(byte[] userData, byte[] optionalEntropy)
        {
            if (userData == null)
            {
                throw new ArgumentNullException("userData");
            }

            GCHandle hIn = System.Runtime.InteropServices.GCHandle.Alloc(userData, GCHandleType.Pinned);
            GCHandle hEntropy = (optionalEntropy == null) ? GCHandle.Alloc(0, GCHandleType.Pinned) :GCHandle.Alloc(optionalEntropy, GCHandleType.Pinned);
            byte[] result = null;
            try
            {
                NativeMethods.DATA_BLOB inBlob = new NativeMethods.DATA_BLOB();
                inBlob.cbData = userData.Length;
                inBlob.pbData = hIn.AddrOfPinnedObject();
                NativeMethods.DATA_BLOB entropyBlob = new NativeMethods.DATA_BLOB();
                if (optionalEntropy != null)
                {
                    entropyBlob.cbData = optionalEntropy.Length;
                    entropyBlob.pbData = hEntropy.AddrOfPinnedObject();
                }

                NativeMethods.DATA_BLOB outBlob = new NativeMethods.DATA_BLOB();
                bool success = NativeMethods.CryptProtectData(ref inBlob, "", ref entropyBlob, IntPtr.Zero, IntPtr.Zero, 0, ref outBlob);

                if (success)
                {
                    result = new byte[outBlob.cbData];
                    Marshal.Copy(outBlob.pbData, result, 0, result.Length);
                    Marshal.FreeHGlobal(outBlob.pbData);
                }
                else
                {
                    if (Marshal.GetLastWin32Error() == 14)
                    {
                        throw new OutOfMemoryException();
                    }
                    else
                    {
                        throw new CryptographicException(Marshal.GetLastWin32Error());
                    }
                }
            }
            finally
            {
                if (hIn.IsAllocated)
                {
                    hIn.Free();
                }

                if (hEntropy.IsAllocated)
                {
                    hEntropy.Free();
                }
            }

            return result;
        }

        /// <summary>
        /// Decrypts the data in a specified byte array and returns a byte array that contains the decrypted data.
        /// </summary>
        /// <param name="encryptedData">A byte array containing data encrypted using the <see cref="Protect"/> method.</param>
        /// <param name="optionalEntropy">An optional additional byte array that was used to encrypt the data, or Nothing if the additional byte array was not used.</param>
        /// <returns>A byte array representing the decrypted data.</returns>
        /// <remarks>This method can be used to unprotect data that was encrypted using the <see cref="Protect"/> method.
        /// If the optionalEntropy parameter was used during encryption, it must be supplied to unencrypt the data.</remarks>
        /// <exception cref="ArgumentNullException">The encryptedData parameter is Nothing.</exception>
        /// <exception cref="CryptographicException">The decryption failed.</exception>
        /// <exception cref="OutOfMemoryException">The system ran out of memory while decrypting the data.</exception>
        public static byte[] Unprotect(byte[] encryptedData, byte[] optionalEntropy)
        {
            if (encryptedData == null)
            {
                throw new ArgumentNullException("encryptedData");
            }

            GCHandle hIn = System.Runtime.InteropServices.GCHandle.Alloc(encryptedData, GCHandleType.Pinned);
            GCHandle hEntropy = (optionalEntropy == null) ? GCHandle.Alloc(0, GCHandleType.Pinned) : GCHandle.Alloc(optionalEntropy, GCHandleType.Pinned);
            byte[] result = null;
            try
            {
                NativeMethods.DATA_BLOB inBlob = new NativeMethods.DATA_BLOB();
                inBlob.cbData = encryptedData.Length;
                inBlob.pbData = hIn.AddrOfPinnedObject();
                NativeMethods.DATA_BLOB entropyBlob = new NativeMethods.DATA_BLOB();

                if (optionalEntropy != null)
                {
                    entropyBlob.cbData = optionalEntropy.Length;
                    entropyBlob.pbData = hEntropy.AddrOfPinnedObject();
                }

                NativeMethods.DATA_BLOB outBlob = new NativeMethods.DATA_BLOB();
                bool success = NativeMethods.CryptUnprotectData(ref inBlob, "", ref entropyBlob, IntPtr.Zero, IntPtr.Zero, 0, ref outBlob);

                if (success)
                {
                    result = new byte[outBlob.cbData];
                    Marshal.Copy(outBlob.pbData, result, 0, result.Length);
                    Marshal.FreeHGlobal(outBlob.pbData);
                }
                else
                {
                    if (Marshal.GetLastWin32Error() == 14)
                    {
                        throw new OutOfMemoryException();
                    }
                    else
                    {
                        throw new CryptographicException(Marshal.GetLastWin32Error());
                    }
                }
            }
            finally
            {
                if (hIn.IsAllocated)
                {
                    hIn.Free();
                }

                if (hEntropy.IsAllocated)
                {
                    hEntropy.Free();
                }
            }
            return result;
        }

        private static class NativeMethods
        {
            [DllImport("coredll", SetLastError=true)]
            [return:MarshalAs(UnmanagedType.Bool)]
            internal static extern bool CryptProtectData(ref DATA_BLOB dataIn, 
                string dataDescr, ref DATA_BLOB optionalEntropy, IntPtr reserved,
                IntPtr promptStruct, uint flags, ref DATA_BLOB dataOut);

            [DllImport("coredll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool CryptUnprotectData(ref DATA_BLOB dataIn, 
                string dataDescr, ref DATA_BLOB optionalEntropy, 
                IntPtr reserved, IntPtr promptStruct,
                uint flags, ref DATA_BLOB dataOut);

            [StructLayout(LayoutKind.Sequential)]
            internal struct DATA_BLOB
            {
                public int cbData;
                public IntPtr pbData;
            }
        }
    }
}