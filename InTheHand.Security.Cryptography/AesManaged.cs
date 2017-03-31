// In The Hand - .NET Components for Mobility
//
// InTheHand.Security.Cryptography.AesManaged
// 
// Copyright (c) 2010-12 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace InTheHand.Security.Cryptography
{
    /// <summary>
    /// Represents the abstract base class from which all implementations of the Advanced Encryption Standard (AES) must inherit.
    /// </summary>
    public abstract class Aes : SymmetricAlgorithm
    {
        /// <summary>
        /// Creates a cryptographic object that is used to perform the symmetric algorithm.
        /// </summary>
        /// <returns>A cryptographic object that is used to perform the symmetric algorithm.</returns>
        public static new Aes Create()
        {
            return new AesManaged();
        }
    }

    /// <summary>
    /// Provides a managed implementation of the Advanced Encryption Standard (AES) symmetric algorithm.
    /// </summary>
    public sealed class AesManaged : Aes
    {
        private RijndaelManaged rm;

        /// <summary>
        /// Provides a managed implementation of the Advanced Encryption Standard (AES) symmetric algorithm.
        /// </summary>
        public AesManaged()
        {
            rm = new RijndaelManaged();
        }

        /// <summary>
        /// Creates a symmetric decryptor object using the current key and initialization vector (IV).
        /// </summary>
        /// <returns></returns>
        public override ICryptoTransform CreateDecryptor()
        {
            return rm.CreateDecryptor();
        }

        /// <summary>
        /// Creates a symmetric decryptor object using the specified key and initialization vector (IV).
        /// </summary>
        /// <param name="rgbKey"></param>
        /// <param name="rgbIV"></param>
        /// <returns></returns>
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return rm.CreateDecryptor(rgbKey, rgbIV);
        }

        /// <summary>
        /// Creates a symmetric encryptor object using the current key and initialization vector (IV).
        /// </summary>
        /// <returns></returns>
        public override ICryptoTransform CreateEncryptor()
        {
            return rm.CreateEncryptor();
        }

        /// <summary>
        /// Creates a symmetric encryptor object using the specified key and initialization vector (IV).
        /// </summary>
        /// <param name="rgbKey"></param>
        /// <param name="rgbIV"></param>
        /// <returns></returns>
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return rm.CreateEncryptor(rgbKey, rgbIV);
        }

        /// <summary>
        /// Gets or sets the number of bits to use as feedback. 
        /// </summary>
        /// <value>The feedback size, in bits.</value>
        /// <remarks>The maximum feedback size is 128 bits.
        /// <para>Because this algorithm does not support feedback modes, using this property is discouraged.</para></remarks>
        public override int FeedbackSize
        {
            get
            {
                return rm.FeedbackSize;
            }

            set
            {
                if (value > 128)
                {
                    throw new ArgumentOutOfRangeException();
                }

                rm.FeedbackSize = value;
            }
        }

        /// <summary>
        /// Generates a random initialization vector (IV) to use for the symmetric algorithm.
        /// </summary>
        public override void GenerateIV()
        {
            rm.GenerateIV();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void GenerateKey()
        {
            rm.GenerateKey();
        }

        /// <summary>
        /// Gets or sets the initialization vector (IV) to use for the symmetric algorithm.
        /// </summary>
        /// <value>The initialization vector to use for the symmetric algorithm</value>
        public override byte[] IV
        {
            get
            {
                return rm.IV;
            }

            set
            {
                rm.IV = value;
            }
        }

        /// <summary>
        /// Gets or sets the secret key used for the symmetric algorithm.
        /// </summary>
        /// <value>The key for the symmetric algorithm.</value>
        public override byte[] Key
        {
            get
            {
                return rm.Key;
            }

            set
            {
                if (value.Length > 32)
                {
                    throw new ArgumentOutOfRangeException();
                }
                rm.Key = value;
            }
        }

        /// <summary>
        /// Gets or sets the size, in bits, of the secret key used for the symmetric algorithm. 
        /// </summary>
        /// <value>The size, in bits, of the key used by the symmetric algorithm.</value>
        /// <remarks>The maximum size of the key is 256 bits.</remarks>
        public override int KeySize
        {
            get
            {
                return rm.KeySize;
            }

            set
            {
                if (value < 1 || value > 256)
                {
                    throw new ArgumentOutOfRangeException();
                }
                rm.KeySize = value;
            }
        }

        /// <summary>
        /// Gets the block sizes, in bits, that are supported by the symmetric algorithm.
        /// </summary>
        /// <value>An array that contains the block sizes supported by the algorithm.</value>
        /// <remarks>The symmetric algorithm supports only block sizes that match an entry in this array.</remarks>
        public override KeySizes[] LegalBlockSizes
        {
            get
            {
                return new KeySizes[] { new KeySizes(0x80,0x80,0)};
            }
        }

        /// <summary>
        /// Gets or sets the mode for operation of the symmetric algorithm.
        /// </summary>
        /// <value>One of the CipherMode values. 
        /// The default is CBC.</value>
        /// <remarks>The CFB and OFB modes are not supported.</remarks>
        /// <exception cref="CryptographicException">Mode is set to CFB or OFB.</exception>
        public override CipherMode Mode
        {
            get
            {
                return rm.Mode;
            }

            set
            {
                switch (value)
                {
                    case CipherMode.CFB:
                    case CipherMode.OFB:
                        throw new CryptographicException(Properties.Resources.Cryptography_InvalidCipherMode);
                }
                rm.Mode = value;
            }
        }

        /// <summary>
        /// Gets or sets the padding mode used in the symmetric algorithm.
        /// </summary>
        /// <value>One of the PaddingMode values. 
        /// The default is PKCS7.</value>
        public override PaddingMode Padding
        {
            get
            {
                return rm.Padding;
            }

            set
            {
                rm.Padding = value;
            }
        }
    }
}