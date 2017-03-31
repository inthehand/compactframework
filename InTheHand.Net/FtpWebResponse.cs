// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FtpWebResponse.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net;
using System.Text;

namespace InTheHand.Net
{
    /// <summary>
    /// Encapsulates a File Transfer Protocol (FTP) server's response to a request.
    /// </summary>
    /// <remarks>Instances of <see cref="FtpWebResponse"/> are obtained by calling the <see cref="FtpWebRequest.GetResponse"/> method.
    /// The returned object must be cast to an <see cref="FtpWebResponse"/>. 
    /// When your application no longer needs the <see cref="FtpWebResponse"/> object, call the <see cref="Close"/> method to free the resources held by the <see cref="FtpWebResponse"/>.
    /// <para>The <see cref="StatusCode"/> property contains the status code returned by the server, and the <see cref="StatusDescription"/> property returns the status code and a message that describes the status. 
    /// The values returned by these properties change as the messages are returned by the server.</para>
    /// <para>Any data returned by the request, such as the list of file names returned for a <see cref="WebRequestMethods.Ftp.ListDirectory"/> request, is available in the stream returned by the <see cref="GetResponseStream"/> method.
    /// The length of the stream data can be obtained from the <see cref="ContentLength"/> property.</para></remarks>
    public class FtpWebResponse : WebResponse
    {
        private FtpWebRequest request;
        private Ftp f;
        private InternetFileStream stream;

