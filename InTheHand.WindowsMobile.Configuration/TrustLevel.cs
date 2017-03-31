// In The Hand - .NET Components for Mobility
//
// InTheHand.WindowsMobile.Configuration.TrustLevel
// 
// Copyright (c) 2003-2010 In The Hand Ltd, All rights reserved.

using System;
using System.Xml;
using System.Runtime.InteropServices;

namespace InTheHand.WindowsMobile.Configuration
{
    /// <summary>
    /// Indicates the level of trust for a process.
    /// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
    /// </list>
    /// </remarks>
    public enum TrustLevel
    {
        /// <summary>
        /// The OEM does not trust this module.
        /// </summary>
        NoTrust = 0,

        /// <summary>
        /// The OEM trusts the module to run, but restricts the module from making certain function calls. 
        /// </summary>
        Restricted = 1,

        /// <summary>
        /// The OEM trusts the module to perform any OS function calls.
        /// </summary>
        Trusted = 2,
    }
}
