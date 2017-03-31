// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.NetworkInformation.PingException
// 
// Copyright (c) 2003-2014 In The Hand Ltd, All rights reserved.

using System;

namespace InTheHand.Net.NetworkInformation
{
    /// <summary>
    /// The exception that is thrown when a <see cref="Ping.Send(string)"/> method throws an exception.
    /// </summary>
    /// <remarks>The Ping class throws this exception to indicate that while sending an Internet Control Message Protocol (ICMP) Echo request, a method called by the <see cref="Ping"/> class threw an unhandled exception.
    /// Applications should check the inner exception of a <see cref="PingException"/> object to identify the problem.
    /// <para>The <see cref="Ping"/> class does not throw this exception if the ICMP Echo request fails because of network, ICMP, or destination errors.
    /// For such errors, the <see cref="Ping"/> class returns a <see cref="PingReply"/> object with the relevant <see cref="IPStatus"/> value set in the <see cref="PingReply.Status"/> property.</para></remarks>
    [Serializable]
    public class PingException : InvalidOperationException
    {
        internal PingException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PingException"/> class using the specified message. 
        /// </summary>
        /// <param name="message">A <see cref="String"/> that describes the error.</param>
        public PingException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PingException"/> class using the specified message and inner exception. 
        /// </summary>
        /// <param name="message">A <see cref="String"/> that describes the error.</param>
        /// <param name="innerException">The exception that causes the current exception.</param>
        public PingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
