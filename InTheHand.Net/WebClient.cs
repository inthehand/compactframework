// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebClient.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using InTheHand.ComponentModel;
using InTheHand.IO;
using System.ComponentModel;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;

namespace InTheHand.Net
{
    /// <summary>
    /// Provides helper methods for sending data to and receiving data from a resource identified by a URI.
    /// </summary>
    /// <remarks>Equivalent to System.Net.WebClient</remarks>
    public class WebClient : Component
    {
        private int callNesting = 0;
        private bool cancelled = false;
#pragma warning disable 0649, 0169, 0414
        private long contentLength;
#pragma warning restore 0649, 0169, 0414
        private string method;
        private ProgressData progress = new ProgressData();
        private NameValueCollection requestParameters;

        #region Progress Data
        private class ProgressData
        {
            internal long BytesReceived;
            internal long BytesSent;
            internal bool HasUploadPhase;
            internal long TotalBytesToReceive = -1L;
            internal long TotalBytesToSend = -1L;

            internal void Reset()
            {
                this.BytesSent = 0L;
                this.TotalBytesToSend = -1L;
                this.BytesReceived = 0L;
                this.TotalBytesToReceive = -1L;
                this.HasUploadPhase = false;
            }
        }
        #endregion

        private WebRequest webRequest;
        private WebResponse webResponse;

        static WebClient()
        {
            // add our custom ftp/file implementations
            FtpWebRequest.RegisterPrefix();
            FileWebRequest.RegisterPrefix();
        }

        #region Abort Request
        private static void AbortRequest(WebRequest request)
        {
            try
            {
                if (request != null)
                {
                    request.Abort();
                }
            }
            catch (Exception exception)
            {
                if (((exception is OutOfMemoryException) || (exception is StackOverflowException)) || (exception is ThreadAbortException))
                {
                    throw;
                }
            }
            catch
            {
            }
        }


        #endregion

        #region Another Call In Progress
        private bool AnotherCallInProgress(int callNesting)
        {
            return (callNesting > 1);
        }
        #endregion

        #region Clear WebClient State
        private void ClearWebClientState()
        {
            if (this.AnotherCallInProgress(Interlocked.Increment(ref this.callNesting)))
            {
                this.CompleteWebClientState();
                throw new NotSupportedException(InTheHand.Net.Properties.Resources.net_webclient_no_concurrent_io_allowed);
            }
            this.contentLength = -1L;
            this.webResponse = null;
            this.webRequest = null;
            this.method = null;
            this.cancelled = false;
            if (this.progress != null)
            {
                this.progress.Reset();
            }
        }
        #endregion

        #region Complete WebClient State
        private void CompleteWebClientState()
        {
            Interlocked.Decrement(ref this.callNesting);
        }
        #endregion

        #region Base Address
        private Uri baseAddress = null;
        /// <summary>
        /// Gets or sets the base URI for requests made by a <see cref="WebClient"/>.
        /// </summary>
        /// <value>A <see cref="String"/> containing the base URI for requests made by a <see cref="WebClient"/> or <see cref="String.Empty"/> if no base address has been specified.</value>
        /// <exception cref="ArgumentException">BaseAddress is set to an invalid URI.
        /// The inner exception may contain information that will help you locate the error.</exception>
        /// <remarks>The BaseAddress property contains a base URI that is combined with a relative address.
        /// When you call a method that uploads or downloads data, the WebClient object combines this base URI with the relative address you specify in the method call.
        /// If you specify an absolute URI, WebClient does not use the BaseAddress property value.
        /// To remove a previously set value, set this property to a null reference (Nothing in Visual Basic) or an empty string ("").</remarks>
        public string BaseAddress
        {
            get
            {
                if (baseAddress != null)
                {
                    return baseAddress.ToString();
                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    baseAddress = null;
                }
                else
                {
                    try
                    {
                        baseAddress = new Uri(value);
                    }
                    catch (UriFormatException exception)
                    {
                        throw new ArgumentException(InTheHand.Net.Properties.Resources.net_webclient_invalid_baseaddress, exception);
                    }
                }

            }
        }
        #endregion

        #region Credentials
        private ICredentials credentials;
        /// <summary>
        /// Gets or sets the network credentials that are sent to the host and used to authenticate the request.
        /// </summary>
        public System.Net.ICredentials Credentials
        {
            get
            {
                return credentials;
            }
            set
            {
                credentials = value;
            }    
        }
        #endregion

        #region Use Default Credentials
        private bool useDefaultCredentials;
        /// <summary>
        /// Gets or sets a Boolean value that controls whether the <see cref="CredentialCache.DefaultCredentials"/> are sent with requests.
        /// </summary>
        public bool UseDefaultCredentials
        {
            get
            {
                return useDefaultCredentials;
            }
            set
            {
                useDefaultCredentials = value;
            }
        }
        #endregion

        #region Encoding
        private Encoding encoding = Encoding.Default;
        /// <summary>
        /// Gets and sets the <see cref="System.Text.Encoding"/> used to upload and download strings. 
        /// </summary>
        public Encoding Encoding
        {
            get
            {
                return encoding;
            }

            set
            {
                encoding = value;
            }
        }
        #endregion

        #region Headers
        private WebHeaderCollection headers = new WebHeaderCollection();
        /// <summary>
        /// Gets or sets a collection of header name/value pairs associated with the request.
        /// </summary>
        /// <value>A <see cref="WebHeaderCollection"/> containing header name/value pairs associated with this request.</value>
        /// <remarks>The <see cref="Headers"/> property contains a <see cref="WebHeaderCollection"/> instance containing header information that the <see cref="WebClient"/> sends with the request.
        /// This is an unrestricted collection of headers, so setting headers that are restricted by <see cref="WebRequest"/> descendants such as <see cref="HttpWebRequest"/> is allowed.</remarks>
        public WebHeaderCollection Headers
        {
            get
            {
                return headers;
            }

            set
            {
                headers = value;
            }
        }

        #endregion

        #region Is Busy
        /// <summary>
        /// Gets whether a Web request is in progress.
        /// </summary>
        /// <value>true if the Web request is still in progress; otherwise false.</value>
        public bool IsBusy
        {
            get
            {
                return callNesting > 0;
            }
        }
        #endregion

        #region Map To Default Method
        private string MapToDefaultMethod(Uri address)
        {
            if (GetAbsoluteUri(address).Scheme.ToLower(System.Globalization.CultureInfo.InvariantCulture) == "ftp")
            {
                return "STOR";
            }
            return "POST";
        }
        #endregion

        #region Proxy
        private IWebProxy proxy = System.Net.GlobalProxySelection.GetEmptyWebProxy();
        /// <summary>
        /// Gets or sets the proxy used by this <see cref="WebClient"/> object.
        /// </summary>
        /// <value>An <see cref="IWebProxy"/> instance used to send requests.</value>
        /// <remarks>The Proxy property identifies the <see cref="IWebProxy"/> instance that communicates with remote servers on behalf of this <see cref="WebClient"/> object.
        /// The proxy is set by the system using configuration files and the Internet Explorer Mobile Local Area Network settings.
        /// To specify that no proxy should be used, set the Proxy property to the proxy instance returned by the <see cref="GlobalProxySelection.GetEmptyWebProxy"/> method.</remarks>
        public IWebProxy Proxy
        {
            get
            {
                return proxy;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Proxy");
                }
                proxy = value;
            }
        }
        #endregion

        #region Query String
        /// <summary>
        /// Gets or sets a collection of query name/value pairs associated with the request.
        /// </summary>
        /// <value>A <see cref="NameValueCollection"/> that contains query name/value pairs associated with the request.
        /// If no pairs are associated with the request, the value is an empty <see cref="NameValueCollection"/>.</value>
        public NameValueCollection QueryString
        {
            get
            {
                if (this.requestParameters == null)
                {
                    this.requestParameters = new NameValueCollection();
                }
                return this.requestParameters;
            }
            set
            {
                this.requestParameters = value;
            }
        }
        #endregion

