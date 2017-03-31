// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.ConnectionManager.NativeMethods
// 
// Copyright (c) 2003-2010 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Net.ConnectionManager
{
    [Flags()]
    internal enum CONNMGR_PARAM
    {
        GUIDDESTNET       = 0x1,
        MAXCOST           = 0x2,
        MINRCVBW          = 0x4,
        MAXCONNLATENCY    = 0x8,
    }

    [Flags()]
    internal enum CONNMGR_FLAG
    {
        PROXY_HTTP		= 0x1,
        PROXY_WAP      = 0x2,
        PROXY_SOCKS4   = 0x4,
        PROXY_SOCKS5   = 0x8,
        SUSPEND_AWARE   = 0x10,
        REGISTERED_HOME = 0x20,
        NO_ERROR_MSGS   = 0x40,
    }

    internal enum ConnMgrConRefType
    {
        NAP = 0,
        PROXY,
    }

#if V2
    internal static class NativeMethods
    {
#else
    internal sealed class NativeMethods
    {
        private NativeMethods(){}
#endif

        //Windows CE RNA
        internal const int RNA_RASCMD = 0x0401;
        //internal const int RNA_ADDREF		=1;
        internal const int RNA_DELREF = 2;


        private const string cellcore = "cellcore.dll";

        internal static readonly bool hasCellcore = System.IO.File.Exists(System.IO.Path.Combine(EnvironmentInTheHand.SystemDirectory, cellcore));

        [DllImport("coredll", EntryPoint="RegisterWindowMessage", SetLastError = true)]
        internal static extern int RegisterWindowMessage(string lpString);

        [DllImport(cellcore, EntryPoint = "ConnMgrEnumDestinations")]
        internal static extern int EnumDestinations(int Index, out CONNMGR_DESTINATION_INFO pDestInfo);

        [DllImport(cellcore, EntryPoint = "ConnMgrEstablishConnection")]
        internal static extern int EstablishConnection(ref CONNMGR_CONNECTIONINFO pConnInfo, out IntPtr phConnection);

        [DllImport(cellcore, EntryPoint = "ConnMgrEstablishConnectionSync")]
        internal static extern int EstablishConnectionSync(ref CONNMGR_CONNECTIONINFO pConnInfo, out IntPtr phConnection,
            int dwTimeout, out ConnectionStatus pdwStatus);

        [DllImport(cellcore, EntryPoint = "ConnMgrConnectionStatus")]
        internal static extern int ConnectionStatus(IntPtr hConnection, out ConnectionStatus pdwStatus);

        [DllImport(cellcore, EntryPoint = "ConnMgrReleaseConnection")]
        internal static extern int ReleaseConnection(IntPtr hConnection, int lCache);

        [DllImport(cellcore, EntryPoint = "ConnMgrSetConnectionPriority")]
        internal static extern int SetConnectionPriority(IntPtr hConnection, ConnectionPriority dwPriority);

        [DllImport(cellcore, EntryPoint = "ConnMgrMapURL")]
        internal static extern int MapURL(string pwszURL, out Guid pguid, ref int pdwIndex);

        [DllImport(cellcore, EntryPoint = "ConnMgrMapConRef")]
        internal static extern int MapConRef(ConnMgrConRefType e, string szConRef, out Guid pGUID);

        [DllImport(cellcore, EntryPoint = "ConnMgrQueryDetailedStatus")]
        internal static extern int QueryDetailedStatus(IntPtr buffer, ref int pcbBufferSize);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CONNMGR_CONNECTIONINFO
    {
        internal int cbSize;
        internal CONNMGR_PARAM dwParams;
        internal CONNMGR_FLAG dwFlags;
        internal ConnectionPriority dwPriority;
        internal int bExclusive;
        internal int bDisabled;
        internal Guid guidDestNet;
        internal IntPtr hWnd;
        internal int uMsg;
        internal uint lParam;
        internal uint ulMaxCost;
        internal uint ulMinRcvBw;
        internal uint ulMaxConnLatency;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CONNMGR_DESTINATION_INFO
    {
        internal Guid guid;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=128)]
        internal string szDescription;
        [MarshalAs(UnmanagedType.Bool)]
        internal bool fSecure;
    }
}