        internal FtpWebResponse(FtpWebRequest request)
        {
            bool success = false;

            this.request = request;
            
            IntPtr hFile = IntPtr.Zero;
            NetworkCredential credentials = null;

            if (request.Credentials != null)
            {
                credentials = (NetworkCredential)request.Credentials;
            }
            else
            {
                //parse user info in the uri
                string userInfo = request.RequestUri.UserInfo;
                if (!string.IsNullOrEmpty(userInfo))
                {
                    //username and password
                    if (userInfo.IndexOf(':') > -1)
                    {
                        string[] userParts = userInfo.Split(':');
                        credentials = new NetworkCredential(userParts[0], userParts[1]);
                    }
                    else
                    {
                        //username only
                        credentials = new NetworkCredential(userInfo, string.Empty);
                    }

                    request.Credentials = credentials;
                }
            }


                f = new Ftp(request.RequestUri, credentials, request.UsePassive);

                GetStatus();


            /*// Set timeout if not default
            if (request.Timeout != System.Threading.Timeout.Infinite)
            {
                int timeout = request.Timeout;
                bool timeoutSet = NativeMethods.InternetSetOption(f.Handle, NativeMethods.INTERNET_OPTION_CONNECT_TIMEOUT, ref timeout, 4);
            }*/

            //set default response uri
            if (request.RequestUri.UserInfo != string.Empty)
            {
                StringBuilder cleanUri = new StringBuilder();
                cleanUri.Append(request.RequestUri.Scheme);

                for (int i = 2; i < request.RequestUri.Segments.Length; i++)
                {
                    cleanUri.Append(request.RequestUri.Segments[i]);
                }
                responseUri = new Uri(cleanUri.ToString());
            }
            else
            {
                responseUri = request.RequestUri;
            }

            switch (request.Method)
            {
                case WebRequestMethods.Ftp.DeleteFile:
                    success = NativeMethods.pFtpDeleteFile(f.Handle, request.RequestUri.LocalPath);
                    break;

                case WebRequestMethods.Ftp.DownloadFile:
                    //resolved issue with caching
                    hFile = NativeMethods.pFtpOpenFile(f.Handle, request.RequestUri.LocalPath, NativeMethods.GENERIC_READ, (request.UseBinary ? NativeMethods.FTP_TRANSFER_TYPE_BINARY : NativeMethods.FTP_TRANSFER_TYPE_ASCII) | NativeMethods.INTERNET_FLAG_RELOAD, this.GetHashCode());

                    GetStatus();

                    if (hFile == IntPtr.Zero)
                    {
                        NativeMethods.ThrowException(this);
                    }
                    stream = new InternetFileStream(hFile, this, false);
                    contentLength = -1;
                    success = true;
                    break;

                case WebRequestMethods.Ftp.PrintWorkingDirectory:
                    StringBuilder workingDirectoryBuffer = new StringBuilder(260);
                    int len = workingDirectoryBuffer.Capacity;
                    success = NativeMethods.pFtpGetCurrentDirectory(f.Handle, workingDirectoryBuffer, ref len);
                    this.responseUri = new Uri(new Uri(this.request.RequestUri.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped)), workingDirectoryBuffer.ToString());
                    break;
                case WebRequestMethods.Ftp.MakeDirectory:
                    success = NativeMethods.pFtpCreateDirectory(f.Handle, request.RequestUri.LocalPath);
                    break;

                case WebRequestMethods.Ftp.Rename:
                    success = NativeMethods.pFtpRenameFile(f.Handle, request.RequestUri.LocalPath, request.RenameTo);
                    break;

                case WebRequestMethods.Ftp.UploadFile:
                    hFile = NativeMethods.pFtpOpenFile(f.Handle, request.RequestUri.LocalPath, NativeMethods.GENERIC_WRITE, request.UseBinary ? NativeMethods.FTP_TRANSFER_TYPE_BINARY : NativeMethods.FTP_TRANSFER_TYPE_ASCII, this.GetHashCode());

                    GetStatus();

                    if (hFile == IntPtr.Zero)
                    {
                        NativeMethods.ThrowException(this);
                    }
                    InternetFileStream uploadStream = new InternetFileStream(hFile, this, true);
                    byte[] uploadBuffer = ((MemoryStream)request.GetRequestStream()).ToArray();
                    uploadStream.Write(uploadBuffer, 0, uploadBuffer.Length);
                    uploadStream.Close();
                    success = true;
                    break;

                case WebRequestMethods.Ftp.ListDirectory:
                case WebRequestMethods.Ftp.ListDirectoryDetails:
                    success = NativeMethods.pFtpCommand(f.Handle, true, /*request.UseBinary ? NativeMethods.FTP_TRANSFER_TYPE_BINARY :*/ NativeMethods.FTP_TRANSFER_TYPE_ASCII, request.Method + " " + request.RequestUri.LocalPath + "\r\n"/*command.ToString()*/, 0, out hFile);

                    GetStatus();

                    if (!success)
                    {
                        NativeMethods.ThrowException(this);
                    }
                    stream = new InternetFileStream(hFile, this, false);
                    contentLength = stream.Length;

                    break;

                default:
                    //separate path and file name
                    string[] bits = SplitLocalPath(request.RequestUri.LocalPath);

                    //create command string
                    StringBuilder command = new StringBuilder();
                    command.Append(request.Method);
             
                    if (!string.IsNullOrEmpty(bits[1]))
                    {
                        command.Append(" ");
                        //if have a filename send it (separated with a space)
                        command.Append(bits[1]);
                    }
                    //add return
                    command.Append("\r\n");

                    if (!string.IsNullOrEmpty(bits[0]))
                    {
                        //only change folder if not the root
                        if (bits[0] != "\\")
                        {
                            //change path if required
                            success = NativeMethods.pFtpSetCurrentDirectory(f.Handle, bits[0]);
                            //success = NativeMethods.FtpCommand(f.Handle, false, request.UseBinary ? NativeMethods.FTP_TRANSFER_TYPE_BINARY : NativeMethods.FTP_TRANSFER_TYPE_ASCII, "CWD " + bits[0] + "\r\n", 0, out hFile);
                        }
                    }

                    //switch on the remaining methods
                    switch (request.Method)
                    {
                        case WebRequestMethods.Ftp.AppendFile:
                            success = NativeMethods.pFtpCommand(f.Handle, true, request.UseBinary ? NativeMethods.FTP_TRANSFER_TYPE_BINARY : NativeMethods.FTP_TRANSFER_TYPE_ASCII, command.ToString(), 0, out hFile);

                            GetStatus();

                            if (!success)
                            {
                                NativeMethods.ThrowException(this);
                            }
                            
                            InternetFileStream appendStream = new InternetFileStream(hFile, this, true);
                            byte[] appendBuffer = ((MemoryStream)request.GetRequestStream()).ToArray();
                            appendStream.Write(appendBuffer, 0, appendBuffer.Length);
                            appendStream.Close();
                            break;

                        case WebRequestMethods.Ftp.GetDateTimestamp:
                            success = NativeMethods.pFtpCommand(f.Handle, false, /*request.UseBinary ? NativeMethods.FTP_TRANSFER_TYPE_BINARY :*/ NativeMethods.FTP_TRANSFER_TYPE_ASCII, command.ToString(), 0, out hFile);
                            GetStatus();
                            int timeSpaceIndex = this.statusDescription.LastIndexOf(' ');
                            if (timeSpaceIndex > -1)
                            {
                                string timestamp = this.statusDescription.Substring(timeSpaceIndex + 1, (this.statusDescription.Length - timeSpaceIndex) - 1);
                                try
                                {
                                    this.lastModified = DateTime.ParseExact(timestamp, "yyyyMMddHHmmss", null);
                                    this.lastModified = DateTime.SpecifyKind(this.lastModified, DateTimeKind.Utc);
                                }
                                catch
                                {
                                    success = false;
                                }
                            }
                            break;

                        case WebRequestMethods.Ftp.GetFileSize:
                            success = NativeMethods.pFtpCommand(f.Handle, false, /*request.UseBinary ? NativeMethods.FTP_TRANSFER_TYPE_BINARY :*/ NativeMethods.FTP_TRANSFER_TYPE_ASCII, command.ToString(), 0, out hFile);
                            GetStatus();
                            int sizeSpaceIndex = this.statusDescription.LastIndexOf(' ');
                            if (sizeSpaceIndex > -1)
                            {
                                string size = this.statusDescription.Substring(sizeSpaceIndex + 1, (this.statusDescription.Length - sizeSpaceIndex) - 1);
                                try
                                {
                                    this.contentLength = long.Parse(size);
                                }
                                catch
                                {
                                    success = false;
                                }
                            }
                            break;

                        case WebRequestMethods.Ftp.UploadFileWithUniqueName:
                            success = NativeMethods.pFtpCommand(f.Handle, false, request.UseBinary ? NativeMethods.FTP_TRANSFER_TYPE_BINARY : NativeMethods.FTP_TRANSFER_TYPE_ASCII, command.ToString(), this.GetHashCode(), out hFile);

                            GetStatus();

                            try
                            {
                                if (!success)
                                {
                                    NativeMethods.ThrowException(this);
                                }
                            }
                            finally
                            {
                                f.Close();
                            }

                            f = new Ftp(request.RequestUri, credentials, request.UsePassive);

                            //get unique filename from response
                            int spaceIndex = this.statusDescription.LastIndexOf(' ');
                            if (spaceIndex > -1)
                            {
                                string filename = this.statusDescription.Substring(spaceIndex + 1, (statusDescription.Length - spaceIndex) - 1);
                                filename = filename.TrimEnd('.');
                                responseUri = new Uri("ftp://" + request.RequestUri.Host + "/" + bits[0] + filename);
                            }
                            //open file
                            hFile = NativeMethods.pFtpOpenFile(f.Handle, responseUri.LocalPath, NativeMethods.GENERIC_WRITE, request.UseBinary ? NativeMethods.FTP_TRANSFER_TYPE_BINARY : NativeMethods.FTP_TRANSFER_TYPE_ASCII, 0);

                            GetStatus();


                            if (hFile == IntPtr.Zero)
                            {
                                NativeMethods.ThrowException(this);
                            }
                            InternetFileStream uniqueStream = new InternetFileStream(hFile, this, true);
                            byte[] uniqueBuffer = ((MemoryStream)request.GetRequestStream()).ToArray();
                            uniqueStream.Write(uniqueBuffer, 0, uniqueBuffer.Length);
                            uniqueStream.Close();
                            success = true;
                            break;

                        default:
                            success = NativeMethods.pFtpCommand(f.Handle, false, request.UseBinary ? NativeMethods.FTP_TRANSFER_TYPE_BINARY : NativeMethods.FTP_TRANSFER_TYPE_ASCII, request.Method + "\r\n", 0, out hFile);
                            break;
                    }
                    break;
            }

