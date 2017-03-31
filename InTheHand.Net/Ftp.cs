// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Ftp.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
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
    internal class Ftp
    {
        private static IntPtr hInternet;

        internal static IntPtr InternetHandle
        {
            get
            {
                if (hInternet == IntPtr.Zero)
                {
                    hInternet = NativeMethods.InternetOpen("InTheHand.Net", 1, null, null, 0);
                    if (hInternet == IntPtr.Zero)
                    {
                        throw InTheHand.ComponentModel.Win32ExceptionInTheHand.Create();
                    } 

                }

                return hInternet;
            }
        }

        private IntPtr hConnect;
        
        internal Ftp(Uri host, System.Net.NetworkCredential credentials, bool usePassive)
        {
            string server = host.Host;
            int port = NativeMethods.INTERNET_DEFAULT_FTP_PORT;

            if (!host.IsDefaultPort)
            {
                port = host.Port;
            }
            string userName = null;
            string password = null;
            if (credentials != null)
            {
                //additional handling for logins with domains
                if (!string.IsNullOrEmpty(credentials.Domain))
                {
                    userName = credentials.Domain + "\\" + credentials.UserName;
                }
                else
                {
                    userName = credentials.UserName;
                }
                password = credentials.Password;
            }
            else
            {
                //parse user info in the uri
                string userInfo = host.UserInfo;
                if (!string.IsNullOrEmpty(userInfo))
                {
                    //username and password
                    if (userInfo.IndexOf(':') > -1)
                    {
                        string[] userParts = userInfo.Split(':');
                        userName = userParts[0];
                        password = userParts[1];
                    }
                    else
                    {
                        //username only
                        userName = userInfo;
                    }
                }
            }
            hConnect = NativeMethods.InternetConnect(InternetHandle, server, port, userName, password, NativeMethods.INTERNET_SERVICE.FTP, usePassive ? NativeMethods.INTERNET_FLAG_PASSIVE : 0, 0);

            if (hConnect == IntPtr.Zero)
            {
                NativeMethods.ThrowException(null);
            }
        }

        internal IntPtr Handle
        {
            get
            {
                return hConnect;
            }
        }

        internal void GetFile(string remoteFile, string localFile)
        {
            //now overwrites existing file
            bool success = NativeMethods.pFtpGetFile(hConnect, remoteFile, localFile, false, 0, 0x04000000, this.GetHashCode());
            if (!success)
            {
                NativeMethods.ThrowException(null);
            }
        }

        internal void PutFile(string localFile, string remoteFile)
        {
            bool success = NativeMethods.pFtpPutFile(hConnect, localFile, remoteFile, 0x04000000, this.GetHashCode());

            if (!success)
            {
                NativeMethods.ThrowException(null);
            }
        }

        internal static string GetLastResponseInfo(out NativeMethods.ERROR_INTERNET error)
        {
            StringBuilder sb = new StringBuilder(260);
            int len = sb.Capacity;

            bool success = NativeMethods.InternetGetLastResponseInfo(out error, sb, ref len);
            if (success)
            {
                if (sb.Length > 0)
                {
                    string statusDescription = sb.ToString().TrimEnd('\r', '\n');
                    if (statusDescription.LastIndexOf('\n') > -1)
                    {
                        statusDescription = statusDescription.Substring(statusDescription.LastIndexOf('\n') + 1, (statusDescription.Length - statusDescription.LastIndexOf('\n')) - 1);
                    }

                    return statusDescription;
                }
            }

            return string.Empty;
        }

        internal void Close()
        {
            lock (this)
            {
                if (hConnect != IntPtr.Zero)
                {
                    bool success = NativeMethods.InternetCloseHandle(hConnect);
                    hConnect = IntPtr.Zero;
                }
            }
        }
    }
}
