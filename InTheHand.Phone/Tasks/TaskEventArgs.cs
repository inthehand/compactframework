// In The Hand - .NET Components for Mobility
//
// InTheHand.Phone.Tasks.TaskEventArgs
// 
// Copyright (c) 2010 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Phone.Tasks
{
    /// <summary>
    /// The EventArgs used by the Completed event for all Choosers.
    /// </summary>
    public abstract class TaskEventArgs : EventArgs 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskEventArgs"/> class.
        /// </summary>
        public TaskEventArgs() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskEventArgs"/> class with the specified <see cref="TaskResult"/>.
        /// </summary>
        /// <param name="taskResult">The <see cref="TaskResult"/> associated with the new <see cref="TaskEventArgs"/>.</param>
        public TaskEventArgs(TaskResult taskResult)
        {
            this.TaskResult = taskResult;
        }

        /// <summary>
        /// The exception associated with the <see cref="TaskEventArgs"/>.
        /// </summary>
        public virtual Exception Error
        {
            get;
            set;
        }

        /// <summary>
        /// The <see cref="TaskResult"/> associated with the <see cref="TaskEventArgs"/>.
        /// This indicates whether the task was completed successfully, if the user cancelled the task, or if no result information is available.
        /// </summary>
        public virtual TaskResult TaskResult
        {
            get;
            set;
        }
    }
}