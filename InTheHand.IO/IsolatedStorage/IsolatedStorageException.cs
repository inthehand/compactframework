// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsolatedStorageException.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.IO.IsolatedStorage
{
    /// <summary>
    /// The exception that is thrown when an operation in isolated storage fails.
    /// </summary>
    public sealed class IsolatedStorageException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception. If the inner parameter is not null, the current exception is raised in a catch block that handles the inner exception.</param>
        public IsolatedStorageException(string message, System.Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public IsolatedStorageException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageException"/> class with default properties.
        /// </summary>
        public IsolatedStorageException() : base() { }
    }
}
