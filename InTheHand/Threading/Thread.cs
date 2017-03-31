// In The Hand - .NET Components for Mobility
//
// InTheHand.Threading.Thread
// 
// Copyright (c) 2003-2012 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace InTheHand.Threading
{

    /// <summary>
    /// Provides helper functions for the <see cref="Thread"/> class.
    /// </summary>
    /// <seealso cref="System.Threading.Thread"/>
    public static class ThreadInTheHand
    {
        /// <summary>
        /// Gets a value indicating the execution status of the specified thread.
        /// </summary>
        /// <param name="t">The specific thread.</param>
        /// <returns>true if this thread has been started and has not terminated normally or aborted; otherwise, false.</returns>
        public static bool GetIsAlive(this Thread t)
        {
            int ec;
            bool success = gect(t.ManagedThreadId, out ec);        

            if (ec == 0x00000103)
            {
                return true;
            }
            return false;
        }


        // Thread Helper
        [DllImport("coredll", EntryPoint = "GetExitCodeThread", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool gect(int hThread, out int lpExitCode);

    }

}
