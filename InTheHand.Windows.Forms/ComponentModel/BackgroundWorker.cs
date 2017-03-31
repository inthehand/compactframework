// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackgroundWorker.cs" company="In The Hand Ltd">
// Copyright (c) 2004-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;

namespace InTheHand.ComponentModel
{
    /// <summary>
    /// Executes an operation on a separate thread.
    /// </summary>
    /// <remarks><para>Equivalent to System.ComponentModel.BackgroundWorker.</para>
    /// The <see cref="BackgroundWorker"/> class allows you to run an operation on a separate, dedicated thread.
    /// Time-consuming operations like downloads and database transactions can cause your user interface (UI) to seem as though it has stopped responding while they are running.
    /// When you want a responsive UI and you are faced with long delays associated with such operations, the <see cref="BackgroundWorker"/> class provides a convenient solution.</remarks>
    public class BackgroundWorker : Component
    {
        /// <summary>
        /// Occurs when <see cref="RunWorkerAsync()"/> is called.
        /// </summary>
        public event DoWorkEventHandler DoWork;

        /// <summary>
        /// Occurs when <see cref="ReportProgress(System.Int32)"/> is called.
        /// </summary>
        public event ProgressChangedEventHandler ProgressChanged;

        /// <summary>
        /// Occurs when the background operation has completed, has been cancelled, or has raised an exception.
        /// </summary>
        public event RunWorkerCompletedEventHandler RunWorkerCompleted;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundWorker"/> class.
        /// </summary>
        public BackgroundWorker()
        {
            // used to raise events on UI thread
            uiControl = new System.Windows.Forms.Control();
        }