            GetStatus();
            
            if (!success)
            {
                NativeMethods.ThrowException(this);   
            }
        }

        #region Get Status
        private void GetStatus()
        {
            NativeMethods.ERROR_INTERNET error;

            statusDescription = Ftp.GetLastResponseInfo(out error);

            string[] pieces = statusDescription.Split(' ');
            if (pieces.Length > 0)
            {
                try
                {
                    statusCode = (FtpStatusCode)int.Parse(pieces[0]);
                }
                catch
                {
                }
            }
        }
        #endregion

        #region SplitPath
        private static string[] SplitLocalPath(string localPath)
        {
            if (localPath.Length > 1)
            {
                int slashIndex = localPath.LastIndexOf('/');
                int periodIndex = localPath.LastIndexOf('.');


                if (periodIndex > -1)
                {
                    string path = localPath.Substring(0, slashIndex);
                    string file = localPath.Substring(slashIndex + 1, (localPath.Length - slashIndex) - 1);
                    return new string[] { path, file };
                }
            }
            return new string[] { localPath, string.Empty };
        }
        #endregion

        #region Status Code
        private FtpStatusCode statusCode = FtpStatusCode.Undefined;
        /// <summary>
        /// Gets the most recent status code sent from the FTP server.
        /// </summary>
        public FtpStatusCode StatusCode 
        {
            get
            {
                return statusCode;
            }
        }
        #endregion