        #region Use Default Credentials
        /*
        private bool useDefaultCredentials = false;
        /// <summary>
        /// Gets or sets a <see cref="Boolean"/> value that controls whether the <see cref="CredentialCache.DefaultCredentials"/> are sent with requests.
        /// </summary>
        /// <value>true if the default credentials are used; otherwise false.
        /// The default value is false.</value>
        public bool UseDefaultCredentials
        {
            get
            {
                return useDefaultCredentials;
            }
            set
            {
                useDefaultCredentials = value;
            }
        }*/
        #endregion


        /// <summary>
        /// Returns a <see cref="WebRequest"/> object for the specified resource.
        /// </summary>
        /// <param name="address">A <see cref="Uri"/> that identifies the resource to request.</param>
        /// <returns>A new <see cref="WebRequest"/> object for the specified resource.</returns>
        protected virtual WebRequest GetWebRequest(Uri address)
        {
            //create a web request for the uri
            WebRequest request = WebRequest.Create(address);
            request.Proxy = this.proxy;

            if (this.UseDefaultCredentials == true)
            {
                request.Credentials = CredentialCache.DefaultCredentials.GetCredential(address, "Basic");
            }
            else if (this.Credentials != null)
            {
                request.Credentials = this.Credentials;
            }

            if (this.headers.Count > 0)
            {
                WebHeaderCollection whc = new WebHeaderCollection();
                foreach (string header in this.headers.Keys)
                {
                    switch (header)
                    {
                        case "Content-Type":
                            System.Diagnostics.Debug.WriteLine("WebClient.GetWebRequest Set ContentType");
                            request.ContentType = this.headers["Content-Type"];
                            break;
                        default:
                            System.Diagnostics.Debug.WriteLine("WebClient.GetWebRequest " + header + ": " + this.headers[header]);
                            whc.Add(header, this.headers[header]);
                            break;
                    }

                }
                request.Headers = whc;
            }

            return request;
        }

        /// <summary>
        /// Returns the <see cref="WebResponse"/> for the specified <see cref="WebRequest"/>. 
        /// </summary>
        /// <param name="request">A <see cref="WebRequest"/> that is used to obtain the response.</param>
        /// <returns>A <see cref="WebResponse"/> for the specified <see cref="WebRequest"/>.</returns>
        /// <remarks>The object returned by this method is obtained by calling the <see cref="WebRequest.GetResponse"/> method on the specified <see cref="WebRequest"/> object. 
        /// This method can be called only by classes that inherit from <see cref="WebClient"/>.
        /// It is provided to give inheritors access to the underlying <see cref="WebResponse"/> object.
        /// </remarks>
        /// <example>The following code example shows an implementation of this method that can be customized by a class derived from <see cref="WebClient"/>.
        /// <code lang="cs">
        /// protected override WebResponse GetWebResponse (WebRequest request)
        /// {
        ///     WebResponse response = base.GetWebResponse (request);
        ///     // Perform any custom actions with the response ...
        ///     return response;
        /// }
        /// </code></example>
        protected virtual WebResponse GetWebResponse(WebRequest request)
        {
            return request.GetResponse();
        }

