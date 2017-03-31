// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoWorkEventArgs.cs" company="In The Hand Ltd">
// Copyright (c) 2004-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.ComponentModel
{
    /// <summary>
    /// Represents the method that will handle the DoWork event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="DoWorkEventArgs"/> that contains the event data.</param>
    /// <remarks>When you create a <see cref="DoWorkEventHandler"/> delegate, you identify the method that will handle the event.
    /// To associate the event with your event handler, add an instance of the delegate to the event.
    /// The event-handler method is called whenever the event occurs, unless you remove the delegate.</remarks>
    public delegate void DoWorkEventHandler(object sender, DoWorkEventArgs e);

    /// <summary>
    /// Provides data for the <see cref="BackgroundWorker.DoWork"/> event handler.
    /// </summary>
    public sealed class DoWorkEventArgs : System.ComponentModel.CancelEventArgs
    {
        private readonly object argument;
        private object result;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoWorkEventArgs"/> class.
        /// </summary>
        /// <param name="argument">Specifies an argument for an asynchronous operation.</param>
        public DoWorkEventArgs(object argument)
        {
            this.argument = argument;
        }
        /// <summary>
        /// Gets a value that represents the argument of an asynchronous operation.
        /// </summary>
        public object Argument
        {
            get
            {
                return argument;
            }

        }
        /// <summary>
        /// Gets or sets a value that represents the result of an asynchronous operation.
        /// </summary>
        public object Result
        {
            get
            {
                return result;
            }
            set
            {
                result = value;
            }
        }
    }
}
