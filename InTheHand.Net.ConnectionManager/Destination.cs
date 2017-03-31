// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.ConnectionManager.Destination
// 
// Copyright (c) 2003-2010 In The Hand Ltd, All rights reserved.

using System;

namespace InTheHand.Net.ConnectionManager
{
    /// <summary>
    /// Contains Guid values for standard destination networks.
    /// </summary>
    public static class Destination
    {
        /// <summary>
        /// The Internet.
        /// </summary>
        public static readonly Guid Internet = new Guid("{436EF144-B4FB-4863-A041-8F905A62C572}");
        /// <summary>
        /// The corporate network.
        /// </summary>
        public static readonly Guid Corporate = new Guid("{A1182988-0D73-439e-87AD-2A5B369F808B}");
        /// <summary>
        /// The WAP network.
        /// </summary>
        public static readonly Guid Wap = new Guid("{7022E968-5A97-4051-BC1C-C578E2FBA5D9}");
        /// <summary>
        /// The secure WAP network.
        /// </summary>
        public static readonly Guid SecureWap = new Guid("{F28D1F74-72BE-4394-A4A7-4E296219390C}");
    }

    /// <summary>
    /// Provides information about a specific network.
    /// </summary>
    public class DestinationInfo
    {
        private Guid guid;
        private string description;
        private bool secure;

        internal DestinationInfo(CONNMGR_DESTINATION_INFO info)
        {
            guid = info.guid;
            description = info.szDescription;
            secure = info.fSecure;
        }

        /// <summary>
        /// GUID associated with the network to which the connection is established.
        /// </summary>
        public Guid Guid
        {
            get
            {
                return guid;
            }
        }

        /// <summary>
        /// Description of the network.
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }
        }

        /// <summary>
        /// Specifies if multi-homing is allowed on the network.
        /// </summary>
        public bool Secure
        {
            get
            {
                return secure;
            }
        }
    }
}