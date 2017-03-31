// In The Hand - .NET Components for Mobility
//
// InTheHand.Phone.Tasks.ChooserBase
// 
// Copyright (c) 2010 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Phone.Tasks
{
    /// <summary>
    /// The base class from which all choosers are derived.
    /// This class exposes a common function for showing the choosers and an event for handling the chooser result.
    /// </summary>
    /// <typeparam name="TTaskEventArgs">The generic parameter for the EventArgs object for each type of chooser.</typeparam>
    public abstract class ChooserBase<TTaskEventArgs> where TTaskEventArgs : TaskEventArgs 
    {
        private bool showing = false;

        /// <summary>
        /// The EventArgs for the <see cref="Completed"/> event.
        /// </summary>
        public TTaskEventArgs TaskEventArgs
        {
            get;
            set;
        }

        /// <summary>
        /// Launches and displays the chooser.
        /// </summary>
        public virtual void Show()
        {
            if (showing)
            {
                throw new InvalidOperationException();
            }

            showing = true;
        }

        /// <summary>
        /// Raises the <see cref="Completed"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> associated with the event.</param>
        /// <param name="fireThisHandlerOnly">The delegate to be called when the event is raised.</param>
        protected void FireCompleted(object sender, TTaskEventArgs e, Delegate fireThisHandlerOnly)
        {
            showing = false;
            
            TaskEventArgs = e;

            if (fireThisHandlerOnly != null)
            {
                // fire specific delegate
                ((EventHandler<TTaskEventArgs>)fireThisHandlerOnly)(sender, e);
            }
            else
            {
                if (Completed != null)
                {
                    Completed(sender, e);
                }
            }
        }

        /// <summary>
        /// Occurs when a chooser task is completed.
        /// </summary>
        public event EventHandler<TTaskEventArgs> Completed;
    }
}