        /// <summary>
        /// Returns the <see cref="WebResponse"/> for the specified WebRequest using the specified <see cref="IAsyncResult"/>.
        /// </summary>
        /// <param name="request">A <see cref="WebRequest"/> that is used to obtain the response.</param>
        /// <param name="result">An <see cref="IAsyncResult"/> object obtained from a previous call to <see cref="WebRequest.BeginGetResponse"/>.</param>
        /// <returns>A <see cref="WebResponse"/> containing the response for the specified <see cref="WebRequest"/>.</returns>
        /// <example>The following code example shows an implementation of this method that can be customized by a class derived from WebClient.
        /// <code lang="cs">
        /// protected override WebResponse GetWebResponse (WebRequest request, IAsyncResult result)
        /// {
        ///     WebResponse response = base.GetWebResponse (request, result);
        ///     // Perform any custom actions with the response ...
        ///     return response;
        /// }</code>
        /// </example>
        protected virtual WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            return request.EndGetResponse(result);
        }

        private Uri GetAbsoluteUri(string address)
        {
            if (Uri.IsWellFormedUriString(address, UriKind.Absolute))
            {
                return GetAbsoluteUri(new Uri(address));
            }
            else
            {
                return GetAbsoluteUri(new Uri(address, UriKind.Relative));
            }
        }

        private Uri GetAbsoluteUri(Uri address)
        {
            Uri result = null;

            if (address.IsAbsoluteUri)
            {
                result = address;
            }
            else if (baseAddress != null)
            {
                result = new Uri(baseAddress, address);
            }

            if (result == null)
            {
                throw new WebException("The URI formed by combining BaseAddress and address is invalid");
            }

            if (string.IsNullOrEmpty(result.Query) && (this.requestParameters != null) && (this.requestParameters.Count > 0))
            {
                //add querystring
                StringBuilder sb = new StringBuilder();
                string str = "?";
                for (int i = 0; i < this.requestParameters.Count; i++)
                {
                    sb.Append(str + this.requestParameters.AllKeys[i] + "=" + this.requestParameters[i]);
                    str = "&";
                }
                result = new Uri(result.ToString() + sb.ToString());

            }
            return result;
        }

        /// <summary>
        /// Cancels a pending asynchronous operation.
        /// </summary>
        /// <remarks>If an operation is pending, this method calls Abort on the underlying <see cref="WebRequest"/>.
        /// When you call CancelAsync, your application still receives the completion event associated with the operation.
        /// For example, when you call CancelAsync to cancel a <see cref="DownloadStringAsync(System.Uri)"/> operation, if you have specified an event handler for the <see cref="DownloadStringCompleted"/> event, your event handler receives notification that the operation has ended.
        /// To learn whether the operation completed successfully, check the <see cref="AsyncCompletedEventArgs.Cancelled"/> property on the base class of <see cref="DownloadStringCompletedEventArgs"/> in the event data object passed to the event handler. 
        /// If no asynchronous operation is in progress, this method does nothing.</remarks>
        public void CancelAsync()
        {
            cancelled = true;
            AbortRequest(this.webRequest);
        }

        /// <summary>
        /// Downloads data at the specified URI as a <see cref="Byte"/> array.
        /// </summary>
        /// <param name="address">The URI from which to download data.</param>
        /// <returns>A Byte array containing the downloaded resource.</returns>
        public byte[] DownloadData(string address)
        {
            return DownloadData(GetAbsoluteUri(address));
        }

        /// <summary>
        /// Downloads data at the specified URI as a <see cref="Byte"/> array.
        /// </summary>
        /// <param name="address">The URI from which to download data.</param>
        /// <returns>A Byte array containing the downloaded resource.</returns>
        public byte[] DownloadData(Uri address)
        {
            
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }

            this.ClearWebClientState();

            //create a web request for the uri
            WebRequest request = GetWebRequest(GetAbsoluteUri(address));

            //perform the GET request
            try
            {
                WebResponse response = GetWebResponse(request);

                //get stream containing received data
                Stream s = response.GetResponseStream();

                byte[] result = ReadResponseStream(s);
                response.Close();
                return result;

            }
            catch (Exception ex)
            {
                throw new WebException(string.Empty, ex);
            }
            finally
            {
                CompleteWebClientState();
            }
        }

        /// <summary>
        /// Downloads the specified resource as a <see cref="Byte"/> array.
        /// This method does not block the calling thread.
        /// </summary>
        /// <param name="address">The URI of the resource to download.</param>
        public void DownloadDataAsync(Uri address)
        {
            DownloadDataAsync(address, null);
        }

        /// <summary>
        /// Downloads the specified resource as a <see cref="Byte"/> array.
        /// This method does not block the calling thread.
        /// </summary>
        /// <param name="address">The URI of the resource to download.</param>
        /// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
        public void DownloadDataAsync(Uri address, Object userToken)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }

            this.ClearWebClientState();

            try
            {
                DownloadAsyncState das = new DownloadAsyncState();
                das.request = this.GetWebRequest(GetAbsoluteUri(address));
                webRequest = das.request;
                das.userToken = userToken;
                IAsyncResult iar = das.request.BeginGetResponse(new AsyncCallback(DownloadDataCallback), das);
            }
            catch (Exception exception)
            {
                if (((exception is System.Threading.ThreadAbortException) || (exception is StackOverflowException)) || (exception is OutOfMemoryException))
                {
                    throw;
                }
                if (!(exception is WebException))
                {
                    throw;
                }
            }
            catch
            {
                throw;
            }
        }

        private void DownloadDataCallback(IAsyncResult result)
        {
            byte[] theData = null;
            Exception theException = null;
            DownloadAsyncState das = null;
            try
            {
                das = (DownloadAsyncState)result.AsyncState;
                webResponse = GetWebResponse(webRequest,result);

                //get stream containing received data
                theData = new byte[webResponse.ContentLength];

                Stream s = webResponse.GetResponseStream();

                //read all data in one operation
                int bytesRead = s.Read(theData, 0, theData.Length);

                //close both streams
                s.Close();
                webResponse.Close();
            }
            catch (Exception ex)
            {
                if (((ex is ThreadAbortException) || (ex is StackOverflowException)) || (ex is OutOfMemoryException))
                {
                    throw;
                }

                theException = ex;
            }
            finally
            {
                this.CompleteWebClientState();
            }
            OnDownloadDataCompleted(new DownloadDataCompletedEventArgs(theData, theException, this.cancelled, das.userToken));

        }

        /// <summary>
        /// Raises the <see cref="DownloadDataCompleted"/> event.
        /// </summary>
        /// <param name="e">A <see cref="DownloadDataCompletedEventArgs"/> object that contains event data.</param>
        protected virtual void OnDownloadDataCompleted(DownloadDataCompletedEventArgs e)
        {
            if (this.DownloadDataCompleted != null)
            {
                this.DownloadDataCompleted(this, e);
            }
        }

        /// <summary>
        /// Occurs when an asynchronous data download operation completes.
        /// </summary>
        public event DownloadDataCompletedEventHandler DownloadDataCompleted;

        /// <summary>
        /// Downloads the resource with the specified URI to a local file.
        /// </summary>
        /// <param name="address">The URI from which to download data.</param>
        /// <param name="fileName">The name of the local file that is to receive the data.</param>
        public void DownloadFile(string address, string fileName)
        {
            DownloadFile(GetAbsoluteUri(address), fileName);
        }

        /// <summary>
        /// Downloads the resource with the specified URI to a local file.
        /// </summary>
        /// <param name="address">The URI specified as a String, from which to download data.</param>
        /// <param name="fileName">The name of the local file that is to receive the data.</param>
        public void DownloadFile(Uri address, string fileName)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            Uri absoluteAddress = GetAbsoluteUri(address);
            
            this.ClearWebClientState();

            try
            {
                if (address.Scheme == "ftp")
                {
                    Ftp f = new Ftp(absoluteAddress, (NetworkCredential)this.Credentials, true);
                    try
                    {
                        f.GetFile(absoluteAddress.LocalPath, fileName);
                    }
                    finally
                    {
                        f.Close();
                    }
                }
                else
                {
                    //create a web request for the uri
                    WebRequest request = GetWebRequest(absoluteAddress);

                    //perform the GET request
                    WebResponse response = GetWebResponse(request);

                    //get stream containing received data
                    Stream s = response.GetResponseStream();

                    //open filestream for output file
                    FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);

                    //copy until all data is read
                    s.CopyTo(fs);

                    //close both streams
                    s.Close();
                    response.Close();
                    fs.Close();
                }
            }
            finally
            {
                CompleteWebClientState();
            }
        }

        /// <summary>
        /// Downloads, to a local file, the resource with the specified URI.
        /// This method does not block the calling thread.
        /// </summary>
        /// <param name="address">The URI of the resource to download.</param>
        /// <param name="fileName">The name of the file to be placed on the local computer.</param>
        public void DownloadFileAsync(Uri address, string fileName)
        {
            DownloadFileAsync(address, fileName, null);
        }

        /// <summary>
        /// Downloads, to a local file, the resource with the specified URI.
        /// This method does not block the calling thread.
        /// </summary>
        /// <param name="address">The URI of the resource to download.</param>
        /// <param name="fileName">The name of the file to be placed on the local computer.</param>
        /// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
        public void DownloadFileAsync(Uri address, string fileName, Object userToken)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            this.ClearWebClientState();

            try
            {
                DownloadAsyncState das = new DownloadAsyncState();
                das.writeStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                das.request = this.GetWebRequest(GetAbsoluteUri(address));
                webRequest = das.request;
                das.userToken = userToken;
                IAsyncResult iar = das.request.BeginGetResponse(new AsyncCallback(DownloadFileCallback), das);
            }
            catch (Exception exception)
            {
                if (((exception is System.Threading.ThreadAbortException) || (exception is StackOverflowException)) || (exception is OutOfMemoryException))
                {
                    throw;
                }
                if (!(exception is WebException))
                {
                    throw;
                }
            }
            catch
            {
                throw;
            }
        }

        private void DownloadFileCallback(IAsyncResult result)
        {
            Exception theException = null;
            DownloadAsyncState das = null;
            try
            {
                das = (DownloadAsyncState)result.AsyncState;
                webResponse = GetWebResponse(webRequest, result);

                //get stream containing received data
                Stream s = webResponse.GetResponseStream();

                //copy until all data is read
                s.CopyTo(das.writeStream);

                //close both streams
                s.Close();
                webResponse.Close();
                das.writeStream.Close();
            }
            catch (Exception ex)
            {
                if (((ex is ThreadAbortException) || (ex is StackOverflowException)) || (ex is OutOfMemoryException))
                {
                    throw;
                }

                theException = ex;
            }
            finally
            {
                this.CompleteWebClientState();
            }
            OnDownloadFileCompleted(new AsyncCompletedEventArgs(theException, this.cancelled, das.userToken));

        }

        /// <summary>
        /// Raises the <see cref="DownloadFileCompleted"/> event.
        /// </summary>
        /// <param name="e">An <see cref="AsyncCompletedEventArgs"/> object containing event data.</param>
        protected virtual void OnDownloadFileCompleted(AsyncCompletedEventArgs e)
        {
            if (this.DownloadFileCompleted != null)
            {
                this.DownloadFileCompleted(this, e);
            }
        }

        /// <summary>
        /// Occurs when an asynchronous file download operation completes.
        /// </summary>
        public event AsyncCompletedEventHandler DownloadFileCompleted;

        /// <summary>
        /// Downloads the specified resource as a <see cref="String"/>.
        /// </summary>
        /// <param name="address">The URI from which to download data.</param>
        /// <returns>A <see cref="String"/> containing the specified resource.</returns>
        public string DownloadString(string address)
        {
            return DownloadString(GetAbsoluteUri(address));
        }
        /// <summary>
        /// Downloads the specified resource as a <see cref="String"/>.
        /// </summary>
        /// <param name="address">The URI from which to download data.</param>
        /// <returns>A <see cref="String"/> containing the specified resource.</returns>
        public string DownloadString(System.Uri address)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }

            string data = null;

            this.ClearWebClientState();

            try
            {
                //create a web request for the uri
                WebRequest request = GetWebRequest(GetAbsoluteUri(address));

                //perform the GET request
                WebResponse response = GetWebResponse(request);

                //get stream containing received data
                Stream s = response.GetResponseStream();
                StreamReader sr = new StreamReader(s, this.encoding);
                data = sr.ReadToEnd();
                sr.Close();
                s.Close();
                response.Close();

            }
            finally
            {
                CompleteWebClientState();
            }
            return data;
        }

        internal class DownloadAsyncState
        {
            public WebRequest request;
            public object userToken;
            public Stream writeStream;
        }

        internal class UploadAsyncState
        {
            public WebRequest request;
            public object userToken;
            public byte[] dataValue;
            public string stringValue;
            public string fileName;
            public NameValueCollection values;
        }

        /// <summary>
        /// Downloads the resource specified as a <see cref="Uri"/>. This method does not block the calling thread.
        /// </summary>
        /// <param name="address">A <see cref="Uri"/> containing the URI to download.</param>
        /// <remarks>This method retrieves the specified resource using the GET method.
        /// The resource is downloaded asynchronously using thread resources that are automatically allocated from the thread pool.
        /// <para>After downloading the resource, this method uses the encoding specified in the <see cref="Encoding"/> property to convert the resource to a <see cref="String"/>.
        /// This method does not block the calling thread while downloading the resource.
        /// To download a resource and block while waiting for the server's response, use the <see cref="DownloadString(String)"/> method.
        /// When the download completes, the <see cref="DownloadStringCompleted"/> event is raised.
        /// Your application must handle this event to receive notification.
        /// The downloaded string is available in the <see cref="DownloadStringCompletedEventArgs.Result"/> property.</para>
        /// <para>You can use the <see cref="CancelAsync"/> method to cancel asynchronous operations that have not completed.</para>
        /// <para>If the <see cref="BaseAddress"/> property is not an empty string ("") and address does not contain an absolute URI, address must be a relative URI that is combined with <see cref="BaseAddress"/> to form the absolute URI of the requested data.
        /// If the <see cref="QueryString"/> property is not an empty string, it is appended to address.</para>
        /// <para>This method uses the RETR command to download an FTP resource.
        /// For an HTTP resource, the GET method is used.</para></remarks>
        public void DownloadStringAsync(Uri address)
        {
            DownloadStringAsync(address, null);
        }

        /// <summary>
        /// Downloads the resource specified as a <see cref="Uri"/>. This method does not block the calling thread.
        /// </summary>
        /// <param name="address">A <see cref="Uri"/> containing the URI to download.</param>
        /// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
        /// <remarks>This method retrieves the specified resource using the GET method.
        /// The resource is downloaded asynchronously using thread resources that are automatically allocated from the thread pool.
        /// <para>After downloading the resource, this method uses the encoding specified in the <see cref="Encoding"/> property to convert the resource to a <see cref="String"/>.
        /// This method does not block the calling thread while downloading the resource.
        /// To download a resource and block while waiting for the server's response, use the <see cref="DownloadString(String)"/> method.
        /// When the download completes, the <see cref="DownloadStringCompleted"/> event is raised.
        /// Your application must handle this event to receive notification.
        /// The downloaded string is available in the <see cref="DownloadStringCompletedEventArgs.Result"/> property.</para>
        /// <para>You can use the <see cref="CancelAsync"/> method to cancel asynchronous operations that have not completed.</para>
        /// <para>If the <see cref="BaseAddress"/> property is not an empty string ("") and address does not contain an absolute URI, address must be a relative URI that is combined with <see cref="BaseAddress"/> to form the absolute URI of the requested data.
        /// If the <see cref="QueryString"/> property is not an empty string, it is appended to address.</para>
        /// <para>This method uses the RETR command to download an FTP resource.
        /// For an HTTP resource, the GET method is used.</para></remarks>
        public void DownloadStringAsync(Uri address, object userToken)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }

            this.ClearWebClientState();

            try
            {
                DownloadAsyncState das = new DownloadAsyncState();
                das.request = this.GetWebRequest(GetAbsoluteUri(address));
                webRequest = das.request;
                das.userToken = userToken;
                IAsyncResult iar = das.request.BeginGetResponse(new AsyncCallback(DownloadStringCallback), das);
            }
            catch (Exception exception)
            {
                if (((exception is System.Threading.ThreadAbortException) || (exception is StackOverflowException)) || (exception is OutOfMemoryException))
                {
                    throw;
                }
                if (!(exception is WebException))
                {
                    throw;
                }
            }
            catch
            {
                throw;
            }

        }

        private void DownloadStringCallback(IAsyncResult result)
        {
            string theString = null;
            Exception theException = null;
            DownloadAsyncState das = null;
            try
            {
                das = (DownloadAsyncState)result.AsyncState;
                webResponse = GetWebResponse(webRequest, result);


                //get stream containing received data
                Stream s = webResponse.GetResponseStream();
                StreamReader sr = new StreamReader(s, this.encoding);
                theString = sr.ReadToEnd();
                sr.Close();
                s.Close();
                webResponse.Close();
            }
            catch (Exception ex)
            {
                if (((ex is ThreadAbortException) || (ex is StackOverflowException)) || (ex is OutOfMemoryException))
                {
                    throw;
                }

                theException = ex;
            }
            finally
            {
                this.CompleteWebClientState();
            }

            OnDownloadStringCompleted(new DownloadStringCompletedEventArgs(theString, theException, this.cancelled, das.userToken));
            
        }

        /// <summary>
        /// Raises the <see cref="DownloadStringCompleted"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDownloadStringCompleted(DownloadStringCompletedEventArgs e)
        {
            if (this.DownloadStringCompleted != null)
            {
                this.DownloadStringCompleted(this, e);
            }
        }

        /// <summary>
        /// Occurs when an asynchronous resource-download operation completes.
        /// </summary>
        public event DownloadStringCompletedEventHandler DownloadStringCompleted;

        /// <summary>
        /// Opens a readable stream for the data downloaded from a resource with the URI specified as a <see cref="String"/>.
        /// </summary>
        /// <param name="address">The URI specified as a <see cref="String"/> from which to download data.</param>
        /// <returns>A <see cref="Stream"/> used to read data from a resource.</returns>
        public Stream OpenRead(string address)
        {
            return this.OpenRead(GetAbsoluteUri(address));
        }

        /// <summary>
        /// Opens a readable stream for the data downloaded from a resource with the URI specified as a <see cref="Uri"/>.
        /// </summary>
        /// <param name="address">The URI specified as a <see cref="Uri"/> from which to download data. </param>
        /// <returns>A <see cref="Stream"/> used to read data from a resource.</returns>
        public Stream OpenRead(Uri address)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }

            this.ClearWebClientState();

            WebRequest request = GetWebRequest(GetAbsoluteUri(address));
            WebResponse response = GetWebResponse(request);
            Stream s = response.GetResponseStream();
            
            CompleteWebClientState();
            
            return s;
        }

        /// <summary>
        /// Opens a stream for writing data to the specified resource.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the data.</param>
        /// <returns>A <see cref="Stream"/> used to write data to the resource.</returns>
        public Stream OpenWrite(string address)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }
            return this.OpenWrite(GetAbsoluteUri(address), null);
        }

        /// <summary>
        /// Opens a stream for writing data to the specified resource.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the data.</param>
        /// <returns>A <see cref="Stream"/> used to write data to the resource.</returns>
        public Stream OpenWrite(Uri address)
        {
            return this.OpenWrite(address, null);
        }

        /// <summary>
        /// Opens a stream for writing data to the specified resource, using the specified method.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the data.</param>
        /// <param name="method">The method used to send the data to the resource.
        /// If null, the default is POST for http and STOR for ftp.</param>
        /// <returns>A <see cref="Stream"/> used to write data to the resource.</returns>
        public Stream OpenWrite(string address, string method)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }
            return this.OpenWrite(GetAbsoluteUri(address), method);
        }

        /// <summary>
        /// Opens a stream for writing data to the specified resource, using the specified method.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the data.</param>
        /// <param name="method">The method used to send the data to the resource.
        /// If null, the default is POST for http and STOR for ftp.</param>
        /// <returns>A <see cref="Stream"/> used to write data to the resource.</returns>
        public Stream OpenWrite(Uri address, string method)
        {
            Stream stream;

            if (address == null)
            {
                throw new ArgumentNullException("address");
            }
            if (method == null)
            {
                method = this.MapToDefaultMethod(address);
            }

            WebRequest request = null;
            this.ClearWebClientState();

            try
            {
                this.method = method;
                this.webRequest = this.GetWebRequest(GetAbsoluteUri(address));
                stream = new WebClientWriteStream(request.GetRequestStream(), request, this);
            }
            catch (Exception exception)
            {
                if (((exception is ThreadAbortException) || (exception is StackOverflowException)) || (exception is OutOfMemoryException))
                {
                    throw;
                }
                if (!(exception is WebException) && !(exception is System.Security.SecurityException))
                {
                    exception = new WebException(Properties.Resources.net_webclient, exception);
                }
                AbortRequest(request);
                throw exception;
            }
            catch
            {
                Exception exception2 = new WebException(Properties.Resources.net_webclient, new Exception(Properties.Resources.net_nonClsCompliantException));
                AbortRequest(request);
                throw exception2;
            }
            finally
            {
                this.CompleteWebClientState();
            }
            return stream;
        }

        /// <summary>
        /// Uploads a data buffer to a resource identified by a URI.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the data.</param>
        /// <param name="data">The data buffer to send to the resource.</param>
        /// <returns>A <see cref="Byte"/> array containing the body of the response from the resource.</returns>
        public byte[] UploadData(string address, byte[] data)
        {
            return UploadData(GetAbsoluteUri(address), null, data);
        }

        /// <summary>
        /// Uploads a data buffer to a resource identified by a URI.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the data.</param>
        /// <param name="data">The data buffer to send to the resource.</param>
        /// <returns>A <see cref="Byte"/> array containing the body of the response from the resource.</returns>
        public byte[] UploadData(Uri address, byte[] data)
        {
            return UploadData(address, null, data);
        }

        /// <summary>
        /// Uploads a data buffer to the specified resource, using the specified method.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the data.</param>
        /// <param name="method">The HTTP method used to send the data to the resource.
        /// If null, the default is POST for http and STOR for ftp.</param>
        /// <param name="data">The data buffer to send to the resource.</param>
        /// <returns>A <see cref="Byte"/> array containing the body of the response from the resource.</returns>
        public byte[] UploadData(string address, string method, byte[] data)
        {
            return UploadData(GetAbsoluteUri(address), method, data);
        }

        /// <summary>
        /// Uploads a data buffer to the specified resource, using the specified method.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the data.</param>
        /// <param name="method">The HTTP method used to send the data to the resource.
        /// If null, the default is POST for http and STOR for ftp.</param>
        /// <param name="data">The data buffer to send to the resource.</param>
        /// <returns>A <see cref="Byte"/> array containing the body of the response from the resource.</returns>
        public byte[] UploadData(Uri address, string method, byte[] data)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            Uri absoluteUri = GetAbsoluteUri(address);

            string requestMethod;
            if (string.IsNullOrEmpty(method))
            {
                requestMethod = MapToDefaultMethod(absoluteUri);
            }
            else
            {
                requestMethod = method;
            }
            

            this.ClearWebClientState();


            //create a web request for the uri
            WebRequest request = GetWebRequest(absoluteUri);
            request.Method = requestMethod;

            //set allow write stream buffering for http
            if (absoluteUri.Scheme.StartsWith("http"))
            {
                HttpWebRequest hRequest = (HttpWebRequest)request;
                hRequest.AllowWriteStreamBuffering = true;
            }

            //write the data
            Stream s = request.GetRequestStream();
            s.Write(data, 0, data.Length);
            s.Close();

            byte[] result = null;

            //perform the PUT request
            try
            {
                WebResponse response = GetWebResponse(request);

                
                Stream rs = response.GetResponseStream();

                

                if (rs != null)
                {
                    //get the response
                    result = ReadResponseStream(rs);
                }

                response.Close();
            }
            catch
            {
            }
            finally
            {
                CompleteWebClientState();
            }

            return result;
        }

        /// <summary>
        /// Uploads a data buffer to a resource identified by a URI, using the specified method and identifying token.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the file.
        /// For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page. </param>
        /// <param name="data">The data buffer to send to the resource.</param>
        public void UploadDataAsync(Uri address, byte[] data)
        {
            UploadDataAsync(address, null, data, null);
        }

        /// <summary>
        /// Uploads a data buffer to a resource identified by a URI, using the specified method and identifying token.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the file.
        /// For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page. </param>
        /// <param name="method">The HTTP method used to send the file to the resource.
        /// If null, the default is POST for http and STOR for ftp.</param>
        /// <param name="data">The data buffer to send to the resource.</param>
        public void UploadDataAsync(Uri address, string method, byte[] data)
        {
            UploadDataAsync(address, method, data, null);
        }

        /// <summary>
        /// Uploads a data buffer to a resource identified by a URI, using the specified method and identifying token.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the file.
        /// For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page. </param>
        /// <param name="method">The HTTP method used to send the file to the resource.
        /// If null, the default is POST for http and STOR for ftp.</param>
        /// <param name="data">The data buffer to send to the resource.</param>
        /// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
        public void UploadDataAsync(Uri address, string method, byte[] data, Object userToken)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            this.ClearWebClientState();

            UploadAsyncState uas = new UploadAsyncState();
            uas.request = this.GetWebRequest(GetAbsoluteUri(address));
            if (string.IsNullOrEmpty(method))
            {
                uas.request.Method = MapToDefaultMethod(uas.request.RequestUri);
            }
            else
            {
                uas.request.Method = method;
            }
            //set allow write stream buffering for http
            if (uas.request.RequestUri.Scheme.StartsWith("http"))
            {
                HttpWebRequest hRequest = (HttpWebRequest)uas.request;
                hRequest.AllowWriteStreamBuffering = true;
            }

            webRequest = uas.request;
            uas.userToken = userToken;
            uas.dataValue = data;

            System.Threading.ThreadPool.QueueUserWorkItem(UploadAsyncWorker, uas);

        }

        /// <summary>
        /// Uploads the specified local file to a resource with the specified URI.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the data.</param>
        /// <param name="fileName">The file to send to the resource.</param>
        /// <returns>A <see cref="Byte"/> array containing the body of the response from the resource.</returns>
        /// <example>The following code example uploads the specified file to the specified URI using UploadFile. Any response returned by the server is displayed.
        /// <code>
        /// String uriString = "ftp://yoururihere";
        /// 
        /// // Create a new WebClient instance. 
        /// WebClient myWebClient = new WebClient();
        /// 
        /// string fileName = "\\yourfilepath.ext";
        /// MessageBox.Show("Uploading {0} to {1} ...",fileName,uriString);
        /// // Upload the file to the URI.
        /// // The 'UploadFile(uriString,fileName)' method implicitly uses HTTP POST or FTP STOR method.
        /// byte[] responseArray = myWebClient.UploadFile(uriString,fileName);
        /// // Decode and display the response.
        /// MessageBox.Show("\nResponse Received.The contents of the file uploaded are:\n{0}", System.Text.Encoding.ASCII.GetString(responseArray));
        /// </code></example>
        public byte[] UploadFile(string address, string fileName)
        {
            return UploadFile(GetAbsoluteUri(address), null, fileName);
        }

        /// <summary>
        /// Uploads the specified local file to a resource with the specified URI.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the data.</param>
        /// <param name="fileName">The file to send to the resource.</param>
        /// <returns>A <see cref="Byte"/> array containing the body of the response from the resource.</returns>
        public byte[] UploadFile(Uri address, string fileName)
        {
            return UploadFile(address, null, fileName);
        }

        /// <summary>
        /// Uploads the specified local file to the specified resource, using the specified method.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the data.</param>
        /// <param name="method">The HTTP method used to send the data to the resource.
        /// If null, the default is POST for http and STOR for ftp.</param>
        /// <param name="fileName">The file to send to the resource.</param>
        /// <returns>A <see cref="Byte"/> array containing the body of the response from the resource.</returns>
        public byte[] UploadFile(string address, string method, string fileName)
        {
            return UploadFile(GetAbsoluteUri(address), method, fileName);
        }

        /// <summary>
        /// Uploads the specified local file to the specified resource, using the specified method.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the data.</param>
        /// <param name="method">The HTTP method used to send the data to the resource.
        /// If null, the default is POST for http and STOR for ftp.</param>
        /// <param name="fileName">The file to send to the resource.</param>
        /// <returns>A <see cref="Byte"/> array containing the body of the response from the resource.</returns>
        public byte[] UploadFile(Uri address, string method, string fileName)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            Uri absoluteUri = GetAbsoluteUri(address);

            byte[] result = null;

            this.ClearWebClientState();

            if (absoluteUri.Scheme == "ftp")
            {
                Ftp f = new Ftp(absoluteUri, (NetworkCredential)this.credentials, true);
                try
                {
                    f.PutFile(fileName, absoluteUri.LocalPath);
                }
                finally
                {
                    CompleteWebClientState();
            
                    f.Close();
                }
            }
            else
            {
                string requestMethod;
                if (string.IsNullOrEmpty(method))
                {
                    requestMethod = MapToDefaultMethod(absoluteUri);
                }
                else
                {
                    requestMethod = method;
                }

                //create a web request for the uri
                WebRequest request = GetWebRequest(absoluteUri);
                request.Method = requestMethod;

                //set allow write stream buffering for http
                if (absoluteUri.Scheme.StartsWith("http"))
                {
                    HttpWebRequest hRequest = (HttpWebRequest)request;
                    hRequest.AllowWriteStreamBuffering = true;
                }
                //write the data
                //int fileLength = 0;

                Stream s = request.GetRequestStream();
                FileStream fs = new FileStream(fileName, FileMode.Open);
                fs.CopyTo(s);

                fs.Close();
                s.Close();

                //request.ContentLength = fileLength;

                //perform the PUT request
                try
                {
                    WebResponse response = GetWebResponse(request);
                    if (response != null)
                    {
                        Stream rs = response.GetResponseStream();

                        if (rs != null)
                        {
                            //get the response
                            MemoryStream ms = new MemoryStream();
                            rs.CopyTo(ms);

                            rs.Close();
                            ms.Close();

                            result = ms.ToArray();
                        }

                        response.Close();
                    }
                }
                catch
                {
                }
                finally
                {
                    CompleteWebClientState();
                }
            }

            return result;
        }

        /// <summary>
        /// Uploads the specified local file to the specified resource, using the POST method.
        /// This method does not block the calling thread.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the file.
        /// For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
        /// <param name="fileName">The file to send to the resource.</param>
        public void UploadFileAsync(Uri address, string fileName)
        {
            UploadFileAsync(address, null, fileName, null);
        }

        /// <summary>
        /// Uploads the specified local file to the specified resource, using the POST method.
        /// This method does not block the calling thread.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the file.
        /// For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
        /// <param name="method">The HTTP method used to send the data to the resource.
        /// If null, the default is POST for http and STOR for ftp.</param>
        /// <param name="fileName">The file to send to the resource.</param>
        public void UploadFileAsync(Uri address, string method, string fileName)
        {
            UploadFileAsync(address, method, fileName, null);
        }

        /// <summary>
        /// Uploads the specified local file to the specified resource, using the POST method.
        /// This method does not block the calling thread.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the file.
        /// For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
        /// <param name="method">The HTTP method used to send the data to the resource.
        /// If null, the default is POST for http and STOR for ftp.</param>
        /// <param name="fileName">The file to send to the resource.</param>
        /// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
        public void UploadFileAsync(Uri address, string method, string fileName, object userToken)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            this.ClearWebClientState();

            UploadAsyncState uas = new UploadAsyncState();
            uas.request = this.GetWebRequest(GetAbsoluteUri(address));
            if (string.IsNullOrEmpty(method))
            {
                uas.request.Method = MapToDefaultMethod(uas.request.RequestUri);
            }
            else
            {
                uas.request.Method = method;
            }
            //set allow write stream buffering for http
            if (uas.request.RequestUri.Scheme.StartsWith("http"))
            {
                HttpWebRequest hRequest = (HttpWebRequest)uas.request;
                hRequest.AllowWriteStreamBuffering = true;
            }

            webRequest = uas.request;
            uas.userToken = userToken;
            uas.fileName = fileName;

            System.Threading.ThreadPool.QueueUserWorkItem(UploadAsyncWorker, uas);
        }

        /// <summary>
        /// Uploads the specified string to the specified resource.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the string.
        /// For Http resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
        /// <param name="data">The string to be uploaded.</param>
        /// <returns>A <see cref="String"/> containing the response sent by the server.</returns>
        public string UploadString(string address, string data)
        {
            return UploadString(GetAbsoluteUri(address), null, data);
        }

        /// <summary>
        /// Uploads the specified string to the specified resource.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the string.
        /// For Http resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
        /// <param name="data">The string to be uploaded.</param>
        /// <returns>A <see cref="String"/> containing the response sent by the server.</returns>
        public string UploadString(Uri address, string data)
        {
            return UploadString(address, null, data);
        }

        /// <summary>
        /// Uploads the specified string to the specified resource, using the specified method.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the string.
        /// For Http resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
        /// <param name="method">The HTTP method used to send the data to the resource.
        /// If null, the default is POST for http and STOR for ftp.</param>
        /// <param name="data">The string to be uploaded.</param>
        /// <returns>A <see cref="String"/> containing the response sent by the server.</returns>
        public string UploadString(string address, string method, string data)
        {
            return UploadString(GetAbsoluteUri(address), method, data);
        }

        /// <summary>
        /// Uploads the specified string to the specified resource, using the specified method.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the string.
        /// For Http resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
        /// <param name="method">The HTTP method used to send the data to the resource.
        /// If null, the default is POST for http and STOR for ftp.</param>
        /// <param name="data">The string to be uploaded.</param>
        /// <returns>A <see cref="String"/> containing the response sent by the server.</returns>
        public string UploadString(Uri address, string method, string data)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("data");
            }

            Uri absoluteUri = GetAbsoluteUri(address);

            string requestMethod;
            if (string.IsNullOrEmpty(method))
            {
                requestMethod = MapToDefaultMethod(absoluteUri);
            }
            else
            {
                requestMethod = method;
            }
            

            this.ClearWebClientState();

            //create a web request for the uri
            WebRequest request = GetWebRequest(absoluteUri);
            request.Method = requestMethod;

            //set allow write stream buffering for http
            if (absoluteUri.Scheme.StartsWith("http"))
            {
                HttpWebRequest hRequest = (HttpWebRequest)request;
                hRequest.AllowWriteStreamBuffering = true;
            }

            //write the data
            Stream s = request.GetRequestStream();
            byte[] rawData = this.encoding.GetBytes(data);
            s.Write(rawData, 0, rawData.Length);
            s.Close();

            //get response string
            string result = null;

            //perform the PUT request
            try
            {
                WebResponse response = GetWebResponse(request);
                Stream rs = response.GetResponseStream();

                
                if (rs != null)
                {
                    StreamReader sr = new StreamReader(rs);
                    result = sr.ReadToEnd();
                    sr.Close();
                    rs.Close();
                }

                response.Close();
            }
            finally
            {
                CompleteWebClientState();
            }

            return result;
        }

        /// <summary>
        /// Uploads the specified string to the specified resource.
        /// This method does not block the calling thread.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the file.
        /// For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page. </param>
        /// <param name="data">The string to be uploaded.</param>
        public void UploadStringAsync(Uri address, string data)
        {
            UploadStringAsync(address, null, data, null);
        }

        /// <summary>
        /// Uploads the specified string to the specified resource.
        /// This method does not block the calling thread.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the file.
        /// For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page. </param>
        /// <param name="method">The HTTP method used to send the file to the resource.
        /// If null, the default is POST for http and STOR for ftp.</param>
        /// <param name="data">The string to be uploaded.</param>
        public void UploadStringAsync(Uri address, string method, string data)
        {
            UploadStringAsync(address, method, data, null);
        }

        /// <summary>
        /// Uploads the specified string to the specified resource.
        /// This method does not block the calling thread.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the file.
        /// For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page. </param>
        /// <param name="method">The HTTP method used to send the file to the resource.
        /// If null, the default is POST for http and STOR for ftp.</param>
        /// <param name="data">The string to be uploaded.</param>
        /// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
        public void UploadStringAsync(Uri address, string method, string data, Object userToken)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            this.ClearWebClientState();

            UploadAsyncState uas = new UploadAsyncState();
            uas.request = this.GetWebRequest(GetAbsoluteUri(address));
            if (string.IsNullOrEmpty(method))
            {
                uas.request.Method = MapToDefaultMethod(uas.request.RequestUri);
            }
            else
            {
                uas.request.Method = method;
            }
            //set allow write stream buffering for http
            if (uas.request.RequestUri.Scheme.StartsWith("http"))
            {
                HttpWebRequest hRequest = (HttpWebRequest)uas.request;
                hRequest.AllowWriteStreamBuffering = true;
            }

            webRequest = uas.request;
            uas.userToken = userToken;
            uas.stringValue = data;

            System.Threading.ThreadPool.QueueUserWorkItem(UploadAsyncWorker, uas);

        }

        private void UploadAsyncWorker(object state)
        {
            UploadAsyncState uas = (UploadAsyncState)state;
            Stream s = uas.request.GetRequestStream();

            if (uas.stringValue != null)
            {
                //upload string          
                byte[] stringData = this.Encoding.GetBytes(uas.stringValue);
                s.Write(stringData, 0, stringData.Length);
                s.Close();
            }
            else if (uas.fileName != null)
            {
                FileStream fs = File.OpenRead(uas.fileName);
                fs.CopyTo(s);

                fs.Close();
                s.Close();
            }
            else if (uas.dataValue != null)
            {
                //upload data
                s.Write(uas.dataValue, 0, uas.dataValue.Length);
                s.Close();
            }
            else if (uas.values != null)
            {
                //upload values
                byte[] data = GetFormDataBytes(uas.values);
                s.Write(data, 0, data.Length);
                s.Close();
            }

            //do request
            byte[] result = null;
            string resultString = null;
            Exception ex = null;

            if (!cancelled)
            {
                try
                {
                    WebResponse wr = uas.request.GetResponse();
                    Stream responseStream = wr.GetResponseStream();
                    if (uas.stringValue != null)
                    {
                        StreamReader sr = new StreamReader(responseStream);
                        resultString = sr.ReadToEnd();
                    }
                    else //if (uas.dataValue != null)
                    {   
                        MemoryStream ms = new MemoryStream();
                        responseStream.CopyTo(ms);

                        result = ms.ToArray();
                    }

                    responseStream.Close();
                    wr.Close();

                }
                catch (Exception exception)
                {
                    ex = exception;
                }
            }
            if (uas.stringValue != null)
            {
                OnUploadStringCompleted(new UploadStringCompletedEventArgs(resultString, ex, this.cancelled, uas.userToken));
            }
            else if (uas.fileName != null)
            {
                OnUploadFileCompleted(new UploadFileCompletedEventArgs(result, ex, this.cancelled, uas.userToken));
            }
            else if (uas.dataValue != null)
            {
                OnUploadDataCompleted(new UploadDataCompletedEventArgs(result, ex, this.cancelled, uas.userToken));
            }
            else if (uas.values != null)
            {
                OnUploadValuesCompleted(new UploadValuesCompletedEventArgs(result, ex, this.cancelled, uas.userToken));
            }
        }

        /// <summary>
        /// Uploads the specified name/value collection to the resource identified by the specified URI.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the collection.</param>
        /// <param name="data">The <see cref="NameValueCollection"/> to send to the resource.</param>
        /// <returns>A <see cref="Byte"/> array containing the body of the response from the resource.</returns>
        public byte[] UploadValues(string address, NameValueCollection data)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }
            return UploadValues(GetAbsoluteUri(address), null, data);
        }

        /// <summary>
        /// Uploads the specified name/value collection to the resource identified by the specified URI.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the collection.</param>
        /// <param name="data">The <see cref="NameValueCollection"/> to send to the resource.</param>
        /// <returns>A <see cref="Byte"/> array containing the body of the response from the resource.</returns>
        public byte[] UploadValues(Uri address, NameValueCollection data)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }

            return UploadValues(address, null, data);
        }

        /// <summary>
        /// Uploads the specified name/value collection to the resource identified by the specified URI, using the specified method.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the collection.</param>
        /// <param name="method">The HTTP method used to send the file to the resource.
        /// If null, the default is POST for http and STOR for ftp.</param>
        /// <param name="data">The <see cref="NameValueCollection"/> to send to the resource.</param>
        /// <returns>A <see cref="Byte"/> array containing the body of the response from the resource.</returns>
        public byte[] UploadValues(string address, string method, NameValueCollection data)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }
            return UploadValues(GetAbsoluteUri(address), method, data);
        }

        /// <summary>
        /// Uploads the specified name/value collection to the resource identified by the specified URI, using the specified method.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the collection.</param>
        /// <param name="method">The HTTP method used to send the file to the resource.
        /// If null, the default is POST for http and STOR for ftp.</param>
        /// <param name="data">The <see cref="NameValueCollection"/> to send to the resource.</param>
        /// <returns>A <see cref="Byte"/> array containing the body of the response from the resource.</returns>
        public byte[] UploadValues(Uri address, string method, NameValueCollection data)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            Uri absoluteUri = GetAbsoluteUri(address);

            string requestMethod;
            if (string.IsNullOrEmpty(method))
            {
                requestMethod = MapToDefaultMethod(absoluteUri);
            }
            else
            {
                requestMethod = method;
            }
            

            this.ClearWebClientState();

            //create a web request for the uri
            WebRequest request = GetWebRequest(absoluteUri);
            request.Method = requestMethod;
            request.ContentType = "application/x-www-form-urlencoded";

            //set allow write stream buffering for http
            if (absoluteUri.Scheme.StartsWith("http"))
            {
                HttpWebRequest hRequest = (HttpWebRequest)request;
                hRequest.AllowWriteStreamBuffering = true;
            }

            //format the data
            byte[] formData = GetFormDataBytes(data);

            //write the data
            Stream s = request.GetRequestStream();
            s.Write(formData, 0, formData.Length);
            s.Close();

            byte[] result = null;

            //perform the PUT request
            try
            {
                WebResponse response = GetWebResponse(request);


                Stream rs = response.GetResponseStream();



                if (rs != null)
                {
                    //get the response
                    result = ReadResponseStream(rs);
                }

                response.Close();
            }
            catch
            {
            }
            finally
            {
                CompleteWebClientState();
            }

            return result;
        }

        private static byte[] GetFormDataBytes(NameValueCollection data)
        {
            //format the data
            StringBuilder queryBuilder = new StringBuilder();

            // Build the query
            for (int i = 0; i < data.Count; i++)
            {
                queryBuilder.Append(data.Keys[i]);
                queryBuilder.Append("=");
                queryBuilder.Append(Uri.EscapeDataString(data[i]));//InTheHand.Web.HttpUtility.UrlEncode(data[i]));
                queryBuilder.Append("&");
            }
            queryBuilder.Remove(queryBuilder.Length - 1, 1);

            return System.Text.Encoding.ASCII.GetBytes(queryBuilder.ToString());
        }

        /// <summary>
        /// Uploads the data in the specified name/value collection to the resource identified by the specified URI, using the specified method.
        /// This method does not block the calling thread, and allows the caller to pass an object to the method that is invoked when the operation completes.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the collection.
        /// This URI must identify a resource that can accept a request sent with the method method.</param>
        /// <param name="data">The <see cref="NameValueCollection"/> to send to the resource.</param>
        public void UploadValuesAsync(Uri address, NameValueCollection data)
        {
            UploadValuesAsync(address, null, data, null);
        }

        /// <summary>
        /// Uploads the data in the specified name/value collection to the resource identified by the specified URI, using the specified method.
        /// This method does not block the calling thread, and allows the caller to pass an object to the method that is invoked when the operation completes.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the collection.
        /// This URI must identify a resource that can accept a request sent with the method method.</param>
        /// <param name="method">The HTTP method used to send the string to the resource.
        /// If null, the default is POST for http and STOR for ftp.</param>
        /// <param name="data">The <see cref="NameValueCollection"/> to send to the resource.</param>
        public void UploadValuesAsync(Uri address, string method, NameValueCollection data)
        {
            UploadValuesAsync(address, method, data, null);
        }

        /// <summary>
        /// Uploads the data in the specified name/value collection to the resource identified by the specified URI, using the specified method.
        /// This method does not block the calling thread, and allows the caller to pass an object to the method that is invoked when the operation completes.
        /// </summary>
        /// <param name="address">The URI of the resource to receive the collection.
        /// This URI must identify a resource that can accept a request sent with the method method.</param>
        /// <param name="method">The HTTP method used to send the string to the resource.
        /// If null, the default is POST for http and STOR for ftp.</param>
        /// <param name="data">The <see cref="NameValueCollection"/> to send to the resource.</param>
        /// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
        public void UploadValuesAsync(Uri address, string method, NameValueCollection data, object userToken)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            this.ClearWebClientState();

            UploadAsyncState uas = new UploadAsyncState();
            uas.request = this.GetWebRequest(GetAbsoluteUri(address));
            if (string.IsNullOrEmpty(method))
            {
                uas.request.Method = MapToDefaultMethod(uas.request.RequestUri);
            }
            else
            {
                uas.request.Method = method;
            }

            //set to form encoded type
            uas.request.ContentType = "application/x-www-form-urlencoded";

            //set allow write stream buffering for http
            if (uas.request.RequestUri.Scheme.StartsWith("http"))
            {
                HttpWebRequest hRequest = (HttpWebRequest)uas.request;
                hRequest.AllowWriteStreamBuffering = true;
            }

            webRequest = uas.request;
            uas.userToken = userToken;
            uas.values = data;

            System.Threading.ThreadPool.QueueUserWorkItem(UploadAsyncWorker, uas);
        }

        private byte[] ReadResponseStream(Stream s)
        {
            //get the response
            MemoryStream ms = new MemoryStream();
            s.CopyTo(ms);

            s.Close();
            ms.Close();

            return ms.ToArray();
        }

        #region Download Progress Changed
        /// <summary>
        /// Raises the <see cref="DownloadProgressChanged"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDownloadProgressChanged(DownloadProgressChangedEventArgs e)
        {
            if (this.DownloadProgressChanged != null)
            {
                this.DownloadProgressChanged(this, e);
            }
        }
        /// <summary>
        /// Occurs when an asynchronous download operation successfully transfers some or all of the data.
        /// </summary>
        public event DownloadProgressChangedEventHandler DownloadProgressChanged;
        #endregion

        #region Upload Progress Changed
        /// <summary>
        /// Raises the <see cref="UploadProgressChanged"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnUploadProgressChanged(UploadProgressChangedEventArgs e)
        {
            if (this.UploadProgressChanged != null)
            {
                this.UploadProgressChanged(this, e);
            }
        }
        /// <summary>
        /// Occurs when an asynchronous upload operation successfully transfers some or all of the data.
        /// </summary>
        public event UploadProgressChangedEventHandler UploadProgressChanged;
        #endregion


        #region Upload Data Completed
        /// <summary>
        /// Raises the <see cref="UploadDataCompleted"/> event.
        /// </summary>
        /// <param name="e">An <see cref="UploadDataCompletedEventArgs"/> object containing event data.</param>
        protected virtual void OnUploadDataCompleted(UploadDataCompletedEventArgs e)
        {
            if (this.UploadDataCompleted != null)
            {
                this.UploadDataCompleted(this, e);
            }
        }
        /// <summary>
        /// Occurs when an asynchronous data-upload operation completes.
        /// </summary>
        public event UploadDataCompletedEventHandler UploadDataCompleted;
        #endregion

        #region Upload File Completed
        /// <summary>
        /// Raises the <see cref="UploadFileCompleted"/> event.
        /// </summary>
        /// <param name="e">An <see cref="UploadFileCompletedEventArgs"/> object containing event data.</param>
        protected virtual void OnUploadFileCompleted(UploadFileCompletedEventArgs e)
        {
            if (this.UploadFileCompleted != null)
            {
                this.UploadFileCompleted(this, e);
            }
        }
        /// <summary>
        /// Occurs when an asynchronous file-upload operation completes.
        /// </summary>
        public event UploadFileCompletedEventHandler UploadFileCompleted;
        #endregion

        #region Upload String Completed
        /// <summary>
        /// Raises the <see cref="UploadStringCompleted"/> event.
        /// </summary>
        /// <param name="e">An <see cref="UploadStringCompletedEventArgs"/> object containing event data.</param>
        protected virtual void OnUploadStringCompleted(UploadStringCompletedEventArgs e)
        {
            if (this.UploadStringCompleted != null)
            {
                this.UploadStringCompleted(this, e);
            }
        }
        /// <summary>
        /// Occurs when an asynchronous string-upload operation completes.
        /// </summary>
        public event UploadStringCompletedEventHandler UploadStringCompleted;
        #endregion

        #region Upload Values Completed
        /// <summary>
        /// Raises the <see cref="UploadValuesCompleted"/> event.
        /// </summary>
        /// <param name="e">A <see cref="UploadValuesCompletedEventArgs"/> object containing event data.</param>
        protected virtual void OnUploadValuesCompleted(UploadValuesCompletedEventArgs e)
        {
            if (this.UploadValuesCompleted != null)
            {
                this.UploadValuesCompleted(this, e);
            }
        }

        /// <summary>
        /// Occurs when an asynchronous upload of a name/value collection completes.
        /// </summary>
        public event UploadValuesCompletedEventHandler UploadValuesCompleted;
        #endregion


        #region Write Stream
        private class WebClientWriteStream : Stream
        {
            private WebRequest request;
            private Stream stream;
            private WebClient webClient;

            public WebClientWriteStream(Stream stream, WebRequest request, WebClient webClient)
            {
                this.request = request;
                this.stream = stream;
                this.webClient = webClient;
            }

            public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
            {
                return this.stream.BeginRead(buffer, offset, size, callback, state);
            }

            public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
            {
                return this.stream.BeginWrite(buffer, offset, size, callback, state);
            }

            protected override void Dispose(bool disposing)
            {
                try
                {
                    if (disposing)
                    {
                        this.stream.Close();
                        this.webClient.GetWebResponse(this.request).Close();
                    }
                }
                finally
                {
                    this.webClient.CompleteWebClientState();
                    base.Dispose(disposing);
                }
            }

            public override int EndRead(IAsyncResult result)
            {
                return this.stream.EndRead(result);
            }

            public override void EndWrite(IAsyncResult result)
            {
                this.stream.EndWrite(result);
            }

            public override void Flush()
            {
                this.stream.Flush();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return this.stream.Read(buffer, offset, count);
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return this.stream.Seek(offset, origin);
            }

            public override void SetLength(long value)
            {
                this.stream.SetLength(value);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                this.stream.Write(buffer, offset, count);
            }

            public override bool CanRead
            {
                get
                {
                    return this.stream.CanRead;
                }
            }

            public override bool CanSeek
            {
                get
                {
                    return this.stream.CanSeek;
                }
            }

            public override bool CanTimeout
            {
                get
                {
                    return this.stream.CanTimeout;
                }
            }

            public override bool CanWrite
            {
                get
                {
                    return this.stream.CanWrite;
                }
            }

            public override long Length
            {
                get
                {
                    return this.stream.Length;
                }
            }

            public override long Position
            {
                get
                {
                    return this.stream.Position;
                }
                set
                {
                    this.stream.Position = value;
                }
            }

            public override int ReadTimeout
            {
                get
                {
                    return this.stream.ReadTimeout;
                }
                set
                {
                    this.stream.ReadTimeout = value;
                }
            }

            public override int WriteTimeout
            {
                get
                {
                    return this.stream.WriteTimeout;
                }
                set
                {
                    this.stream.WriteTimeout = value;
                }
            }
        }

 

        #endregion
    }
}