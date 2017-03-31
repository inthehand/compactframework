// In The Hand - .NET Components for Mobility
//
// InTheHand.Phone.Tasks.PhoneNumberResult
// 
// Copyright (c) 2010 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Phone.Tasks
{
    /// <summary>
    /// Represents a phone number returned from a call to the <see cref="PhoneNumberChooserTask.Show"/> method of a <see cref="PhoneNumberChooserTask"/> object.
    /// </summary>
    public sealed class PhoneNumberResult : TaskEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneNumberResult"/> class.
        /// </summary>
        public PhoneNumberResult()
            : base()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneNumberResult"/> class with the specified <see cref="TaskResult"/>.
        /// </summary>
        /// <param name="taskResult">The TaskResult associated with the new <see cref="PhoneNumberResult"/>.</param>
        public PhoneNumberResult(TaskResult taskResult)
            : base(taskResult)
        {
        }

        /// <summary>
        /// The phone number contained in the result.
        /// </summary>
        public string PhoneNumber
        {
            get;
            set;
        }
    }
}