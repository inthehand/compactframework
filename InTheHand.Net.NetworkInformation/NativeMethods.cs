// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.NetworkInformation.NativeMethods
// 
// Copyright (c) 2003-2010 In The Hand Ltd, All rights reserved.

using System;
using System.Text;
using System.Runtime.InteropServices;


namespace InTheHand.Net.NetworkInformation
{
    internal static class NativeMethods
    {

        internal static readonly bool hasIphlp = System.IO.File.Exists(System.IO.Path.Combine(EnvironmentInTheHand.SystemDirectory, iphlpapi));

        private const string iphlpapi = "iphlpapi.dll";

        [DllImport(iphlpapi, EntryPoint = "GetAdaptersAddresses")]
        internal static extern int GetAdaptersAddresses(uint Family, NetworkInformation.GAA_FLAG Flags, IntPtr Reserved, IntPtr pAdapterAddresses, ref int pOutBufLen);

        [DllImport(iphlpapi, EntryPoint = "GetNetworkParams")]
        internal static extern int GetNetworkParams(byte[] pFixedInfo, ref int pOutBufLen);

        [DllImport(iphlpapi, EntryPoint = "GetIcmpStatisticsEx", SetLastError = true)]
        internal static extern int GetIcmpStatisticsEx(ref NetworkInformation.MIBICMPINFO pStats, System.Net.Sockets.AddressFamily dwFamily);

        [DllImport(iphlpapi, EntryPoint = "GetIpStatisticsEx")]
        internal static extern int GetIpStatisticsEx(NetworkInformation.IPGlobalStatistics pStats, System.Net.Sockets.AddressFamily dwFamily);

        [DllImport(iphlpapi, EntryPoint = "GetTcpStatisticsEx")]
        internal static extern int GetTcpStatisticsEx(NetworkInformation.TcpStatistics pStats, System.Net.Sockets.AddressFamily dwFamily);

        [DllImport(iphlpapi, EntryPoint = "GetTcpTable")]
        internal static extern int GetTcpTable(IntPtr pTcpTable, ref int pdwSize, [MarshalAs(UnmanagedType.Bool)] bool bOrder);

        [DllImport(iphlpapi, EntryPoint = "GetUdpTable")]
        internal static extern int GetUdpTable(IntPtr pUdpTable, ref int pdwSize, [MarshalAs(UnmanagedType.Bool)] bool bOrder);

        [StructLayout(LayoutKind.Sequential)]
        internal struct MIB_UDPROW
        {
            internal uint dwLocalAddr;
            internal uint dwLocalPort;
        }

        [DllImport(iphlpapi, EntryPoint = "GetUdpStatisticsEx")]
        internal static extern int GetUdpStatisticsEx(NetworkInformation.UdpStatistics pStats, System.Net.Sockets.AddressFamily dwFamily);

        [DllImport(iphlpapi, EntryPoint = "GetInterfaceInfo")]
        internal static extern int GetInterfaceInfo(IntPtr pIfTable, ref int dwOutBufLen);

        [StructLayout(LayoutKind.Sequential)]
        internal struct IP_ADAPTER_INDEX_MAP 
        {
            internal uint Index; 
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=256)]
            internal string Name; 
        }

        //[DllImport(iphlpapi, SetLastError = true)]
        //internal static extern int GetIfTable(IntPtr pIfTable, ref int pdwSize, [MarshalAs(UnmanagedType.Bool)] bool bOrder);

        [DllImport(iphlpapi, EntryPoint = "GetIfEntry")]
        internal static extern int GetIfEntry(ref MIB_IFROW pIfRow);

        [StructLayout(LayoutKind.Sequential)]
        internal struct MIB_IFROW 
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=256)]
            internal string wszName;
            internal uint dwIndex;
            internal NetworkInformation.NetworkInterfaceType dwType; 
            internal int dwMtu; 
            internal int dwSpeed; 
            internal int dwPhysAddrLen; 
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=8)]
            internal byte[] bPhysAddr; 
            internal int dwAdminStatus; 
            internal NetworkInformation.OperationalStatus dwOperStatus; 
            internal int dwLastChange; 
            internal int dwInOctets; 
            internal int dwInUcastPkts; 
            internal int dwInNUcastPkts; 
            internal int dwInDiscards; 
            internal int dwInErrors; 
            internal int dwInUnknownProtos; 
            internal int dwOutOctets; 
            internal int dwOutUcastPkts; 
            internal int dwOutNUcastPkts; 
            internal int dwOutDiscards; 
            internal int dwOutErrors; 
            internal int dwOutQLen; 
            internal int dwDescrLen; 
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=256)]
            internal byte[] bDescr; 
        }

        [DllImport(iphlpapi, EntryPoint = "NotifyAddrChange")]
        internal static extern int NotifyAddrChange(ref IntPtr Handle, IntPtr overlapped);

        /*[StructLayout(LayoutKind.Sequential)]
        internal struct IP_ADAPTER_INFO 
        {
            internal IntPtr Next;
            internal int ComboIndex;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=MAX_ADAPTER_NAME_LENGTH + 4)]
            internal string AdapterName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=MAX_ADAPTER_DESCRIPTION_LENGTH + 4)]
            internal string Description;
            internal int AddressLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=MAX_ADAPTER_ADDRESS_LENGTH)]
            internal byte[] Address;
            internal int Index;
            internal int Type;
            internal int DhcpEnabled;
            internal IntPtr CurrentIpAddress;
            IP_ADDR_STRING IpAddressList;
            IP_ADDR_STRING GatewayList;
            IP_ADDR_STRING DhcpServer;
            internal int HaveWins;
            IP_ADDR_STRING PrimaryWinsServer;
            IP_ADDR_STRING SecondaryWinsServer;
            internal long LeaseObtained;
            internal long LeaseExpires;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct IP_ADDR_STRING 
        {
            internal IntPtr Next;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            internal string IpAddress;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            internal string IpMask;
            internal int Context;
        }*/
        
    }
}