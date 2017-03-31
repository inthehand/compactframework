// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileWebRequest.cs" company="In The Hand Ltd">
// Copyright (c) 2008-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace InTheHand.Net
{
    /// <summary>
    /// Provides a file system implementation of the <see cref="WebRequest"/> class.
    /// </summary>
    /// <remarks>The FileWebRequest class implements the <see cref="WebRequest"/> abstract base class for Uniform Resource Identifiers (URIs) that use the file:// scheme to request local files.
    /// <para>You must call the static method <see cref="RegisterPrefix"/> before this class can be used with <see cref="WebRequest.Create(string)"/>.
    /// You only need to call <see cref="RegisterPrefix"/> once in your application.</para>
    /// <para>To obtain an instance of <see cref="FileWebRequest"/>, use the <see cref="WebRequest.Create(string)"/> method after calling <see cref="RegisterPrefix"/>.
    /// You can also use the <see cref="WebClient"/> class to upload and download files.</para></remarks>
    /// <example>The following code example uses the FileWebRequest class to access a file system resource.
    /// <code lang="vbnet">
    /// '
    /// ' This example creates or opens a text file and stores a string in it. 
    /// ' Both the file and the string are passed by the user.
    /// ' Note. For this program to work, the folder containing the test file
    /// ' must be shared, with its permissions set to allow write access. 
    /// 
    /// Imports System.Net
    /// Imports System
    /// Imports System.Diagnostics
    /// Imports System.IO
    /// Imports System.Text
    /// Imports InTheHand.Net
    /// 
    /// Namespace Mssc.PluggableProtocols.File
    /// 
    ///     Module TestGetRequestStream
    /// 
    ///         Class TestGetRequestStream
    /// 
    ///             Private Shared myFileWebRequest As FileWebRequest
    /// 
    ///             ' Show how to use this program.
    ///             Private Shared Sub showUsage()
    ///                 Console.WriteLine(ControlChars.Lf + "Please enter file name and timeout :")
    ///                 Console.WriteLine("Usage: vb_getrequeststream &lt;systemname&gt;/&lt;sharedfoldername&gt;/&lt;filename&gt; timeout")
    ///                 Console.WriteLine("Example: vb_getrequeststream ngetrequestrtream() ndpue/temp/hello.txt  1000")
    ///                 Console.WriteLine("Small time-out values (for example, 3 or less) cause a time-out exception.")
    ///             End Sub
    /// 
    ///             Private Shared Sub makeFileRequest(ByVal fileName As String, ByVal timeout As Integer)
    ///                 Try
    ///                     ' Create a Uri object.to access the file requested by the user. 
    ///                     Dim myUrl As New Uri("file://" + fileName)
    /// 
    ///                     ' Create a FileWebRequest object.for the requeste file.
    ///                     myFileWebRequest = CType(WebRequest.CreateDefault(myUrl), FileWebRequest)
    /// 
    ///                     ' Set the time-out to the value selected by the user.
    ///                     myFileWebRequest.Timeout = timeout
    /// 
    ///                     ' Set the Method property to POST  
    ///                     myFileWebRequest.Method = "POST"
    /// 
    ///                 Catch e As WebException
    ///                     Console.WriteLine(("WebException is: " + e.Message))
    ///                 Catch e As UriFormatException
    ///                     Console.WriteLine(("UriFormatWebException is: " + e.Message))
    ///                 End Try
    /// 
    ///             End Sub
    /// 
    ///             Private Shared Sub writeToFile()
    ///                 Try
    ///                     ' Enter the string to write to the file.
    ///                     Console.WriteLine("Enter the string you want to write:")
    ///                     Dim userInput As String = Console.ReadLine()
    /// 
    ///                     ' Convert the string to a byte array.
    ///                     Dim encoder As New ASCIIEncoding
    ///                     Dim byteArray As Byte() = encoder.GetBytes(userInput)
    /// 
    ///                     ' Set the ContentLength property.
    ///                     myFileWebRequest.ContentLength = byteArray.Length
    /// 
    ///                     Dim contentLength As String = myFileWebRequest.ContentLength.ToString()
    /// 
    ///                     Console.WriteLine(ControlChars.Lf + "The content length is {0}.", contentLength)
    /// 
    /// 
    ///                     ' Get the file stream handler to write to the file.
    ///                     Dim readStream As Stream = myFileWebRequest.GetRequestStream()
    /// 
    ///                     ' Write to the stream. 
    ///                     ' Note. For this to work the file must be accessible
    ///                     ' on the network. This can be accomplished by setting the property
    ///                     ' sharing of the folder containg the file.  
    ///                     ' FileWebRequest.Credentials property cannot be used for this purpose.
    ///                     readStream.Write(byteArray, 0, userInput.Length)
    /// 
    /// 
    ///                     Console.WriteLine(ControlChars.Lf + "The String you entered was successfully written to the file.")
    /// 
    ///                     readStream.Close()
    /// 
    ///                 Catch e As WebException
    ///                     Console.WriteLine(("WebException is: " + e.Message))
    ///                 Catch e As UriFormatException
    ///                     Console.WriteLine(("UriFormatWebException is: " + e.Message))
    ///                 End Try
    /// 
    ///             End Sub
    /// 
    ///             Public Shared Sub Main(ByVal args() As String)
    /// 
    ///                 If args.Length &lt; 2 Then
    ///                     showUsage()
    ///                 Else
    ///                     makeFileRequest(args(0), Integer.Parse(args(1)))
    ///                     writeToFile()
    ///                 End If
    /// 
    ///             End Sub 'Main
    /// 
    ///         End Class 'TestGetRequestStream
    /// 
    ///     End Module
    /// 
    /// End Namespace
    /// </code>
    /// </example>
    public class FileWebRequest : WebRequest
    {
        internal static bool registered = false;

        //register with ftp prefix
        static FileWebRequest()
        {            
            RegisterPrefix();
        }

        /// <summary>
        /// Register this class with the <see cref="WebRequest"/> class.
        /// </summary>
        /// <remarks>Once this method is called once you can use <see cref="WebRequest.Create(string)"/> to create new <see cref="FileWebRequest"/> instances.</remarks>
        public static void RegisterPrefix()
        {
            if (!registered)
            {
                registered = WebRequest.RegisterPrefix("file", new FileWebRequestCreate());
            }
        }

        /// <summary>
        /// Initializes a new <see cref="FileWebRequest"/> instance for the specified URI.
        /// </summary>
        /// <param name="uri"></param>
        internal FileWebRequest(Uri uri)
        {
            this.uri = uri;
        }


        #region Content Length
        private long contentLength = 0;
        /// <summary>
        /// Gets or sets the content length of the data being sent.
        /// </summary>
        /// <value>The number of bytes of request data being sent.</value>
        /// <exception cref="ArgumentException">ContentLength is less than 0.</exception>
        public override long ContentLength
        {
            get
            {
                return contentLength;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("ContentLength");
                }
                contentLength = value;
            }
        }
        #endregion

        #region Content Type
        /// <summary>
        /// Gets or sets the content type of the data being sent.
        /// This property is reserved for future use.
        /// </summary>
        /// <remarks>The ContentType property contains the media type of the data being sent.
        /// This is typically the MIME encoding of the content.
        /// The ContentType property is currently not used by the FileWebRequest class.</remarks>
        public override string ContentType
        {
            get
            {
                return string.Empty;
            }
            set
            {
            }
        }

        #endregion

        #region Credentials
        /// <summary>
        /// Gets or sets the credentials that are associated with this request.
        /// This property is reserved for future use.
        /// </summary>
        public override ICredentials Credentials
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        #endregion

        #region Headers
        private WebHeaderCollection headers = new WebHeaderCollection();
        /// <summary>
        /// Gets a collection of the name/value pairs that are associated with the request.
        /// This property is reserved for future use.
        /// </summary>
        /// <value>A <see cref="WebHeaderCollection"/> that contains header name/value pairs associated with this request.</value>
        /// <remarks>The <see cref="Headers"/> property is currently not used by the <see cref="FileWebRequest"/> class.</remarks>
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
        #endregion

        #region Method
        private string method = WebRequestMethods.File.DownloadFile;
        /// <summary>
        /// Gets or sets the protocol method used for the request.
        /// This property is reserved for future use.
        /// </summary>
        /// <value>The protocol method to use in this request.</value>
        /// <remarks>The Method property is currently not used by the FileWebRequest class.</remarks>
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

                /* Desktop does no validation
                //validate method
                bool valid = false;
                foreach (System.Reflection.FieldInfo fi in typeof(WebRequestMethods.File).GetFields())
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
                }*/
                method = value;
            }
        }
        #endregion

        #region Pre Authenticate
        /// <summary>
        /// Gets or sets a value that indicates whether to preauthenticate a request.
        /// This property is reserved for future use.
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
        #endregion

        #region Proxy
        /// <summary>
        /// Gets or sets the network proxy to use for this request.
        /// This property is reserved for future use.
        /// </summary>
        /// <value>An <see cref="IWebProxy"/> that indicates the network proxy to use for this request.</value>
        /// <remarks>The Proxy property is currently not used by the FileWebRequest class.</remarks>
        public override IWebProxy Proxy
        {
            get
            {
                return GlobalProxySelection.GetEmptyWebProxy();
            }
            set
            {
            }
        }
        #endregion

        #region Request Uri

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
        #endregion


        #region Get Request Stream
        private Stream requestStream;
        /// <summary>
        /// Returns a <see cref="Stream"/> object for writing data to the file system resource.
        /// </summary>
        /// <returns>A writable <see cref="Stream"/> instance used to store data to be sent to the server by the current request.</returns>
        // <exception cref="ProtocolViolationException">The <see cref="Method"/> property is not set to <see cref="WebRequestMethods.File.UploadFile"/>.</exception>
        public override Stream GetRequestStream()
        {
            /*if (Method != WebRequestMethods.File.UploadFile)
            {
                throw new ProtocolViolationException();
            }*/
            if (requestStream == null)
            {
                requestStream = new FileStream(RequestUri.AbsolutePath, FileMode.Open, FileAccess.Write);//new MemoryStream();
            }
            return requestStream;
        }
        #endregion

        #region Response
        private FileWebResponse response;
        /// <summary>
        /// Returns a response to a file system request.
        /// </summary>
        /// <returns>A <see cref="WebResponse"/> that contains the response from the file system resource.</returns>
        public override WebResponse GetResponse()
        {
            if(response == null)
            {
                //close the request stream
                if (requestStream != null)
                {
                    requestStream.Close();
                }
                response = new FileWebResponse(this);
            }
            
            return response;
        }
        #endregion

    }

    #region FileWebRequestCreate
    internal class FileWebRequestCreate : System.Net.IWebRequestCreate
    {
        #region IWebRequestCreate Members

        public WebRequest Create(Uri uri)
        {
            if (uri.Scheme.ToLower() == "file")
            {
                return new FileWebRequest(uri);
            }
            return null;
        }

        #endregion
    }
    #endregion
}