// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DownloadProgressChangedEventArgs.cs" company="In The Hand Ltd">
// Copyright (c) 2009-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Runtime.InteropServices;
using InTheHand.ComponentModel;

namespace InTheHand.Net
{
    #region Upload Data Completed
    /// <summary>
    /// Represents the method that will handle the <see cref="WebClient.UploadDataCompleted"/> event of a <see cref="WebClient"/>.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="UploadDataCompletedEventArgs"/> containing event data.</param>
    public delegate void UploadDataCompletedEventHandler(object sender, UploadDataCompletedEventArgs e);
    /// <summary>
    /// Provides data for the <see cref="WebClient.UploadDataCompleted"/> event.
    /// </summary>
    public class UploadDataCompletedEventArgs : AsyncCompletedEventArgs
    {
        private byte[] result;

        internal UploadDataCompletedEventArgs(byte[] result, Exception exception, bool cancelled, object userToken)
            : base(exception, cancelled, userToken)
        {
            this.result = result;
        }
        /// <summary>
        /// Gets the server reply to a data upload operation started by calling an <see cref="WebClient.UploadDataAsync(Uri,byte[])"/> method.
        /// </summary>
        public byte[] Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return this.result;
            }
        }
    }
    #endregion

    #region Upload File Completed
    /// <summary>
    /// Represents the method that will handle the <see cref="WebClient.UploadFileCompleted"/> event of a <see cref="WebClient"/>.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="UploadFileCompletedEventArgs"/> that contains event data.</param>
    public delegate void UploadFileCompletedEventHandler(object sender, UploadFileCompletedEventArgs e);

    /// <summary>
    /// Provides data for the <see cref="WebClient.UploadFileCompleted"/> event.
    /// </summary>
    public class UploadFileCompletedEventArgs : AsyncCompletedEventArgs
    {
        private byte[] result;

        internal UploadFileCompletedEventArgs(byte[] result, Exception exception, bool cancelled, object userToken)
            : base(exception, cancelled, userToken)
        {
            this.result = result;
        }
        /// <summary>
        /// Gets the server reply to a data upload operation that is started by calling an <see cref="WebClient.UploadFileAsync(Uri,String)"/> method.
        /// </summary>
        public byte[] Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return this.result;
            }
        }
    }
    #endregion

    #region Upload String Completed
    /// <summary>
    /// Represents the method that will handle the <see cref="WebClient.UploadStringCompleted"/> event of a <see cref="WebClient"/>.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="UploadStringCompletedEventArgs"/> containing event data.</param>
    public delegate void UploadStringCompletedEventHandler(object sender, UploadStringCompletedEventArgs e);
    /// <summary>
    /// Provides data for the <see cref="WebClient.UploadStringCompleted"/> event.
    /// </summary>
    public class UploadStringCompletedEventArgs : AsyncCompletedEventArgs
    {
        private string result;

        internal UploadStringCompletedEventArgs(string result, Exception exception, bool cancelled, object userToken)
            : base(exception, cancelled, userToken)
        {
            this.result = result;
        }
        /// <summary>
        /// Gets the server reply to a string upload operation that is started by calling an <see cref="WebClient.UploadStringAsync(Uri,String)"/> method.
        /// </summary>
        public string Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return this.result;
            }
        }
    }
    #endregion

    #region Upload Values Completed
    /// <summary>
    /// Represents the method that will handle the <see cref="WebClient.UploadValuesCompleted"/> event of a <see cref="WebClient"/>.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="UploadValuesCompletedEventArgs"/> that contains event data.</param>
    public delegate void UploadValuesCompletedEventHandler(object sender, UploadValuesCompletedEventArgs e);
    /// <summary>
    /// Provides data for the <see cref="WebClient.UploadValuesCompleted"/> event.
    /// </summary>
    public class UploadValuesCompletedEventArgs : AsyncCompletedEventArgs
    {
        private byte[] result;

        internal UploadValuesCompletedEventArgs(byte[] result, Exception exception, bool cancelled, object userToken)
            : base(exception, cancelled, userToken)
        {
            this.result = result;
        }
        /// <summary>
        /// Gets the server reply to a data upload operation started by calling an <see cref="WebClient.UploadValuesAsync(Uri,System.Collections.Specialized.NameValueCollection)"/> method.
        /// </summary>
        public byte[] Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return this.result;
            }
        }
    }
    #endregion

    #region Download Data Completed
    /// <summary>
    /// Provides data for the <see cref="WebClient.DownloadDataCompleted"/> event.
    /// </summary>
    public class DownloadDataCompletedEventArgs : AsyncCompletedEventArgs
    {
        private byte[] result;

        internal DownloadDataCompletedEventArgs(byte[] result, Exception exception, bool cancelled, object userToken)
            : base(exception, cancelled, userToken)
        {
            this.result = result;
        }
        /// <summary>
        /// Gets the data that is downloaded by a <see cref="WebClient.DownloadDataAsync(System.Uri)"/> method.
        /// </summary>
        public byte[] Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return this.result;
            }
        }
    }
    /// <summary>
    /// Represents the method that will handle the <see cref="WebClient.DownloadDataCompleted"/> event of a <see cref="WebClient"/>.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="DownloadDataCompletedEventArgs"/> containing event data.</param>
    public delegate void DownloadDataCompletedEventHandler(object sender, DownloadDataCompletedEventArgs e);
    #endregion

    #region Download String Completed
    /// <summary>
    /// Provides data for the <see cref="WebClient.DownloadStringCompleted"/> event.
    /// </summary>
    public class DownloadStringCompletedEventArgs : AsyncCompletedEventArgs
    {
        private string result;

        internal DownloadStringCompletedEventArgs(string result, Exception exception, bool cancelled, object userToken)
            : base(exception, cancelled, userToken)
        {
            this.result = result;
        }
        /// <summary>
        /// Gets the data that is downloaded by a <see cref="WebClient.DownloadStringAsync(System.Uri)"/> method.
        /// </summary>
        public string Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return this.result;
            }
        }
    }
    /// <summary>
    /// Represents the method that will handle the <see cref="WebClient.DownloadStringCompleted"/> event of a <see cref="WebClient"/>.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="DownloadStringCompletedEventArgs"/> that contains event data.</param>
    /// <remarks>When you create a <see cref="DownloadStringCompletedEventHandler"/> delegate, you identify the method that will handle the event.
    /// To associate the event with your event handler, add an instance of the delegate to the event.
    /// The event handler is called whenever the event occurs, unless you remove the delegate.
    /// </remarks>
    public delegate void DownloadStringCompletedEventHandler(object sender, DownloadStringCompletedEventArgs e);
    #endregion



    #region Download Progress Changed
    /// <summary>
    /// Represents the method that will handle the <see cref="WebClient.DownloadProgressChanged"/> event of a <see cref="WebClient"/>.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="DownloadProgressChangedEventArgs"/> containing event data.</param>
    public delegate void DownloadProgressChangedEventHandler(object sender, DownloadProgressChangedEventArgs e);

    /// <summary>
    /// Provides data for the <see cref="WebClient.DownloadProgressChanged"/> event of a <see cref="WebClient"/>.
    /// </summary>
    public class DownloadProgressChangedEventArgs : ProgressChangedEventArgs
    {
        
        private long bytesReceived;
        private long totalBytesToReceive;


        internal DownloadProgressChangedEventArgs(int progressPercentage, object userToken, long bytesReceived, long totalBytesToReceive)
            : base(progressPercentage, userToken)
        {
            this.bytesReceived = bytesReceived;
            this.totalBytesToReceive = totalBytesToReceive;
        }

        /// <summary>
        /// Gets the number of bytes received.
        /// </summary>
        public long BytesReceived
        {
            get
            {
                return this.bytesReceived;
            }
        }
        /// <summary>
        /// Gets the total number of bytes in a <see cref="WebClient"/> data upload operation.
        /// </summary>
        public long TotalBytesToReceive
        {
            get
            {
                return this.totalBytesToReceive;
            }
        }
    }
    #endregion

    #region Upload Progress Changed
    /// <summary>
    /// Represents the method that will handle the <see cref="WebClient.UploadProgressChanged"/> event of a <see cref="WebClient"/>.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">An <see cref="UploadProgressChangedEventArgs"/> containing event data.</param>
    public delegate void UploadProgressChangedEventHandler(object sender, UploadProgressChangedEventArgs e);

    /// <summary>
    /// Provides data for the <see cref="WebClient.UploadProgressChanged"/> event of a <see cref="WebClient"/>.
    /// </summary>
    public class UploadProgressChangedEventArgs : ProgressChangedEventArgs
    {

        private long bytesReceived;
        private long bytesSent;
        private long totalBytesToReceive;
        private long totalBytesToSend;

        internal UploadProgressChangedEventArgs(int progressPercentage, object userToken, long bytesSent, long totalBytesToSend, long bytesReceived, long totalBytesToReceive)
            : base(progressPercentage, userToken)
        {
            this.bytesReceived = bytesReceived;
            this.totalBytesToReceive = totalBytesToReceive;
            this.bytesSent = bytesSent;
            this.totalBytesToSend = totalBytesToSend;
        }

        /// <summary>
        /// Gets the number of bytes received.
        /// </summary>
        public long BytesReceived
        {
            get
            {
                return this.bytesReceived;
            }
        }
        /// <summary>
        /// Gets the total number of bytes in a <see cref="WebClient"/> data upload operation.
        /// </summary>
        public long TotalBytesToReceive
        {
            get
            {
                return this.totalBytesToReceive;
            }
        }

        /// <summary>
        /// Gets the number of bytes sent.
        /// </summary>
        public long BytesSent
        {
            get
            {
                return this.bytesSent;
            }
        }

        /// <summary>
        /// Gets the total number of bytes to send.
        /// </summary>
        public long TotalBytesToSend
        {
            get
            {
                return this.totalBytesToSend;
            }
        }
    }
    #endregion

}
