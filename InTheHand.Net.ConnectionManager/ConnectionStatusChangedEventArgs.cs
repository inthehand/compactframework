// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.ConnectionManager.ConnectionStatusChangedEventArgs
// 
// Copyright (c) 2003-2010 In The Hand Ltd, All rights reserved.

using System;

namespace InTheHand.Net.ConnectionManager
{
    /// <summary>
    /// Describes a method to handle the <see cref="ConnectionManager.ConnectionStatusChanged"/> event.
    /// </summary>
    /// <param name="e"></param>
    public delegate void ConnectionStatusChangedEventHandler(ConnectionStatusChangedEventArgs e);

    /// <summary>
    /// Contains information to accompany the <see cref="ConnectionManager.ConnectionStatusChanged"/> event.
    /// </summary>
    public class ConnectionStatusChangedEventArgs : EventArgs
    {
        private ConnectionStatus connectionStatus;

        internal ConnectionStatusChangedEventArgs(ConnectionStatus status)
        {
            this.connectionStatus = status;
        }

        /// <summary>
        /// Gets the new <see cref="ConnectionStatus"/>.
        /// </summary>
        public ConnectionStatus ConnectionStatus
        {
            get
            {
                return connectionStatus;
            }
        }
    }
}