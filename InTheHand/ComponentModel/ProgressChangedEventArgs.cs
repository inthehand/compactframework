// In The Hand - .NET Components for Mobility
//
// InTheHand.ComponentModel.ProgressChangedEventArgs
// 
// Copyright (c) 2009-2014 In The Hand Ltd, All rights reserved.

using System;

namespace InTheHand.ComponentModel
{
    /// <summary>
    /// Represents the method that will handle the <see cref="BackgroundWorker.ProgressChanged"/> event of the <see cref="BackgroundWorker"/> class.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="ProgressChangedEventArgs"/> that contains the event data.</param>
    /// <remarks>When you create a ProgressChangedEventHandler delegate, you identify a method to handle the event.
    /// To associate the event with your event handler, add an instance of the delegate to the event.
    /// The event handler is called whenever the event occurs, unless you remove the delegate.</remarks>
    /// <example>The following code example shows how to use the ProgressChangedEventHandler class.
    /// This example is part of a larger example for the <see cref="BackgroundWorker"/> class.
    /// <code lang="vbnet">
    /// ' This event handler updates the progress bar.
    /// Private Sub backgroundWorker1_ProgressChanged( _
    ///   ByVal sender As Object, ByVal e As ProgressChangedEventArgs) _
    ///   Handles backgroundWorker1.ProgressChanged
    /// 
    ///     Me.progressBar1.Value = e.ProgressPercentage
    /// 
    /// End Sub</code>
    /// <code lang="cs">
    /// // This event handler updates the progress bar.
    /// private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
    /// {
    ///     this.progressBar1.Value = e.ProgressPercentage;
    /// }
    /// </code></example>
    public delegate void ProgressChangedEventHandler(object sender, ProgressChangedEventArgs e);
    
    /// <summary>
    /// Provides data for the <see cref="BackgroundWorker.ProgressChanged"/> event.
    /// </summary>
    /// <example>The following code example shows how to use the ProgressChangedEventArgs class.
    /// This example is part of a larger example for the <see cref="BackgroundWorker"/> class.
    /// <code lang="vbnet">
    /// ' This event handler updates the progress bar.
    /// Private Sub backgroundWorker1_ProgressChanged( _
    ///   ByVal sender As Object, ByVal e As ProgressChangedEventArgs) _
    ///   Handles backgroundWorker1.ProgressChanged
    /// 
    ///     Me.progressBar1.Value = e.ProgressPercentage
    /// 
    /// End Sub</code>
    /// <code lang="cs">
    /// // This event handler updates the progress bar.
    /// private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
    /// {
    ///     this.progressBar1.Value = e.ProgressPercentage;
    /// }
    /// </code></example>
    public class ProgressChangedEventArgs : EventArgs
    {
        private readonly int progressPercentage;
        private readonly object userState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressChangedEventArgs"/> class.
        /// </summary>
        /// <param name="progressPercentage">The percentage of an asynchronous task that has been completed.</param>
        /// <param name="userState">A unique user state.</param>
        public ProgressChangedEventArgs(int progressPercentage, object userState)
        {
            this.progressPercentage = progressPercentage;
            this.userState = userState;
        }

        /// <summary>
        /// Gets the asynchronous task progress percentage.
        /// </summary>
        /// <value>A percentage value indicating the asynchronous task progress.</value>
        /// <remarks>The <b>ProgressPercentage</b> property determines what percentage of an asynchronous task has been completed.</remarks>
        public int ProgressPercentage
        {
            get
            {
                return this.progressPercentage;
            }
        }

        /// <summary>
        /// Gets a unique user state.
        /// </summary>
        /// <value>A unique <see cref="Object"/> indicating the user state.</value>
        public object UserState
        {
            get
            {
                return this.userState;
            }
        }
    }
}