        /// <summary>
        /// Gets a value indicating whether the application has requested cancellation of a background operation.
        /// </summary>
        public bool CancellationPending
        {
            get
            {
                return cancellationPending;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="BackgroundWorker"/> is running an asynchronous operation.
        /// </summary>
        /// <value>true, if the <see cref="BackgroundWorker"/> is running an asynchronous operation; otherwise, false.</value>
        /// <remarks>The <see cref="BackgroundWorker"/> starts an asynchronous operation when you call <see cref="RunWorkerAsync()"/>.</remarks>
        /// <seealso cref="RunWorkerAsync()"/>
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
        }

        /// <summary>
        /// Raises the <see cref="BackgroundWorker.ProgressChanged"/> event.
        /// </summary>
        /// <param name="progressPercent">The percentage, from 0 to 100, of the background operation that is complete.</param>
        /// <remarks>If you need the background operation to report on its progress, you can call the <see cref="ReportProgress"/> method to raise the <see cref="ProgressChanged"/> event.
        /// The <see cref="WorkerReportsProgress"/> property value must true, or <see cref="ReportProgress"/> will throw an <see cref="InvalidOperationException"/>.
        /// <para>It is up to you to implement a meaningful way of measuring your background operation's progress as a percentage of the total task completed.</para></remarks>
        /// <exception cref="InvalidOperationException">The <see cref="WorkerReportsProgress"/> property is set to false.</exception>
        public void ReportProgress(int progressPercent)
        {
            if (!reportsProgress)
            {
                throw new System.InvalidOperationException(InTheHand.Properties.Resources.BackgroundWorker_WorkerDoesntReportProgress);
            }

            // Send the event to the GUI
            System.Threading.ThreadPool.QueueUserWorkItem(
                new System.Threading.WaitCallback(ProgressHelper),
                new ProgressChangedEventArgs(progressPercent, null));
        }

        /// <summary>
        /// Starts execution of a background operation.
        /// </summary>
        /// <remarks>The <see cref="RunWorkerAsync()"/> method submits a request to start the operation running asynchronously.
        /// When the request is serviced, the <see cref="DoWork"/> event is raised, which in turn starts execution of your background operation.
        /// <para>If the background operation is already running, calling <see cref="RunWorkerAsync()"/> again will raise an <see cref="InvalidOperationException"/>.</para></remarks>
        /// <exception cref="InvalidOperationException"><see cref="IsBusy"/> is true.</exception>
        public void RunWorkerAsync()
        {
            this.RunWorkerAsync(null);
        }

        /// <summary>
        /// Starts execution of a background operation.
        /// </summary>
        /// <param name="argument">A parameter for use by the background operation to be executed in the <see cref="BackgroundWorker.DoWork"/> event handler.</param>
        /// <remarks>The <see cref="RunWorkerAsync()"/> method submits a request to start the operation running asynchronously.
        /// When the request is serviced, the <see cref="DoWork"/> event is raised, which in turn starts execution of your background operation.
        /// <para>If the background operation is already running, calling <see cref="RunWorkerAsync()"/> again will raise an <see cref="InvalidOperationException"/>.</para></remarks>
        /// <exception cref="InvalidOperationException"><see cref="IsBusy"/> is true.</exception>
        public void RunWorkerAsync(object argument)
        {
            if (isBusy)
            {
                throw new System.InvalidOperationException(Properties.Resources.BackgroundWorker_WorkerAlreadyRunning);
            }

            isBusy = true;
            cancellationPending = false;

            System.Threading.ThreadPool.QueueUserWorkItem(
                new System.Threading.WaitCallback(DoTheRealWork), argument);
        }

        /// <summary>
        /// Requests cancellation of a pending background operation.
        /// </summary>
        /// <remarks><see cref="CancelAsync"/> submits a request to terminate the pending background operation and sets the <see cref="CancellationPending"/> property to true.
        /// <para>When you call <see cref="CancelAsync"/>, your worker method has an opportunity to stop its execution and exit.
        /// The worker code should periodically check the <see cref="CancellationPending"/> property to see if it has been set to true.</para></remarks>
        /// <exception cref="InvalidOperationException"><see cref="WorkerSupportsCancellation"/> is false.</exception>
        public void CancelAsync()
        {
            if (!supportsCancellation)
            {
                throw new System.InvalidOperationException(Properties.Resources.BackgroundWorker_WorkerDoesntSupportCancellation);
            }

            cancellationPending = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="BackgroundWorker"/> object can report progress updates.
        /// </summary>
        /// <value>true if the <see cref="BackgroundWorker"/> supports progress updates; otherwise false.
        /// The default is false.</value>
        /// <remarks>Set the <see cref="WorkerReportsProgress"/> property to true if you want the <see cref="BackgroundWorker"/> to support progress updates.
        /// When this property is true, user code can call the <see cref="ReportProgress"/> method to raise the <see cref="ProgressChanged"/> event.</remarks>
        public bool WorkerReportsProgress
        {
            get
            {
                return reportsProgress;
            }

            set
            {
                reportsProgress = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="BackgroundWorker"/> object supports asynchronous cancellation.
        /// </summary>
        /// <value>true if the <see cref="BackgroundWorker"/> supports cancellation; otherwise false.
        /// The default is false.</value>
        /// <remarks>Set the <see cref="WorkerSupportsCancellation"/> property to true if you want the <see cref="BackgroundWorker"/> to support cancellation.
        /// When this property is true, you can call the <see cref="CancelAsync"/> method to interrupt a background operation.</remarks>
        public bool WorkerSupportsCancellation
        {
            get
            {
                return supportsCancellation;
            }

            set
            {
                supportsCancellation = value;
            }
        }

        //Ensures the component is used only once per session
        private bool isBusy;

        //Stores the cancelation request that the worker thread (user's code) should check via CancellationPending
        private bool cancellationPending;

        //Whether the object supports cancelling or not (and progress or not)
        private bool supportsCancellation;
        private bool reportsProgress;

        //Helper objects since Control.Invoke takes no arguments
        private RunWorkerCompletedEventArgs finalResult;
        private ProgressChangedEventArgs progressArgs;

        // Helper for marshalling execution to GUI thread
        private System.Windows.Forms.Control uiControl;


        #region Private Methods
        // Async(ThreadPool) called by ReportProgress for reporting progress
        private void ProgressHelper(object o)
        {
            progressArgs = (ProgressChangedEventArgs)o;//TODO put this in a queue to preserve the userState if the client code call ReportProgress in quick succession

            uiControl.Invoke(new System.EventHandler(OnProgressChanged));
        }
        // ControlInvoked by ProgressHelper for raising progress
        private void OnProgressChanged(object sender, System.EventArgs e)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, progressArgs);
            }
        }

        // Async(ThreadPool) called by RunWorkerAsync [the little engine of this class]
        private void DoTheRealWork(object o)
        {
            // declare/initialise the vars we will pass back to client on completion
            System.Exception error = null;
            bool cancelled = false;
            object result = null;

            // Raise the event passing the original argument and catching any exceptions
            try
            {
                DoWorkEventArgs inOut = new DoWorkEventArgs(o);
                DoWork(this, inOut);

                cancelled = inOut.Cancel;
                result = inOut.Result;
            }
            catch (System.Exception ex)
            {
                error = ex;
            }

            // store the completed final result in a temp var
            RunWorkerCompletedEventArgs tempResult = new RunWorkerCompletedEventArgs(result, error, cancelled);

            // return execution to client by going async here
            System.Threading.ThreadPool.QueueUserWorkItem(
                new System.Threading.WaitCallback(RealWorkHelper), tempResult);

            // prepare for next use
            isBusy = false;
            cancellationPending = false;
        }

        // Async(ThreadPool) called by DoTheRealWork [to avoid any rentrancy issues at the client end]
        private void RealWorkHelper(object o)
        {
            finalResult = (RunWorkerCompletedEventArgs)o;

            uiControl.Invoke(new System.EventHandler(OnRunWorkerCompleted));
        }

        // ControlInvoked by RealWorkHelper for raising final completed event
        private void OnRunWorkerCompleted(object sender, System.EventArgs e)
        {
            if (RunWorkerCompleted != null)
            {
                RunWorkerCompleted(this, finalResult);
            }
        }

        #endregion
    }
}