        #region Status Description
        private string statusDescription;
        /// <summary>
        /// Gets text that describes a status code sent from the FTP server.
        /// </summary>
        /// <value>A <see cref="String"/> instance that contains the status code and message returned with this response.</value>
        public string StatusDescription
        {
            get
            {
                return statusDescription;
            }
        }
        #endregion

        #region Content Length
        private long contentLength = 0;
        /// <summary>
        /// Gets the length of the data received from the FTP server.
        /// </summary>
        public override long ContentLength
        {
            get
            {
                return contentLength;
            }
        }
        #endregion

        private DateTime lastModified;
        /// <summary>
        /// Gets the date and time that a file on an FTP server was last modified.
        /// </summary>
        public DateTime LastModified
        {
            get
            {
                return lastModified;
            }
        }

        /// <summary>
        /// Retrieves the stream that contains response data sent from an FTP server.
        /// </summary>
        /// <returns>A readable <see cref="Stream"/> instance that contains data returned with the response; otherwise, Null if no response data was returned by the server.</returns>
        public override Stream GetResponseStream()
        {
            if ((request.Method == WebRequestMethods.Ftp.DownloadFile) | (request.Method == WebRequestMethods.Ftp.ListDirectory) | (request.Method == WebRequestMethods.Ftp.ListDirectoryDetails))
            {
                return stream;
            }
            //operation doesn't return data stream
            throw new InvalidOperationException();
        }

        private Uri responseUri;
        /// <summary>
        /// Gets the URI that sent the response to the request.
        /// </summary>
        public override Uri ResponseUri
        {
            get
            {
                return responseUri;
            }
        }

        /// <summary>
        /// Frees the resources held by the response.
        /// </summary>
        public override void Close()
        {
            if (stream != null)
            {
                stream.Close();
                stream = null;
            }

            if (f != null)
            {
                GetStatus();
                f.Close();
            }
        }
    }
}
