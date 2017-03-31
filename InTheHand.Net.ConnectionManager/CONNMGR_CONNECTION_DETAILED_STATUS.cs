// In The Hand - .NET Solutions for Mobility
//
// InTheHand.Net.ConnectionManager.ConnectionDetailedStatus
// 
// Copyright (c) 2003-2008 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;
using InTheHand.Runtime.InteropServices;

namespace InTheHand.Net.ConnectionManager
{
    /// <summary>
    /// Provides the detailed status of a specific network connection.
    /// </summary>
    public sealed class ConnectionDetailedStatus
    {
        private CONNMGR_CONNECTION_DETAILED_STATUS status;

        internal ConnectionDetailedStatus(CONNMGR_CONNECTION_DETAILED_STATUS status)
        {
            this.status = status;
        }

        /// <summary>
        /// The type of connection.
        /// </summary>
        public ConnectionType ConnectionType
        {
            get
            {
                return status.dwType;
            }
        }

        /// <summary>
        /// The sub-type of the connection.
        /// </summary>
        public ConnectionSubType ConnectionSubType
        {
            get
            {
                return (ConnectionSubType)((int)status.dwType << 8 | status.dwSubtype);
            }
        }

        /// <summary>
        /// True if the connection is secure.
        /// </summary>
        public bool Secure
        {
            get
            {
                return (status.dwSecure != 0);
            }
        }

        /// <summary>
        /// <see cref="Guid"/> of the source network.
        /// </summary>
        public Guid SourceNetwork
        {
            get
            {
                return status.guidSourceNet;
            }
        }

        /// <summary>
        /// <see cref="Guid"/> of the destination network.
        /// </summary>
        public Guid DestinationNetwork
        {
            get
            {
                return status.guidDestNet;
            }
        }

        /// <summary>
        /// Name of the connection.
        /// </summary>
        /// <remarks>For a Wi-Fi connection, the Description property contains the name of the service set identifier (SSID) used to connect to the network.</remarks>
        public string Description
        {
            get
            {
                if (status.pszDescription == null)
                {
                    return string.Empty;
                }

                return status.pszDescription;
            }
        }

        /// <summary>
        /// Name of the adapter (if available).
        /// </summary>
        public string AdapterName
        {
            get 
            {
                if (status.pszAdapterName == null)
                {
                    return string.Empty;
                }

                return status.pszAdapterName;
            }
        }

        /// <summary>
        /// Current connection status.
        /// </summary>
        public ConnectionStatus ConnectionStatus
        {
            get
            {
                return status.dwConnectionStatus;
            }
        }

        /// <summary>
        /// Time the connection was last established.
        /// </summary>
        public DateTime LastConnectTime
        {
            get
            {
                return status.lastConnectionTime.ToDateTime(DateTimeKind.Utc);
            }
        }

        /// <summary>
        /// Quality of the signal, can be a value between 0 and 255.
        /// </summary>
        public int SignalQuality
        {
            get
            {
                return (int)status.dwSignalQuality;
            }
        }
    }


    internal struct CONNMGR_CONNECTION_DETAILED_STATUS
    {
        public IntPtr pNext;
        public uint dwVer;
        public CONNMGRDETAILEDSTATUS_PARAM dwParams;
        public ConnectionType dwType;
        public int dwSubtype;
        public uint dwFlags;
        public uint dwSecure;
        public Guid guidDestNet;
        public Guid guidSourceNet;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pszDescription;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pszAdapterName;
        public ConnectionStatus dwConnectionStatus;
        public SYSTEMTIME lastConnectionTime;
        public uint dwSignalQuality;
        public IntPtr pIPAddr; //Actually this is a void*
    }

    [Flags()]
    internal enum CONNMGRDETAILEDSTATUS_PARAM
    {
        TYPE            =0x00000001,
        SUBTYPE         =0x00000002,
        DESTNET         =0x00000004,
        SOURCENET       =0x00000008,
        FLAGS           =0x00000010,
        SECURE          =0x00000020,
        DESCRIPTION     =0x00000040,
        ADAPTERNAME     =0x00000080,
        CONNSTATUS      =0x00000100,
        LASTCONNECT     =0x00000200,
        SIGNALQUALITY   =0x00000400,
        IPADDR          =0x00000800,
    }
    /// <summary>
    /// Types of connection.
    /// </summary>
    public enum ConnectionType : int
    {
        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// GPRS/EDGE/1x
        /// </summary>
        Cellular = 1,
        /// <summary>
        /// Ethernet/WiFi
        /// </summary>
        Nic = 2,
        /// <summary>
        /// Bluetooth.
        /// </summary>
        Bluetooth = 3,
        /// <summary>
        /// Modem.
        /// </summary>
        Unimodem = 4,
        /// <summary>
        /// Virtual Private Network.
        /// </summary>
        Vpn = 5,
        /// <summary>
        /// Proxy.
        /// </summary>
        Proxy = 6,
        /// <summary>
        /// Desktop Passthrough
        /// </summary>
        PC = 7,
    }

