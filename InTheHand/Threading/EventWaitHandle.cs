// In The Hand - .NET Components for Mobility
//
// InTheHand.Threading.EventWaitHandle
// 
// Copyright (c) 2003-2014 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace InTheHand.Threading
{
    /// <summary>
	/// Represents a thread synchronization event.
	/// </summary>
    /// <remarks>The <see cref="EventWaitHandle"/> class allows threads to communicate with each other by signaling.
    /// Typically, one or more threads block on an <see cref="EventWaitHandle"/> until an unblocked thread calls the <see cref="Set"/> method, releasing one or more of the blocked threads.</remarks>
	public sealed class EventWaitHandle : WaitHandle
    {

#if !C2013
        /// <summary>
        /// Indicates that a <see cref="WaitAny(WaitHandle[])"/> operation timed out before any of the wait handles were signaled.
        /// </summary>
        public const int WaitTimeout = 0x102;
#endif

        /// <summary>
		/// Opens an existing named synchronization event.
		/// </summary>
		/// <param name="name">The name of a system event.</param>
		/// <returns>A <see cref="EventWaitHandle"/> object that represents the named system event.</returns>
        /// <exception cref="ArgumentException">name is an empty string. 
        /// -or-
        /// name is longer than 260 characters.</exception>
        /// <exception cref="ArgumentNullException">name is a null reference (Nothing in Visual Basic).</exception>
		/// <exception cref="WaitHandleCannotBeOpenedException">The named system event does not exist.</exception>
        /// <remarks>The OpenExisting method attempts to open an existing named system event.
        /// If the system event does not exist, this method throws an exception instead of creating the system event.
        /// Two calls to this method with the same value for name do not return the same <see cref="EventWaitHandle"/> object, even though they represent the same named system event.</remarks>
        public static EventWaitHandle OpenExisting(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (name.Length > InTheHand.EnvironmentInTheHand.MaxPath)
            {
                throw new ArgumentException(Properties.Resources.Argument_WaitHandleNameTooLong, "name");
            }

            IntPtr handle = NativeMethods.CreateEvent(IntPtr.Zero, false, false, name);
            
            if (Marshal.GetLastWin32Error() != NativeMethods.ERROR_ALREADY_EXISTS)
            {
                throw new WaitHandleCannotBeOpenedException();
            }

			return new EventWaitHandle(handle);
        }

        /// <summary>
		/// Sets the state of the event to nonsignaled, causing threads to block.
		/// </summary>
		/// <returns>true if the function succeeds; otherwise, false.</returns>
		public bool Reset()
        {
			return NativeMethods.EventModify(this.Handle, NativeMethods.EVENT.RESET);
        }

        /// <summary>
		/// Sets the state of the event to signaled, allowing one or more waiting threads to proceed.
		/// </summary>
		/// <returns>true if the function succeeds; otherwise, false.</returns>
		public bool Set()
        {
			return NativeMethods.EventModify(this.Handle, NativeMethods.EVENT.SET);
        }

        /// <summary>
		/// Initializes a newly created <see cref="EventWaitHandle"/> object, specifying whether the wait 
		/// handle is initially signaled, and whether it resets automatically or manually.
		/// </summary>
		/// <param name="initialState">true to set the initial state to signaled, false to set it to nonsignaled.</param>
		/// <param name="mode">An EventResetMode value that determines whether the event resets automatically or manually.</param>
        /// <remarks>If name is a null reference (Nothing in Visual Basic) or an empty string, a local <see cref="EventWaitHandle"/> is created.
        /// If a system event with the name specified for the name parameter already exists, the initialState parameter is ignored.</remarks>
		public EventWaitHandle(bool initialState, EventResetMode mode) : this(initialState, mode, null){}

		/// <summary>
		/// Initializes a newly created <see cref="EventWaitHandle"/> object, specifying whether the wait handle is initially signaled, whether it resets automatically or manually, and the name of a system synchronization event.
		/// </summary>
		/// <param name="initialState">true to set the initial state to signaled, false to set it to nonsignaled.</param>
        /// <param name="mode">An <see cref="EventResetMode"/> value that determines whether the event resets automatically or manually.</param>
		/// <param name="name">The name of a system-wide synchronization event.</param>
        /// <remarks>If name is a null reference (Nothing in Visual Basic) or an empty string, a local <see cref="EventWaitHandle"/> is created.
        /// If a system event with the name specified for the name parameter already exists, the initialState parameter is ignored.</remarks>
        public EventWaitHandle(bool initialState, EventResetMode mode, string name) : this(NativeMethods.CreateEvent(IntPtr.Zero, mode == EventResetMode.ManualReset, initialState, name)) { }

		/// <summary>
		/// Initializes a newly created <see cref="EventWaitHandle"/> object, specifying whether the wait handle is initially signaled, whether it resets automatically or manually, the name of a system synchronization event, and a bool variable whose value after the call indicates whether the named system event was created.
		/// </summary>
		/// <param name="initialState">true to set the initial state to signaled, false to set it to nonsignaled.</param>
		/// <param name="mode">An <see cref="EventResetMode"/> value that determines whether the event resets automatically or manually.</param>
		/// <param name="name">The name of a system-wide synchronization event.</param>
		/// <param name="createdNew">When this method returns, contains true if the calling thread was granted initial ownership of the named system event; otherwise, false. This parameter is passed uninitialized.</param>
        /// <remarks>If name is a null reference (Nothing in Visual Basic) or an empty string, a local <see cref="EventWaitHandle"/> is created.
        /// If a system event with the name specified for the name parameter already exists, the initialState parameter is ignored.</remarks>
        public EventWaitHandle(bool initialState, EventResetMode mode, string name, out bool createdNew)
            : this(initialState, mode, name)
        {
			createdNew = (Marshal.GetLastWin32Error() == NativeMethods.ERROR_ALREADY_EXISTS);
        }

#if !C2013
        /// <summary>
        /// Waits for any of the elements in the specified array to receive a signal.
        /// </summary>
        /// <param name="waitHandles">A <see cref="WaitHandle"/> array containing the objects for which the current instance will wait.</param>
        /// <returns>The array index of the object that satisfied the wait.</returns>
        public static int WaitAny(WaitHandle[] waitHandles)
        {
            return WaitAny(waitHandles, Timeout.Infinite, false);
        }

        /// <summary>
        /// Waits for any of the elements in the specified array to receive a signal, using a 32-bit signed integer to measure the time interval, and specifying whether to exit the synchronization domain before the wait.
        /// </summary>
        /// <param name="waitHandles">A <see cref="WaitHandle"/> array containing the objects for which the current instance will wait.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="Timeout.Infinite"/> (-1) to wait indefinitely.</param>
        /// <param name="exitContext">Not supported.</param>
        /// <returns>The array index of the object that satisfied the wait, or <see cref="WaitTimeout"/> if no object satisfied the wait and a time interval equivalent to millisecondsTimeout has passed.</returns>
        public static int WaitAny(WaitHandle[] waitHandles, int millisecondsTimeout, bool exitContext)
        {
            IntPtr[] handles = new IntPtr[waitHandles.Length];
            for (int i = 0; i < handles.Length; i++)
            {
                handles[i] = waitHandles[i].Handle;
            }

            return NativeMethods.WaitForMultipleObjects(handles.Length, handles, false, millisecondsTimeout);
        }
#endif

        /// <summary>
		/// Blocks the current thread until the current <see cref="WaitHandle"/> receives a signal.
		/// </summary>
		/// <returns>true if the current instance receives a signal. if the current instance is never signaled, <see cref="WaitOne(Int32,bool)"/> never returns.</returns>
		public override bool WaitOne()
        {
			return WaitOne(-1, false);
		}

		/// <summary>
        /// Blocks the current thread until the current <see cref="WaitHandle"/> receives a signal, using 32-bit signed integer to measure the time interval and specifying whether to exit the synchronization domain before the wait.
		/// </summary>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="Timeout.Infinite"/> (-1) to wait indefinitely.</param>
		/// <param name="exitContext">Not Supported - Just pass false.</param>
		/// <returns>true if the current instance receives a signal; otherwise, false.</returns>
        public override bool WaitOne(Int32 millisecondsTimeout, bool exitContext)
        {
			return (NativeMethods.WaitForSingleObject(this.Handle, millisecondsTimeout) != EventWaitHandle.WaitTimeout);
        }

        /// <summary>
        /// releases all resources held by the current <see cref="EventWaitHandle"/>.
		/// </summary>
        public override void Close(){
			GC.SuppressFinalize(this);
            if (this.Handle != WaitHandle.InvalidHandle)
            {
                InTheHand.NativeMethods.CloseHandle(this.Handle);
                this.Handle = WaitHandle.InvalidHandle;
            }
        }

        internal EventWaitHandle(IntPtr handle): base()
        {
            if (handle == IntPtr.Zero)
            {
                throw new WaitHandleCannotBeOpenedException();
            }
 
			this.Handle = handle;
		}

        /// <summary>
        /// Allows an <see cref="EventWaitHandle"/> to attempt to free resources and perform other cleanup operations before the <see cref="EventWaitHandle"/> is reclaimed by garbage collection.
        /// </summary>
		~EventWaitHandle()
        {
            Close();
		}

        private static class NativeMethods
        {
            internal const int MAX_PATH = 260;

            internal const int WAIT_FAILED = -1;
            internal const int EVENT_ALL_ACCESS = 0x3;
            internal const int ERROR_INVALID_HANDLE = 0x6;
            internal const int ERROR_ALREADY_EXISTS = 183;

            internal enum EVENT
            {
                PULSE = 1,
                RESET = 2,
                SET = 3,
            }

            [DllImport("coredll", EntryPoint = "EventModify", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool EventModify(IntPtr hEvent, EVENT ef);

            [DllImport("coredll", EntryPoint = "WaitForSingleObject", SetLastError = true)]
            internal static extern Int32 WaitForSingleObject(IntPtr handle, int milliseconds);

            [DllImport("coredll", EntryPoint = "WaitForMultipleObjects", SetLastError = true)]
            internal static extern int WaitForMultipleObjects(int count, IntPtr[] handles, bool waitAll, int milliseconds);

            [DllImport("coredll", EntryPoint = "CreateEvent", SetLastError = true)]
            internal static extern IntPtr CreateEvent(IntPtr eventAttributes, bool manualReset, bool initialState, string name);
        }
	}
}