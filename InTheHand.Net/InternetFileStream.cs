// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternetFileStream.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace InTheHand.Net
{
    internal class InternetFileStream : Stream
    {
        private IntPtr handle;
        private FtpWebResponse response;
        private bool write;
        private long position = 0;

        internal InternetFileStream(IntPtr handle, FtpWebResponse response, bool write)
        {
            this.handle = handle;
            this.response = response;
            this.write = write;
        }

        public override bool CanRead
        {
            get { return !write; }
        }
        public override bool CanSeek
        {
            get { return false; }
        }
        public override bool CanWrite
        {
            get { return write; }
        }

        public override void Close()
        {
            if (handle != IntPtr.Zero)
            {
                bool success = NativeMethods.InternetCloseHandle(handle);

                if (!success)
                {
                    NativeMethods.ThrowException(response);
                }
                handle = IntPtr.Zero;
            }
            base.Close();
        }

        public override long Length
        {
            get 
            {
                if ((this.handle != IntPtr.Zero) && this.CanRead)
                {
                    int bytesAvailable;
                    bool success = NativeMethods.InternetQueryDataAvailable(handle, out bytesAvailable, 0, 0);
                    if (!success)
                    {
                        NativeMethods.ThrowException(response);
                    }
                    return bytesAvailable;
                }
                return 0;
            }
        }

        public override long Position
        {
            get
            {
                return position;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public override void Flush()
        {
            
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this.CanRead)
            {
                byte[] rawBuffer = new byte[count];
                int bytesRead;

                bool success = NativeMethods.InternetReadFile(handle, rawBuffer, count, out bytesRead);

                if (!success)
                {
                    NativeMethods.ThrowException(response);
                }
                position += bytesRead;

                Buffer.BlockCopy(rawBuffer, 0, buffer, offset, bytesRead);

                return bytesRead;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (this.CanWrite)
            {
                byte[] rawBuffer = new byte[count];
                Buffer.BlockCopy(buffer, offset, rawBuffer, 0, count);
                uint bytesWritten = 0;
                bool success = false;

                try
                {
                    success = NativeMethods.InternetWriteFile(handle, rawBuffer, (uint)count, ref bytesWritten);
                }
                catch
                {
                }
                position += bytesWritten;

                if (!success)
                {
                    NativeMethods.ThrowException(response);
                }
            }
            else
            {
                throw new NotSupportedException();
            }
        }
        
    }
}
