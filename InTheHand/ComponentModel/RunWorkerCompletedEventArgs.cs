// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RunWorkerCompletedEventArgs.cs" company="In The Hand Ltd">
// Copyright (c) 2004-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;

namespace InTheHand.ComponentModel
{
    /// <summary>
    /// Represents the method that will handle the <see cref="BackgroundWorker.RunWorkerCompleted"/> event of a <see cref="BackgroundWorker"/> class.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="RunWorkerCompletedEventArgs"/> that contains the event data.</param>
    public delegate void RunWorkerCompletedEventHandler(object sender, RunWorkerCompletedEventArgs e);

    /// <summary>
    /// Provides data for the <see cref="BackgroundWorker.RunWorkerCompleted"/> event.
    /// </summary>
    public sealed class RunWorkerCompletedEventArgs : AsyncCompletedEventArgs
    {
        private readonly object result;

        /// <summary>
        /// Initializes a new instance of the <see cref="RunWorkerCompletedEventArgs"/> class.
        /// </summary>
        /// <param name="result">The result of an asynchronous operation.</param>
        /// <param name="error">Any error that occurred during the asynchronous operation.</param>
        /// <param name="canceled">A value indicating whether the asynchronous operation was canceled.</param>
        public RunWorkerCompletedEventArgs(object result, System.Exception error, bool canceled)
            : base(error, canceled, null)
        {
            this.result = result;
        }
        /// <summary>
        /// Gets a value that represents the result of an asynchronous operation.
        /// </summary>
        public object Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return result;
            }
        }

    }
}
