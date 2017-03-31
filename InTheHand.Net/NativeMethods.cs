// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NativeMethods.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Text;
using System.Runtime.InteropServices;

namespace InTheHand.Net
{
    internal static class NativeMethods
    {
        static NativeMethods()
        {
            FtpWebRequest.RegisterPrefix();
            FileWebRequest.RegisterPrefix();
        }

        private const string wininet = "wininet.dll";
        private const string iphlpapi = "iphlpapi.dll";


        internal enum INTERNET_SERVICE : int
        {
            FTP    = 1,
            //GOPHER = 2,
            HTTP   = 3,
        }

        internal const int INTERNET_DEFAULT_FTP_PORT = 21;

        [DllImport(wininet, EntryPoint="InternetOpen", SetLastError = true)]
        internal static extern IntPtr InternetOpen(string lpszAgent, uint accessType, string proxy, string proxyBypass, uint dflags);

        [DllImport(wininet, EntryPoint = "InternetConnect", SetLastError = true)]
        internal static extern IntPtr InternetConnect(IntPtr hInternet, string lserverName, int serverPort, string userName, string password, INTERNET_SERVICE service, uint flags, int context);


        [DllImport(wininet, EntryPoint = "InternetCloseHandle", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool InternetCloseHandle(IntPtr hInternet);

        [DllImport(wininet, EntryPoint = "InternetOpenUrl", SetLastError = true)]
        internal static extern IntPtr InternetOpenUrl(IntPtr hInternetSession, string url, string headers, int headersLength, uint flags, int context);

        [DllImport(wininet, EntryPoint = "InternetReadFile", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool InternetReadFile(IntPtr hFile, byte[] buffer, int numberOfBytesToRead, out int numberOfBytesRead);

        [DllImport(wininet, EntryPoint = "InternetWriteFile", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool InternetWriteFile(IntPtr hFile, byte[] buffer, uint numberOfBytesToWrite, ref uint numberOfBytesWritten);

        [DllImport(wininet, EntryPoint = "InternetGetLastResponseInfo", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool InternetGetLastResponseInfo(out ERROR_INTERNET error, StringBuilder buffer, ref int bufferLength);
        
        /*[DllImport(wininet, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool InternetQueryOption(IntPtr hInternet, int dwOption, out int lpBuffer, ref int lpdwBufferLength);

        [DllImport(wininet, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool InternetSetOption(IntPtr hInternet, int dwOption, ref int lpBuffer, int dwBufferLength);*/

        [DllImport(wininet, EntryPoint = "InternetQueryDataAvailable", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool InternetQueryDataAvailable(IntPtr hFile, out int numberOfBytesAvailable, uint flags, int context);

        [DllImport(wininet, EntryPoint = "FtpOpenFile", SetLastError = true)]
        internal static extern IntPtr pFtpOpenFile(IntPtr hConnect, string fileName,
            uint dwAccess, uint dwFlags, int dwContext);

        [DllImport(wininet, EntryPoint = "FtpCreateDirectory", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool pFtpCreateDirectory(IntPtr hConnect, string directory);

        [DllImport(wininet, EntryPoint = "FtpRemoveDirectory", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool pFtpRemoveDirectory(IntPtr hConnect, string directory);

        [DllImport(wininet, EntryPoint = "FtpGetCurrentDirectory", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool pFtpGetCurrentDirectory(IntPtr hConnect, StringBuilder currentDirectory, ref int currentDirectoryLen);

        [DllImport(wininet, EntryPoint = "FtpSetCurrentDirectory", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool pFtpSetCurrentDirectory(IntPtr hConnect, string lpszDirectory);


        [DllImport(wininet, EntryPoint = "FtpCommand", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool pFtpCommand(IntPtr hConnect, bool expectResponse, uint flags, string command, int context, out IntPtr hFile);

        [DllImport(wininet, EntryPoint = "FtpDeleteFile", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool pFtpDeleteFile(IntPtr hConnect, string fileName);

        [DllImport(wininet, EntryPoint = "FtpRenameFile", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool pFtpRenameFile(IntPtr hConnect, string existing, string lnew);


        [DllImport(wininet, EntryPoint = "FtpGetFile", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool pFtpGetFile(IntPtr hConnect, string remoteFile, 
            string newFile, 
            [MarshalAs(UnmanagedType.Bool)] 
            bool failIfExists, int flagsAndAttributes, 
            int flags, int context);

        [DllImport(wininet, EntryPoint = "FtpPutFile", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool pFtpPutFile(IntPtr hConnect, string localFile, 
            string newRemoteFile,  uint flags, int context);

        /*[DllImport("wininet.dll", SetLastError = true)]
        internal static extern IntPtr HttpOpenRequest(IntPtr hConnect, string lpszVerb, string lpszObjectName, 
            string lpszVersion, string lpszReferrer, IntPtr lplpszAcceptTypes, int dwFlags, int dwContext);*/

        internal enum ERROR_INTERNET
        {
            //#define INTERNET_ERROR_BASE                     12000

            OUT_OF_HANDLES = 12001,
            TIMEOUT = 12002,
            EXTENDED_ERROR = 12003,
            INTERNAL_ERROR = 12004,
            INVALID_URL = 12005,
            UNRECOGNIZED_SCHEME = 12006,
            NAME_NOT_RESOLVED = 12007,
            PROTOCOL_NOT_FOUND = 12008,
            INVALID_OPTION = 12009,
            BAD_OPTION_LENGTH = 12010,
            OPTION_NOT_SETTABLE = 12011,
            SHUTDOWN = 12012,
            INCORRECT_USER_NAME = 12013,
            INCORRECT_PASSWORD = 12014,
            LOGIN_FAILURE = 12015,
            INVALID_OPERATION = 12016,
            OPERATION_CANCELLED = 12017,
            INCORRECT_HANDLE_TYPE = 12018,
            INCORRECT_HANDLE_STATE = 12019,
            NOT_PROXY_REQUEST = 12020,
            REGISTRY_VALUE_NOT_FOUND = 12021,
            BAD_REGISTRY_PARAMETER = 12022,
            NO_DIRECT_ACCESS = 12023,
            NO_CONTEXT = 12024,
            NO_CALLBACK = 12025,
            REQUEST_PENDING = 12026,
            INCORRECT_FORMAT = 12027,
            ITEM_NOT_FOUND = 12028,
            CANNOT_CONNECT = 12029,
            CONNECTION_ABORTED = 12030,
            CONNECTION_RESET = 12031,
            FORCE_RETRY = 12032,
            INVALID_PROXY_REQUEST = 12033,
            NEED_UI = 12034,
            HANDLE_EXISTS = 12036,
            SEC_CERT_DATE_INVALID = 12037,
            SEC_CERT_CN_INVALID = 12038,
            HTTP_TO_HTTPS_ON_REDIR = 12039,
            HTTPS_TO_HTTP_ON_REDIR = 12040,
            MIXED_SECURITY = 12041,
            CHG_POST_IS_NON_SECURE = 12042,
            POST_IS_NON_SECURE = 12043,
            CLIENT_AUTH_CERT_NEEDED = 12044,
            INVALID_CA = 12045,
            CLIENT_AUTH_NOT_SETUP = 12046,
            ASYNC_THREAD_FAILED = 12047,
            REDIRECT_SCHEME_CHANGE = 12048,
            DIALOG_PENDING = 12049,
            RETRY_DIALOG = 12050,
            HTTPS_HTTP_SUBMIT_REDIR = 12052,
            INSERT_CDROM = 12053,
            FORTEZZA_LOGIN_NEEDED = 12054,
            SEC_CERT_ERRORS = 12055,
            SEC_CERT_NO_REV = 12056,
            SEC_CERT_REV_FAILED = 12057,

            //
            // FTP API errors
            //

            FTP_TRANSFER_IN_PROGRESS = 12110,
            FTP_DROPPED = 12111,
            FTP_NO_PASSIVE_MODE = 12112,

            //
            // additional Internet API error codes
            //

            SECURITY_CHANNEL_ERROR = 12157,
            UNABLE_TO_CACHE_FILE = 12158,
            TCPIP_NOT_INSTALLED = 12159,
            DISCONNECTED = 12163,
            SERVER_UNREACHABLE = 12164,
            PROXY_SERVER_UNREACHABLE = 12165,

            BAD_AUTO_PROXY_SCRIPT = 12166,
            UNABLE_TO_DOWNLOAD_SCRIPT = 12167,
            SEC_INVALID_CERT = 12169,
            SEC_CERT_REVOKED = 12170,

            // InternetAutodial specific errors

            /*FAILED_DUETOSECURITYCHECK  = 12171,
            NOT_INITIALIZED          = 12172,
            NEED_MSN_SSPI_PKG          = 12173,
            LOGIN_FAILURE_DISPLAY_ENTITY_BODY   = 12174,*/
        }

        internal const uint FTP_TRANSFER_TYPE_ASCII = 1;
        internal const uint FTP_TRANSFER_TYPE_BINARY = 2;

        internal const uint INTERNET_FLAG_RELOAD = 0x80000000;
        internal const uint INTERNET_FLAG_PASSIVE = 0x08000000;
        // internal const int INTERNET_OPTION_CONNECT_TIMEOUT = 2;
        internal const uint INTERNET_OPTION_DATAFILE_NAME = 33;
        internal const uint GENERIC_READ = 0x80000000;
        internal const uint GENERIC_WRITE = 0x40000000;

        // status
        [DllImport(wininet, EntryPoint = "InternetSetStatusCallback")]
        internal static extern IntPtr pInternetSetStatusCallback(IntPtr hInternet,
            INTERNET_STATUS_CALLBACK lpfnInternetCallback);

        internal delegate void INTERNET_STATUS_CALLBACK(IntPtr hInternet, ref uint context,
            INTERNET_STATUS internetStatus, IntPtr statusInformation, uint statusInformationLength);

        internal static void ThrowException(FtpWebResponse response)
        {
            System.Net.WebExceptionStatus status = System.Net.WebExceptionStatus.Success;
            ERROR_INTERNET error = (ERROR_INTERNET)Marshal.GetLastWin32Error();
            string message = string.Empty; 
            
            if (error == 0)
            {
                return;
            }

            ERROR_INTERNET lrError;
            string lastResponse = Ftp.GetLastResponseInfo(out lrError);
            if (string.IsNullOrEmpty(lastResponse))
            {
                message = "Unable to connect to the remote server";
            }
            else
            {
                message = "Server response: " + lastResponse;
            }

            //try to get most relevant webexceptionstatus
            switch (error)
            {
                case ERROR_INTERNET.CONNECTION_ABORTED:
                case ERROR_INTERNET.CONNECTION_RESET:
                case ERROR_INTERNET.DISCONNECTED:
                case ERROR_INTERNET.FTP_DROPPED:
                    status = System.Net.WebExceptionStatus.ConnectionClosed;
                    break;
                case ERROR_INTERNET.CANNOT_CONNECT:
                case ERROR_INTERNET.SERVER_UNREACHABLE:
                case ERROR_INTERNET.FTP_NO_PASSIVE_MODE:
                    status = System.Net.WebExceptionStatus.ConnectFailure;
                    break;
                case ERROR_INTERNET.TIMEOUT:
                    status = System.Net.WebExceptionStatus.Timeout;
                    break;
                case ERROR_INTERNET.SECURITY_CHANNEL_ERROR:
                    status = System.Net.WebExceptionStatus.SecureChannelFailure;
                    break;
                case ERROR_INTERNET.NAME_NOT_RESOLVED:
                    status = System.Net.WebExceptionStatus.NameResolutionFailure;
                    break;
                default:
                /*case ERROR_INTERNET.INCORRECT_PASSWORD:
                case ERROR_INTERNET.INCORRECT_USER_NAME:
                case ERROR_INTERNET.FTP_TRANSFER_IN_PROGRESS:
                case ERROR_INTERNET.LOGIN_FAILURE:*/
                    status = System.Net.WebExceptionStatus.ProtocolError;
                    break;
            }

            throw new System.Net.WebException(message, InTheHand.ComponentModel.Win32ExceptionInTheHand.Create((int)error),status, response);
        }
    }
}