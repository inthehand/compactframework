// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FtpWebRequest.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace InTheHand.Net
{
    /// <summary>
    /// Implements a File Transfer Protocol (FTP) client.
    /// </summary>
    /// <remarks>
    /// <para>Equivalent to System.Net.FtpWebRequest</para>
    /// <para>You must call the static method <see cref="RegisterPrefix"/> before this class can be used with <see cref="WebRequest.Create(string)"/>. You only need to call <see cref="RegisterPrefix"/> once in your application.</para>
    /// To obtain an instance of <see cref="FtpWebRequest"/>, use the <see cref="WebRequest.Create(string)"/> method after calling <see cref="RegisterPrefix"/>.
    /// You can also use the <see cref="WebClient"/> class to upload and download information from an FTP server.
    /// Using either of these approaches, when you specify a network resource that uses the FTP scheme (for example, "ftp://contoso.com") the <see cref="FtpWebRequest"/> class provides the ability to programmatically interact with FTP servers.
    /// The URI may be relative or absolute.
    /// If the URI is of the form "ftp://contoso.com/%2fpath" (%2f is an escaped '/'), then the URI is absolute, and the current directory is /path.
    /// If, however, the URI is of the form "ftp://contoso.com/path", first the .NET Framework logs into the FTP server (using the user name and password set by the <see cref="Credentials"/> property), then the current directory is set to &lt;UserLoginDirectory&gt;/path.
    /// You must have a valid user name and password for the server or the server must allow anonymous logon.
    /// You can specify the credentials used to connect to the server by setting the <see cref="Credentials"/> property or you can include them in the <see cref="Uri.UserInfo"/> portion of the URI passed to the <see cref="WebRequest.Create(string)"/> method.
    /// If you include <see cref="Uri.UserInfo"/> information in the URI, the <see cref="Credentials"/> property is set to a new network credential with the specified user name and password information.
    /// </remarks>
    /// <example>The following code example demonstrates deleting a file from an FTP server.
    /// <code lang="cs">
    /// public static bool DeleteFileOnServer(Uri serverUri)
    /// {
    ///     FtpWebRequest.RegisterPrefix();
    ///     // The serverUri parameter should use the ftp:// scheme.
    ///     // It contains the name of the server file that is to be deleted.
    ///     // Example: ftp://contoso.com/someFile.txt.
    ///     // 
    ///     
    ///     if (serverUri.Scheme != Uri.UriSchemeFtp)
    ///     {
    ///         return false;
    ///     }
    ///     // Get the object used to communicate with the server.
    ///     FtpWebRequest request = (FtpWebRequest)WebRequest.Create(serverUri);
    ///     request.Method = WebRequestMethods.Ftp.DeleteFile;
    ///     
    ///     FtpWebResponse response = (FtpWebResponse) request.GetResponse();
    ///     Console.WriteLine("Delete status: {0}",response.StatusDescription);  
    ///     response.Close();
    ///     return true;
    /// }</code></example>
    /// <example>The following code example demonstrates downloading a file from an FTP server by using the <see cref="WebClient"/> class.
    /// <code lang="cs">
    /// public static bool DisplayFileFromServer(Uri serverUri)
    /// {
    ///     // The serverUri parameter should start with the ftp:// scheme.
    ///     if (serverUri.Scheme != Uri.UriSchemeFtp)
    ///     {
    ///         return false;
    ///     }
    ///     // Get the object used to communicate with the server.
    ///     WebClient request = new WebClient();
    ///     // This example assumes the FTP site uses anonymous logon.
    ///     request.Credentials = new NetworkCredential ("anonymous","janeDoe@contoso.com");
    ///     try 
    ///     {
    ///         byte [] newFileData = request.DownloadData(serverUri.ToString());
    ///         string fileString = System.Text.Encoding.UTF8.GetString(newFileData);
    ///         Console.WriteLine(fileString);
    ///     }
    ///     catch (WebException e)
    ///     {
    ///         Console.WriteLine(e.ToString());
    ///     }
    ///     return true;
    /// }</code></example>
    /// <example>The following code example demonstrates using asynchronous operations to upload a file to an FTP server.
    /// <code lang="cs">
    /// using System;
    /// using System.Net;
    /// using InTheHand.Net;
    /// using System.Threading;
    /// using System.IO;
    /// using InTheHand.IO;
    /// 
    /// namespace Examples.InTheHand.Net
    /// {
    ///     public class FtpState
    ///     {
    ///         private ManualResetEvent wait;
    ///         private FtpWebRequest request;
    ///         private string fileName;
    ///         private Exception operationException = null;
    ///         string status;
    ///         
    ///         public FtpState()
    ///         {
    ///             wait = new ManualResetEvent(false);
    ///         }
    ///         
    ///         public ManualResetEvent OperationComplete
    ///         {
    ///             get {return wait;}
    ///         }
    ///         
    ///         public FtpWebRequest Request
    ///         {
    ///             get {return request;}
    ///             set {request = value;}
    ///         }
    ///         
    ///         public string FileName
    ///         {
    ///             get {return fileName;}
    ///             set {fileName = value;}
    ///         }
    ///         
    ///         public Exception OperationException
    ///         {
    ///             get {return operationException;}
    ///             set {operationException = value;}
    ///         }
    ///         
    ///         public string StatusDescription
    ///         {
    ///             get {return status;}
    ///             set {status = value;}
    ///         }
    ///     }
    ///     
    ///     public class AsynchronousFtpUpLoader
    ///     {  
    ///         // Command line arguments are two strings:
    ///         // 1. The url that is the name of the file being uploaded to the server.
    ///         // 2. The name of the file on the local machine.
    ///         //
    ///         public static void Main(string[] args)
    ///         {
    ///             FtpWebRequest.RegisterPrefix();
    ///             // Create a Uri instance with the specified URI string.
    ///             // If the URI is not correctly formed, the Uri constructor
    ///             // will throw an exception.
    ///             ManualResetEvent waitObject;
    ///         
    ///             Uri target = new Uri (args[0]);
    ///             string fileName = args[1];
    ///             FtpState state = new FtpState();
    ///             FtpWebRequest request = (FtpWebRequest)WebRequest.Create(target);
    ///             request.Method = WebRequestMethods.Ftp.UploadFile;
    ///         
    ///             // This example uses anonymous logon.
    ///             // The request is anonymous by default; the credential does not have to be specified. 
    ///             // The example specifies the credential only to
    ///             // control how actions are logged on the server.
    ///         
    ///             request.Credentials = new NetworkCredential ("anonymous","janeDoe@contoso.com");
    ///         
    ///             // Store the request in the object that we pass into the
    ///             // asynchronous operations.
    ///             state.Request = request;
    ///             state.FileName = fileName;
    ///         
    ///             // Get the event to wait on.
    ///             waitObject = state.OperationComplete;
    ///         
    ///             // Asynchronously get the stream for the file contents.
    ///             request.BeginGetRequestStream(
    ///                 new AsyncCallback (EndGetStreamCallback), 
    ///                 state
    ///                 );
    ///             
    ///             // Block the current thread until all operations are complete.
    ///             waitObject.WaitOne();
    ///             
    ///             // The operations either completed or threw an exception.
    ///             if (state.OperationException != null)
    ///             {
    ///                 throw state.OperationException;
    ///             }
    ///             else
    ///             {
    ///                 Console.WriteLine("The operation completed - {0}", state.StatusDescription);
    ///             }
    ///         }
    ///         
    ///         private static void EndGetStreamCallback(IAsyncResult ar)
    ///         {
    ///             FtpState state = (FtpState) ar.AsyncState;
    ///             
    ///             Stream requestStream = null;
    ///             // End the asynchronous call to get the request stream.
    ///             try
    ///             {
    ///                 requestStream = state.Request.EndGetRequestStream(ar);
    ///                 // Copy the file contents to the request stream.
    ///                 FileStream stream = File.OpenRead(state.FileName);
    ///                 stream.CopyTo(requestStream);
    ///                 stream.Close();
    ///                 
    ///                 Console.WriteLine ("Writing {0} bytes to the stream.", stream.Length);
    ///                 // IMPORTANT: Close the request stream before sending the request.
    ///                 requestStream.Close();
    ///                 // Asynchronously get the response to the upload request.
    ///                 state.Request.BeginGetResponse(
    ///                     new AsyncCallback (EndGetResponseCallback), 
    ///                     state
    ///                     );
    ///             } 
    ///             // Return exceptions to the main application thread.
    ///             catch (Exception e)
    ///             {
    ///                 Console.WriteLine("Could not get the request stream.");
    ///                 state.OperationException = e;
    ///                 state.OperationComplete.Set();
    ///                 return;
    ///             }
    ///             
    ///         }
    ///             
    ///         // The EndGetResponseCallback method  
    ///         // completes a call to BeginGetResponse.
    ///         private static void EndGetResponseCallback(IAsyncResult ar)
    ///         {
    ///             FtpState state = (FtpState) ar.AsyncState;
    ///             FtpWebResponse response = null;
    ///             try 
    ///             {
    ///                 response = (FtpWebResponse) state.Request.EndGetResponse(ar);
    ///                 response.Close();
    ///                 state.StatusDescription = response.StatusDescription;
    ///                 // Signal the main application thread that 
    ///                 // the operation is complete.
    ///                 state.OperationComplete.Set();
    ///             }
    ///             // Return exceptions to the main application thread.
    ///             catch (Exception e)
    ///             {
    ///                 Console.WriteLine ("Error getting response.");
    ///                 state.OperationException = e;
    ///                 state.OperationComplete.Set();
    ///             }
    ///         }
    ///     }
    /// }
    /// </code></example>
    public class FtpWebRequest : WebRequest
    {
        
        #region Static

        internal static bool registered = false;

        //register with ftp prefix
        static FtpWebRequest()
        {
            RegisterPrefix();
        }

        /// <summary>
        /// Register this class with the <see cref="WebRequest"/> class.
        /// </summary>
        /// <remarks>Once this method is called once you can use <see cref="WebRequest.Create(string)"/> to create new <see cref="FtpWebRequest"/> instances.</remarks>
        public static void RegisterPrefix()
        {
            if (!registered)
            {
                registered = WebRequest.RegisterPrefix("ftp", new FtpWebRequestCreate());
            }
        }
        
        #endregion

        #region Constructor
        /*
        /// <summary>
        /// Initializes a new <see cref="FtpWebRequest"/> instance for the specified URI.
        /// </summary>
        /// <param name="uri"></param>
        public FtpWebRequest(string uri)
        {
            this.uri = new Uri(uri);
        }*/
        /// <summary>
        /// Initializes a new <see cref="FtpWebRequest"/> instance for the specified URI.
        /// </summary>
        /// <param name="uri"></param>
        internal FtpWebRequest(Uri uri)
        {
            this.uri = uri;
        }
        #endregion


        #region Content Length
        /// <summary>
        /// This property supports the .NET Compact Framework infrastructure and is not intended to be used directly from your code. 
        /// Gets or sets a value that is ignored by the <see cref="FtpWebRequest"/> class.
        /// </summary>
        /// <remarks>The <see cref="ContentLength"/> property is provided only for compatibility with other implementations of the <see cref="WebRequest"/> and <see cref="WebResponse"/> classes.
        /// There is no reason to use <see cref="ContentLength"/>.</remarks>
        public override long ContentLength
        {
            get
            {
                return 0;
            }

            set
            {
            }
        }
        #endregion

        #region Content Type
        /// <summary>
        /// This property supports the .NET Compact Framework infrastructure and is not intended to be used directly from your code. 
        /// Always throws a <see cref="NotSupportedException"/>. 
        /// </summary>
        /// <remarks>The <see cref="ContentType"/> property is provided only for compatibility with other implementations of the <see cref="WebRequest"/> and <see cref="WebResponse"/> classes.
        /// There is no reason to use <see cref="ContentType"/>.</remarks>
        public override string ContentType
        {
            get
            {
                throw new NotSupportedException();
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region Credentials
        private NetworkCredential credentials = null;
        /// <summary>
        /// Gets or sets the credentials used to communicate with the FTP server.
        /// </summary>
        public override ICredentials Credentials
        {
            get
            {
                return credentials;
            }

            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException();
                }

                /*if (value == CredentialCache.DefaultCredentials)
                {
                    credentials = new NetworkCredential("anonymous", "anonymous@");
                    return;
                }*/

                if(value.GetType() != typeof(NetworkCredential))
                {
                    throw new ArgumentException();
                }
                credentials = (NetworkCredential)value;
            }
        }

        #endregion

        private WebHeaderCollection headers = new WebHeaderCollection();
        /// <summary>
        /// This property supports the .NET Compact Framework infrastructure and is not intended to be used directly from your code.
        /// Gets an empty <see cref="WebHeaderCollection"/> object.
        /// </summary>
        /// <value>An empty <see cref="WebHeaderCollection"/> object.</value>
        /// <remarks>The <see cref="Headers"/> property is provided only for compatibility with other implementations of the <see cref="WebRequest"/> and <see cref="WebResponse"/> classes.
        /// There is no reason to use <see cref="Headers"/>.</remarks>
        public override WebHeaderCollection Headers
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

        private string method = WebRequestMethods.Ftp.DownloadFile;
        /// <summary>
        /// Gets or sets the command to send to the FTP server. 
        /// </summary>
        /// <value>A <see cref="String"/> value that contains the FTP command to send to the server.
        /// The default value is <see cref="WebRequestMethods.Ftp.DownloadFile"/>.</value>
        /// <remarks>The Method property determines which command is sent to the server.
        /// You set the Method by using the strings defined in the public field members of the <see cref="WebRequestMethods.Ftp"/> class.
        /// Note that the strings defined in the <see cref="WebRequestMethods.Ftp"/> class are the only supported options for the Method property.
        /// Setting the Method property to any other value will result in an <see cref="ArgumentException"/> exception.</remarks>
        /// <exception cref="InvalidOperationException">A new value was specified for this property for a request that is already in progress.</exception>
        /// <exception cref="ArgumentException">The method is invalid.
        /// <para>- or -</para>
        /// <para>The method is not supported.</para>
        /// <para>- or -</para>
        /// <para>Multiple methods were specified.</para>
        /// </exception>
        public override string Method
        {
            get
            {
                return method;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException(Properties.Resources.net_invalid_method_name, "value");
                }

                if (response != null)
                {
                    throw new InvalidOperationException(Properties.Resources.net_reqsubmitted);
                }
                //validate method
                bool valid = false;
                foreach (System.Reflection.FieldInfo fi in typeof(WebRequestMethods.Ftp).GetFields())
                {
                    if (value == fi.GetValue(null).ToString())
                    {
                        valid = true;
                        break;
                    }
                }

                if (!valid)
                {
                    throw new ArgumentException(Properties.Resources.net_unsupported_method, "value");
                }
                method = value;
            }
        }

        /// <summary>
        /// This API supports the .NET Compact Framework infrastructure and is not intended to be used directly from your code.
        /// </summary>
        public override bool PreAuthenticate
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override IWebProxy Proxy
        {
            get
            {
                return null;
                //return base.Proxy;
            }
            set
            {
                //base.Proxy = value;
            }
        }

        private string renameTo;
        /// <summary>
        /// Gets or sets the new name of a file being renamed.
        /// </summary>
        public string RenameTo
        {
            get
            {
                return renameTo;
            }
            set
            {
                renameTo = value;
            }
        }


        private Uri uri;
        /// <summary>
        /// Gets the URI requested by this instance.
        /// </summary>
        public override Uri RequestUri
        {
            get
            {
                return uri;
            }
        }

        private int timeout = System.Threading.Timeout.Infinite;
        /// <summary>
        /// Not supported for FTP. Gets or sets the number of milliseconds to wait for a request.
        /// </summary>
        /// <value>An <see cref="Int32"/> value that contains the number of milliseconds to wait before a request times out.
        /// The default value is Infinite.</value>
        public override int Timeout
        {
            get
            {
                return timeout;
            }

            set
            {
            }
        }

        private bool useBinary = true;
        /// <summary>
        /// Gets or sets a Boolean value that specifies the data type for file transfers.
        /// </summary>
        /// <value>true to indicate to the server that the data to be transferred is binary; false to indicate that the data is text.
        /// The default value is true.</value>
        /// <exception cref="InvalidOperationException">A new value was specified for this property for a request that is already in progress.</exception>
        public bool UseBinary
        {
            get
            {
                return useBinary;
            }

            set
            {
                if (response != null)
                {
                    throw new InvalidOperationException();
                }
                useBinary = value;
            }
        }

        private bool usePassive = true;
        /// <summary>
        /// Gets or sets the behavior of a client application's data transfer process.
        /// </summary>
        /// <value>false if the client application's data transfer process listens for a connection on the data port; otherwise, true if the client should initiate a connection on the data port.
        /// The default value is true.</value>
        /// <exception cref="InvalidOperationException">A new value was specified for this property for a request that is already in progress.</exception>
        public bool UsePassive
        {
            get
            {
                return usePassive;
            }

            set
            {
                if (response != null)
                {
                    throw new InvalidOperationException();
                }
                usePassive = value;
            }
        }

        private MemoryStream requestStream;

        /// <summary>
        /// Retrieves the stream used to upload data to an FTP server.
        /// </summary>
        /// <returns>A writable <see cref="Stream"/> instance used to store data to be sent to the server by the current request.</returns>
        /// <exception cref="ProtocolViolationException">The <see cref="Method"/> property is not set to <see cref="WebRequestMethods.Ftp.UploadFile"/> or <see cref="WebRequestMethods.Ftp.AppendFile"/>.</exception>
        public override Stream GetRequestStream()
        {
            if ((Method != WebRequestMethods.Ftp.AppendFile) && (Method != WebRequestMethods.Ftp.UploadFile) && (Method != WebRequestMethods.Ftp.UploadFileWithUniqueName))
            {
                throw new ProtocolViolationException();
            }
            if (requestStream == null)
            {
                requestStream = new MemoryStream();
            }
            return requestStream;
        }

        public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
        {
            GetRequestStream();
            IAsyncResult asyncResult = new DummyAsyncResult(callback, state);
            callback(asyncResult);
            return asyncResult;
        }

        public override Stream EndGetRequestStream(IAsyncResult asyncResult)
        {
            if (asyncResult is DummyAsyncResult)
            {
                return this.requestStream;
            }

            throw new InvalidOperationException();
        }

        private sealed class DummyAsyncResult : IAsyncResult
        {
            private readonly AsyncCallback callback;
            private readonly object asyncState;
            private readonly ManualResetEvent waitHandle;

            internal DummyAsyncResult(AsyncCallback callback, object state)
            {
                this.callback = callback;
                this.asyncState = state;
                waitHandle = new ManualResetEvent(false);
                waitHandle.Set();
            }

            #region IAsyncResult Members

            object IAsyncResult.AsyncState
            {
                get { return asyncState; }
            }

            System.Threading.WaitHandle IAsyncResult.AsyncWaitHandle
            {
                get { return waitHandle; }
            }

            bool IAsyncResult.CompletedSynchronously
            {
                get { return true; }
            }

            bool IAsyncResult.IsCompleted
            {
                get { return true; }
            }

            #endregion
        }

        #region Response
        private FtpWebResponse response;

        /// <summary>
        /// Returns the FTP server response.
        /// </summary>
        /// <returns>A <see cref="WebResponse"/> reference that contains an <see cref="FtpWebResponse"/> instance.
        /// This object contains the FTP server's response to the request.</returns>
        public override WebResponse GetResponse()
        {
            if(response == null)
            {
                response = new FtpWebResponse(this);
            }
            
            return response;
        }
        #endregion

        /// <summary>
        /// Begins sending a request and receiving a response from an FTP server asynchronously.
        /// </summary>
        /// <param name="callback">An <see cref="AsyncCallback"/> delegate that references the method to invoke when the operation is complete.</param>
        /// <param name="state">A user-defined object that contains information about the operation.
        /// This object is passed to the callback delegate when the operation completes.</param>
        /// <returns>An <see cref="IAsyncResult"/> instance that indicates the status of the operation.</returns>
        public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
        {
            if (gettingResponse)
            {
                throw new InvalidOperationException();
            }

            gettingResponse = true;
            asyncResult = new FtpAsyncResult(this, state, callback);

            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(GetResponseThread), asyncResult);
            //System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(GetResponseThread));
            //t.IsBackground = true;
            //t.Start();
            return asyncResult;
        }
        /// <summary>
        /// Ends a pending asynchronous operation started with <see cref="BeginGetResponse"/>.
        /// </summary>
        /// <param name="asyncResult">The <see cref="IAsyncResult"/> that was returned when the operation started.</param>
        /// <returns>A <see cref="WebResponse"/> reference that contains an <see cref="FtpWebResponse"/> instance.
        /// This object contains the FTP server's response to the request.</returns>
        public override WebResponse EndGetResponse(IAsyncResult asyncResult)
        {
            if (asyncResult == null)
            {
                throw new ArgumentNullException("asyncResult");
            }

            FtpAsyncResult far = (FtpAsyncResult)asyncResult;

            if (far.IsCompleted)
            {
                if (response == null)
                {
                    throw new WebException(Properties.Resources.net_could_not_connect, WebExceptionStatus.ConnectFailure);
                }
                return this.response;
            }

            //wait till operation finished
            far.AsyncWaitHandle.WaitOne();
            //mark as synchronous completion
            far.completedSynchronously = true;
            far.isCompleted = true;

            if (response == null)
            {
                throw new WebException(Properties.Resources.net_could_not_connect, WebExceptionStatus.ConnectFailure);
            }
            return this.response;

        }

        private FtpAsyncResult asyncResult;

        private void GetResponseThread(object state)
        {
            try
            {
                response = new FtpWebResponse(this);
            }
            catch
            {
                response = null;
            }

            asyncResult.isCompleted = true;
            ((InTheHand.Threading.EventWaitHandle)asyncResult.AsyncWaitHandle).Set();
            asyncResult.callback(asyncResult);
        }

        private bool gettingResponse = false;

    }

    internal class FtpAsyncResult : IAsyncResult
    {
        private object asyncObject;
        private object asyncState;
        internal AsyncCallback callback;
        private InTheHand.Threading.EventWaitHandle asyncWaitHandle;
        internal bool completedSynchronously;
        internal bool isCompleted;

        internal FtpAsyncResult(object obj, object state, AsyncCallback callback)
        {
            asyncObject = obj;
            asyncState = state;
            this.callback = callback;
            asyncWaitHandle = new InTheHand.Threading.EventWaitHandle(false, System.Threading.EventResetMode.ManualReset);
        }
        #region IAsyncResult Members

        public object AsyncState
        {
            get { return asyncState; }
        }

        public System.Threading.WaitHandle AsyncWaitHandle
        {
            get { return asyncWaitHandle; }
        }

        public bool CompletedSynchronously
        {
            get { return completedSynchronously; }
        }

        public bool IsCompleted
        {
            get { return isCompleted; }
        }

        #endregion
    }

    internal class FtpWebRequestCreate : System.Net.IWebRequestCreate
    {
        public WebRequest Create(Uri uri)
        {
            if (uri.Scheme.ToLower() == "ftp")
            {
                return new FtpWebRequest(uri);
            }
            return null;
        }
    }
}