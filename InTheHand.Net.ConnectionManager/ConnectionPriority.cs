// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.ConnectionManager.ConnectionPriority
// 
// Copyright (c) 2003-2010 In The Hand Ltd, All rights reserved.

using System;

namespace InTheHand.Net.ConnectionManager
{
    /// <summary>
    /// Defines the priority of a connection request.
    /// </summary>
    [Flags()]
    public enum ConnectionPriority
    {
        /// <summary>
        /// Voice, highest priority, reserved for internal use only.
        /// </summary>
        Voice = 0x20000,
        /// <summary>
        /// User initiated action caused this request, and UI is        	
        /// currently pending on the creation of this connection.
        /// This is appropriate for an interactive browsing session,
        /// or if the user selects "MORE" at the bottom of a truncated
        /// mail message, etc.
        /// </summary>
        UserInteractive = 0x08000,
        /// <summary>
        /// User initiated connection which has recently become idle.
        /// A connection should be marked as idle when it is no longer the user's current task.
        /// </summary>
        UserBackground = 0x02000,
        /// <summary>
        /// Interactive user task which has been idle for an application specified time.
        /// The application should toggle the state between UserIdle and UserInteractive as the user uses the application.
        /// This helps ConnectionManager optimize responsiveness to the interactive application,
        /// while sharing the connection with background applications.
        /// </summary>
        UserIdle = 0x0800,
        /// <summary>
        /// High priority background connection.
        /// </summary>
        HighPriorityBackground = 0x0200,
        /// <summary>
        /// Idle priority background connection.
        /// </summary>
        IdleBackground = 0x0080,
        /// <summary>
        /// Connection is requested on behalf of an external entity, but is an interactive session (e.g. AT Command Interpreter)
        /// </summary>
        ExternalInteractive = 0x0020,
        /// <summary>
        /// Lowest priority.
        /// Only connects if another higher priority client is already using the same path.
        /// </summary>
        LowPriorityBackground = 0x0008,
        //CACHED            = 0x0002,
        //ALWAYS_ON         = 0x0001,
    }
}