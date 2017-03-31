// In The Hand - .NET Components for Mobility
//
// InTheHand.Text.Encoding
// 
// Copyright (c) 2010-2014 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace InTheHand.Text
{
    /// <summary>
    /// Provides extension methods for <see cref="System.Text.Encoding"/>.
    /// </summary>
    public static class EncodingInTheHand
    {
        /// <summary>
        /// Decodes all the bytes in the specified byte array into a string
        /// </summary>
        /// <param name="e">The <see cref="Encoding"/>.</param>
        /// <param name="bytes">The byte array containing the sequence of bytes to decode.</param>
        /// <returns>A <see cref="String"/> containing the results of decoding the specified sequence of bytes.</returns>
        public static string GetString(this Encoding e, byte[] bytes)
        {
            return e.GetString(bytes, 0, bytes.Length);
        }
    }
}
