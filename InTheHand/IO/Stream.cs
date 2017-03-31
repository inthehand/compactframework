// In The Hand - .NET Components for Mobility
//
// InTheHand.IO.Stream
// 
// Copyright (c) 2010-2014 In The Hand Ltd, All rights reserved.

using System;

namespace InTheHand.IO
{
    /// <summary>
    /// Provides helper methods for the <see cref="System.IO.Stream"/> class.
    /// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Pocket PC 2003, Windows Mobile Version 5.0 and later</description></item>
    /// <item><term>Windows Embedded</term><description>Windows CE .NET 4.1 and later</description></item>
    /// <item><term>Windows Phone</term><description>Windows Phone 7</description></item>
    /// </list></remarks>
    /// <seealso cref="System.IO.Stream"/>
    public static class StreamInTheHand
    {
        #region Copy To
        /// <summary>
        /// Reads all the bytes from the current stream and writes them to a destination stream.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="destination">The stream that will contain the contents of the current stream.</param>
        /// <remarks>Copying begins at the current position in the current stream.</remarks>
        /// <exception cref="ArgumentNullException">destination is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="NotSupportedException">The current stream does not support reading.
        /// <para>-or-</para>
        /// destination does not support writing.</exception>
        public static void CopyTo(this System.IO.Stream s, System.IO.Stream destination)
        {
            CopyTo(s, destination, 4096);
        }

        /// <summary>
        /// Reads all the bytes from the current stream and writes them to a destination stream, using a specified buffer size.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="destination">The stream that will contain the contents of the current stream.</param>
        /// <param name="bufferSize">The size of the buffer. 
        /// This value must be greater than zero. 
        /// The default size is 4096.</param>
        /// <remarks>Copying begins at the current position in the current stream.</remarks>
        /// <exception cref="ArgumentNullException">destination is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentOutOfRangeException">bufferSize is negative or zero.</exception>
        /// <exception cref="NotSupportedException">The current stream does not support reading.
        /// <para>-or-</para>
        /// destination does not support writing.</exception>
        public static void CopyTo(this System.IO.Stream s, System.IO.Stream destination, int bufferSize)
        {
            if (destination == null)
            {
                throw new ArgumentNullException("destination");
            }

            if (bufferSize < 1)
            {
                throw new ArgumentOutOfRangeException("bufferSize");
            }

            if (!s.CanRead || !destination.CanWrite)
            {
                throw new NotSupportedException();
            }

            byte[] buffer = new byte[bufferSize];
            int bytesRead = int.MaxValue;
            while (bytesRead > 0)
            {
                bytesRead = s.Read(buffer, 0, bufferSize);
                if (bytesRead > 0)
                {
                    destination.Write(buffer, 0, bytesRead);
                }
            }
        }
        #endregion
    }
}