    /// <summary>
    /// Specific sub-type of connection.
    /// </summary>
    public enum ConnectionSubType
    {
        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Cellular.
        /// </summary>
        Cellular = 0x0100,
        /// <summary>
        /// Circuit Switched Data.
        /// </summary>
        CellularCsd = 0x0101,
        /// <summary>
        /// General Packet Radio System.
        /// </summary>
        CellularGprs = 0x0102,
        /// <summary>
        /// Not distinct from CSD.
        /// </summary>
        Cellular1xRtt = 0x0103,
        /// <summary>
        /// Not distinct from CSD.
        /// </summary>
        Cellular1xEvdo = 0x0104, 
        /// <summary>
        /// Not distinct from CSD.
        /// </summary>
        Cellular1xEvdv = 0x0105, 
        /// <summary>
        /// Not distinct from GPRS.
        /// </summary>
        CellularEdge = 0x0106, 
        /// <summary>
        /// Not distinct from GPRS.
        /// </summary>
        CellularUmts = 0x0107, 
        /// <summary>
        /// 
        /// </summary>
        CellularVoice = 0x0108,
        /// <summary>
        /// Push-to-Talk, not supported.
        /// </summary>
        CellularPtt = 0x0109,
        /// <summary>
        /// High-Speed Downlink Packet Access (3.5G).
        /// </summary>
        CellularHsdpa = 0x010a,
        //CellularMAX     11

        /// <summary>
        /// Network Interface.
        /// </summary>
        Nic = 0x0200,
        /// <summary>
        /// Network Interface (Wired).
        /// </summary>
        NicEthernet = 0x0201,
        /// <summary>
        /// Network Interface (Wireless).
        /// </summary>
        NicWifi = 0x0202,
        //NIC_MAX      3

        /// <summary>
        /// Bluetooth.
        /// </summary>
        Bluetooth = 0x0300,
        /// <summary>
        /// Bluetooth Remote Access Server Profile.
        /// </summary>
        BluetoothRAS = 0x0301,
        /// <summary>
        /// Bluetooth Personal Area Network Profile.
        /// </summary>
        BluetoothPAN = 0x0302,
//#define CM_CONNSUBTYPE_BLUETOOTH_MAX        3
        /// <summary>
        /// 
        /// </summary>
        Unimodem = 0x0400,
        /// <summary>
        /// 
        /// </summary>
        UnimodemCsd = 0x0401,
        /// <summary>
        /// 
        /// </summary>
        UnimodemOobCsd = 0x0402,
//
// Derived from unimodem device types
//
        /// <summary>
        /// Direct Cable Connect (DCC)
        /// </summary>
        UnimodemNullModem = 0x0403, 
        /// <summary>
        /// Serial port attached modem.
        /// </summary>
        UnimodemExternalModem = 0x0404,
        /// <summary>
        /// Internal Modem.
        /// </summary>
        UnimodemInternalModem = 0x0405,
        /// <summary>
        /// PCMCMIA Modem.
        /// </summary>
        UnimodemPcmciaModem = 0x0406,
        /// <summary>
        /// DCC over Irda.
        /// </summary>
        UnimodemIrCommModem = 0x0407,
        /// <summary>
        /// Bluetooth modem.
        /// </summary>
        UnimodemDynamicModem = 0x0408,
        /// <summary>
        /// DCC over Bluetooth.
        /// </summary>
        UnimodemDynamicPort = 0x0409,
//#define CM_CONNSUBTYPE_UNIMODEM_MAX             10

        /// <summary>
        /// Virtual Private Network.
        /// </summary>
        Vpn = 0x0500,
        /// <summary>
        /// L2TP Virtual Private Network.
        /// </summary>
        VpnL2TP = 0x0501,
        /// <summary>
        /// PPTP Virtual Private Network.
        /// </summary>
        VpnPPTP = 0x0502,
//#define CM_CONNSUBTYPE_VPN_MAX      3

        /// <summary>
        /// Proxy.
        /// </summary>
        Proxy = 0x0600,
        /// <summary>
        /// Null Proxy.
        /// </summary>
        ProxyNull = 0x0601,
        /// <summary>
        /// HTTP Proxy.
        /// </summary>
        ProxyHttp = 0x0602,
        /// <summary>
        /// WAP Proxy.
        /// </summary>
        ProxyWap = 0x0603,
        /// <summary>
        /// Socks 4 Proxy.
        /// </summary>
        ProxySocks4 = 0x0604,
        /// <summary>
        /// Socks 5 Proxy.
        /// </summary>
        ProxySocks5 = 0x0605,
//#define CM_CONNSUBTYPE_PROXY_MAX        6
        /// <summary>
        /// Connection via a PC.
        /// </summary>
        PC = 0x0700,
        /// <summary>
        /// Desktop Passthrough.
        /// </summary>
        PCDesktopPassthrough = 0x0701,
        /// <summary>
        /// Infrared desktop link.
        /// </summary>
        PCIR = 0x0702,
        /// <summary>
        /// Modem link.
        /// </summary>
        PCModemLink = 0x0703,
//#define CM_CONNSUBTYPE_PC_MAX                4
    }

}