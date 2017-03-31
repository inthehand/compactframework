// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.ConnectionStatus
// 
// Copyright (c) 2003-2010 In The Hand Ltd, All rights reserved.

using System;

namespace InTheHand.Net
{
    /// <summary>
    /// Contains the possible connection status values.
    /// </summary>
    public enum ConnectionStatus
    {
        /// <summary>
        /// Connection state is unknown.
        /// </summary>
        Unknown = 0x00,
        /// <summary>
        /// Device is connected to the required destination network.
        /// </summary>
        Connected = 0x10,
        /// <summary>
        /// Connection is currently suspended.
        /// </summary>
        Suspended = 0x11,

        /// <summary>
        /// Connection has been disconnected.
        /// </summary>
        Disconnected = 0x20,
        /// <summary>
        /// Connection attempt failed.
        /// </summary>
        ConnectionFailed = 0x21,
        /// <summary>
        /// Connection attempt was cancelled by the user
        /// </summary>
        ConnectionCancelled = 0x22,
        /// <summary>
        /// Connection is disabled.
        /// </summary>
        ConnectionDisabled = 0x23,
        /// <summary>
        /// Connection Manager couldn't find a path to the required destination network.
        /// </summary>
        NoPathToDestination = 0x24,
        /// <summary>
        /// Device is waiting for an available connection path.
        /// </summary>
        WaitingForPath = 0x25,
        /// <summary>
        /// Device is waiting for phone hardware.
        /// </summary>
        WaitingForPhone = 0x26,
        /// <summary>
        /// Phone hardware is off.
        /// </summary>
        PhoneOff = 0x27,
        /// <summary>
        /// Another connection request has an exclusive connection at a higher priority.
        /// </summary>
        ExclusiveConflict = 0x28,
        /// <summary>
        /// No resources available to perform connection.
        /// </summary>
        NoResources = 0x29,
        /// <summary>
        /// Link failed.
        /// </summary>
        ConnectionLinkFailed = 0x2A,
        /// <summary>
        /// Authentication failed while connecting.
        /// </summary>
        AuthenticationFailed = 0x2B,

        /// <summary>
        /// Waiting for the connection.
        /// </summary>
        WaitingConnection = 0x40,
        /// <summary>
        /// Waiting for a required resource.
        /// </summary>
        WaitingForResource = 0x41,
        /// <summary>
        /// Waiting for a required network.
        /// </summary>
        WaitingForNetwork = 0x42,
        /// <summary>
        /// Waiting to disconnect.
        /// </summary>
        WaitingDisconnection = 0x80,
        /// <summary>
        /// Waiting to abort the connection.
        /// </summary>
        WaitingConnectionAbort = 0x81,
    }
}