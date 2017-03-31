// In The Hand - .NET Components for Mobility
//
// InTheHand.Phone.Tasks.TaskResult
// 
// Copyright (c) 2010-12 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Phone.Tasks
{
    /// <summary>
    /// Describes the success status of a chooser operation.
    /// </summary>
    public enum TaskResult
    {
        /// <summary>
        /// No success status was returned from the chooser operation.
        /// </summary>
        None,

        /// <summary>
        /// The chooser operation was successful.
        /// </summary>
        OK,

        /// <summary>
        /// The chooser operation was cancelled by the user.
        /// </summary>
        Cancel,
    }
}