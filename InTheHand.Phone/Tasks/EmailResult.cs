// In The Hand - .NET Components for Mobility
//
// InTheHand.Phone.Tasks.EmailResult
// 
// Copyright (c) 2010 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Phone.Tasks
{
    /// <summary>
    /// Represents an email address returned from a call to the <see cref="EmailAddressChooserTask.Show"/> method of a <see cref="EmailAddressChooserTask"/> object.
    /// </summary>
    public sealed class EmailResult : TaskEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailResult"/> class.
        /// </summary>
        public EmailResult() : base()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailResult"/> class with the specified <see cref="TaskResult"/>.
        /// </summary>
        /// <param name="taskResult"></param>
        public EmailResult(TaskResult taskResult) : base(taskResult)
        {
        }

        /// <summary>
        /// Gets the email address contained in the result.
        /// </summary>
        public string Email
        {
            get;
            set;
        }
    }
}