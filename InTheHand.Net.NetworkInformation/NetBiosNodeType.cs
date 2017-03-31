// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.NetworkInformation.NetBiosNodeType
// 
// Copyright (c) 2003-2014 In The Hand Ltd, All rights reserved.

namespace InTheHand.Net.NetworkInformation
{
    /// <summary>
    /// Specifies the Network Basic Input/Output System (NetBIOS) node type. 
    /// </summary>
    /// <remarks>The node type determines the way in which NetBIOS names are resolved to Internet Protocol (IP) addresses. 
    /// The following table shows the name resolution method for each node type.
    /// <list type="table">
    /// <term>Node type</term><description>Resolve name to IP address </description>
    /// <term>Broadcast</term><description>Uses NetBIOS name queries.</description>
    /// <term>Peer2Peer</term><description>Uses a NetBIOS name server (NBNS), for example, Windows Internet Name Service (WINS).</description>
    /// <term>Mixed</term><description>Attempts to resolve by first using NetBIOS name queries and then using an NBNS.</description>
    /// <term>Hybrid</term><description>Attempts to resolve by first using an NBNS and then using a NetBIOS name query.</description>
    /// </list>  
    /// This enumeration is used to specify values for the <see cref="IPGlobalProperties.NodeType"/> property.</remarks>
    public enum NetBiosNodeType
    {
        /// <summary>
        /// An unknown node type.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// A broadcast node.
        /// </summary>
        Broadcast = 1,

        /// <summary>
        /// A peer-to-peer node.
        /// </summary>
        Peer2Peer = 2,

        /// <summary>
        /// A mixed node.
        /// </summary>
        Mixed = 4,

        /// <summary>
        /// A hybrid node.
        /// </summary>
        Hybrid = 8,
    }
}
