// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebRequestMethods.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Net;

namespace InTheHand.Net
{
    /// <summary>
    /// Container class for <see cref="WebRequestMethods.Ftp"/> and <see cref="WebRequestMethods.Http"/> classes.
    /// </summary>
    public static class WebRequestMethods
    {
        /// <summary>
        /// Represents the types of file protocol methods that can be used with a FILE request.
        /// </summary>
        /// <remarks>The members of this class can be used to set the <see cref="WebRequest.Method"/> property that determines the protocol method that is to be used to perform a requested action, such as uploading or downloading a file.</remarks>
        public static class File
        {
            /// <summary>
            /// Represents the FILE GET protocol method that is used to retrieve a file from a specified location. 
            /// </summary>
            public const string DownloadFile = "GET";

            /// <summary>
            /// Represents the FILE PUT protocol method that is used to copy a file to a specified location.
            /// </summary>
            public const string UploadFile = "PUT";
        }

        /// <summary>
        /// Represents the types of FTP protocol methods that can be used with an FTP request.
        /// </summary>
        /// <remarks>The members of this class can be used to set the <see cref="WebRequest.Method"/> property that determines the protocol method that is to be used to perform a requested action, such as uploading or downloading a file.</remarks>
        public static class Ftp
        {
            /// <summary>
            /// Represents the FTP APPE protocol method that is used to append a file to an existing file on an FTP server. 
            /// </summary>
            public const string AppendFile = "APPE";

            /// <summary>
            /// Represents the FTP DELE protocol method that is used to delete a file on an FTP server. 
            /// </summary>
            public const string DeleteFile = "DELE";

            /// <summary>
            /// Represents the FTP RETR protocol method that is used to download a file from an FTP server. 
            /// </summary>
            public const string DownloadFile = "RETR";

            /// <summary>
            /// Represents the FTP MDTM protocol method that is used to download a timestamp for a file from an FTP server.
            /// </summary>
            public const string GetDateTimestamp = "MDTM";

            /// <summary>
            /// Represents the FTP SIZE protocol method that is used to retrieve the size of a file on an FTP server. 
            /// </summary>
            public const string GetFileSize = "SIZE";

            /// <summary>
            /// Represents the FTP NLIST protocol method that gets a short listing of the files on an FTP server. 
            /// </summary>
            public const string ListDirectory = "NLST";

            /// <summary>
            /// Represents the FTP LIST protocol method that gets a detailed listing of the files on an FTP server. 
            /// </summary>
            public const string ListDirectoryDetails = "LIST";

            /// <summary>
            /// Represents the FTP MKD protocol method creates a directory on an FTP server. 
            /// </summary>
            public const string MakeDirectory = "MKD";

            /// <summary>
            /// Represents the FTP PWD protocol method that prints the name of the current working directory. 
            /// </summary>
            public const string PrintWorkingDirectory = "PWD";

            /// <summary>
            /// Represents the FTP RMD protocol method that removes a directory. 
            /// </summary>
            public const string RemoveDirectory = "RMD";

            /// <summary>
            /// Represents the FTP RENAME protocol method that renames a directory. 
            /// </summary>
            public const string Rename = "RENAME";

            /// <summary>
            /// Represents the FTP STOR protocol method that uploads a file to an FTP server. 
            /// </summary>
            public const string UploadFile = "STOR";

            /// <summary>
            /// Represents the FTP STOU protocol that uploads a file with a unique name to an FTP server. 
            /// </summary>
            public const string UploadFileWithUniqueName = "STOU";

            //public const string Help = "?";
        }

        /// <summary>
        /// Represents the types of HTTP protocol methods that can be used with an HTTP request.
        /// </summary>
        /// <remarks>The members of this class can be used to set the <see cref="WebRequest.Method"/> property that determines the protocol method that is to be used to perform a requested action, such as uploading or downloading a file.</remarks>
        public static class Http
        {
            /// <summary>
            /// Represents the HTTP CONNECT protocol method that is used with a proxy that can dynamically switch to tunneling, as in the case of SSL tunneling.
            /// </summary>
            public const string Connect = "CONNECT";

            /// <summary>
            /// Represents an HTTP GET protocol method.
            /// </summary>
            public const string Get = "GET";

            /// <summary>
            /// Represents an HTTP HEAD protocol method.
            /// The HEAD method is identical to GET except that the server only returns message-headers in the response, without a message-body.
            /// </summary>
            public const string Head = "HEAD";

            /// <summary>
            /// Represents an HTTP MKCOL request that creates a new collection (such as a collection of pages) at the location specified by the request-Uniform Resource Identifier (URI). 
            /// </summary>
            public const string MkCol = "MKCOL";

            /// <summary>
            /// Represents an HTTP POST protocol method that is used to post a new entity as an addition to a URI.
            /// </summary>
            public const string Post = "POST";

            /// <summary>
            /// Represents an HTTP PUT protocol method that is used to replace an entity identified by a URI.
            /// </summary>
            public const string Put = "PUT";
        }
    }
}
