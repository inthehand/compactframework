// In The Hand - .NET Components for Mobility
//
// InTheHand.WindowsMobile.Configuration.NativeMethods
// 
// Copyright (c) 2003-2012 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace InTheHand.WindowsMobile.Configuration
{
    internal static class NativeMethods
    {
        [DllImport("aygshell", EntryPoint = "DMProcessConfigXML")]
        internal static extern uint ProcessConfigXML(string wXMLin, CFGFLAG flags, ref IntPtr wXMLout);

        
        [DllImport("coredll", EntryPoint = "free", SetLastError = true)]
        internal static extern void free(IntPtr ptr);

        [Flags()]
        internal enum CFGFLAG
        {
            PROCESS = 0x0001,
            METADATA = 0x0002,
        }

        [DllImport("aygshell", EntryPoint = "QueryPolicy")]
        internal static extern int QueryPolicy(SecurityPolicy policyId, out int policyValue);

        [DllImport("coredll", EntryPoint = "CeGetCurrentTrust")]
        internal static extern TrustLevel GetCurrentTrust();
    }
}
