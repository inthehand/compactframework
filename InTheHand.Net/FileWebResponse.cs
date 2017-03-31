// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileWebResponse.cs" company="In The Hand Ltd">
// Copyright (c) 2008-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net;

namespace InTheHand.Net
{
    /// <summary>
    /// Provides a file system implementation of the <see cref="WebResponse"/> class.
    /// </summary>
    /// <remarks><para>Equivalent to System.Net.FileWebResponse</para>
    /// The FileWebResponse class implements the <see cref="WebResponse"/> abstract base class to return file system resources for the <see cref="FileWebRequest"/> class.
    /// <para>Client applications do not create FileWebResponse instances directly; instead, they are created by calling the <see cref="FileWebRequest.GetResponse"/> method on a <see cref="FileWebRequest"/> instance.</para>
    /// <para>The <see cref="GetResponseStream"/> method returns a <see cref="Stream"/> instance that provides read-only access to a file system resource.</para></remarks>
    public class FileWebResponse : WebResponse
    {
        private FileWebRequest request;
        private FileStream stream;

        #region Constructor
        internal FileWebResponse(FileWebRequest request)
        {
            this.request = request;

            /*switch (request.Method)
            {
                case WebRequestMethods.File.DownloadFile:*/
                    stream = new FileStream(request.RequestUri.AbsolutePath, FileMode.Open, FileAccess.Read);
                    contentLength = stream.Length;
                    /*break;

                case WebRequestMethods.File.UploadFile:
                    FileStream fs = new FileStream(request.RequestUri.AbsolutePath, FileMode.Create);
                    Stream requestStream = request.GetRequestStream();
                    requestStream.CopyTo(fs);
                    
                    contentLength = requestStream.Length;

                    requestStream.Close();
                    fs.Close();
                    break;

            }*/

        }
        #endregion


        #region Content Length
        private long contentLength = 0;
        /// <summary>
        /// Gets the length of the content in the file system resource.
        /// </summary>
        public override long ContentLength
        {
            get
            {
                return contentLength;
            }
        }
        #endregion

        #region Content Type
        /// <summary>
        /// Gets the content type of the file system resource.
        /// </summary>
        /// <value>The value "binary/octet-stream".</value>
        /// <remarks>The ContentType property contains the content type of the file system resource.
        /// The value of ContentType is always "binary/octet-stream".</remarks>
        public override string ContentType
        {
            get
            {
                return InTheHand.Net.Mime.MediaTypeNames.Application.Octet;
            }
            set
            {
            }
        }
        #endregion

        #region Get Response Stream

        /// <summary>
        /// Returns the data stream from the file system resource.
        /// </summary>
        /// <returns>A <see cref="Stream"/> for reading data from the file system resource.</returns>
        public override Stream GetResponseStream()
        {
            //if (request.Method == WebRequestMethods.File.DownloadFile)
            //{
                return stream;
            //}
            //operation doesn't return data stream
            //throw new InvalidOperationException();
        }
        #endregion

        #region Response Uri
        /// <summary>
        /// Gets the URI that sent the response to the request.
        /// </summary>
        /// <value>A Uri that contains the URI of the file system resource that provided the response.</value>
        /// <remarks>The ResponseUri property contains the URI of the file system resource that provided the response.
        /// This is always the file system resource that was requested.</remarks>
        public override Uri ResponseUri
        {
            get
            {
                return request.RequestUri;
            }
        }
        #endregion

        #region Close
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
        }
        #endregion
    }
}
