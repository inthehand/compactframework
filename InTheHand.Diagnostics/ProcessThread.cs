// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessThread.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Diagnostics
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Represents an operating system process thread.
    /// </summary>
    /// <remarks>
    /// Use ProcessThread to obtain information about a thread that is currently running on the system.
    /// Doing so allows you, for example, to monitor the thread's performance characteristics.
    /// <para>A thread is a path of execution through a program.
    /// It is the smallest unit of execution that Win32 schedules.
    /// It consists of a stack, the state of the CPU registers, and an entry in the execution list of the system scheduler.</para>
    /// <para>A process consists of one or more threads and the code, data, and other resources of a program in memory.
    /// Typical program resources are open files, semaphores, and dynamically allocated memory.
    /// Each resource of a process is shared by all that process's threads.</para>
    /// <para>A program executes when the system scheduler gives execution control to one of the program's threads.
    /// The scheduler determines which threads should run and when.
    /// A lower-priority thread might be forced to wait while higher-priority threads complete their tasks.
    /// On multiprocessor computers, the scheduler can move individual threads to different processors, thus balancing the CPU load.</para>
    /// <para>Each process starts with a single thread, which is known as the primary thread.
    /// Any thread can create additional threads.
    /// All the threads within a process share the address space of that process.</para></remarks>
    public sealed class ProcessThread : Component
    {
        private NativeMethods.THREADENTRY32 te;
        private long creationTime, exitTime, kernelTime, userTime;

        internal ProcessThread(NativeMethods.THREADENTRY32 te)
        {
            this.te = te;
        }

        //refreshes the thread times
        private void GetThreadTimes()
        {
            bool success = NativeMethods.GetThreadTimes(new IntPtr(te.th32ThreadID), out creationTime, out exitTime, out kernelTime, out userTime);
        }

        /// <summary>
        /// Gets the base priority of the thread.
        /// </summary>
        /// <value>The base priority of the thread, which the operating system computes by combining the process priority class with the priority level of the associated thread.</value>
        public int BasePriority
        {
            get
            {
                return te.tpBasePri;
            }
        }

        /// <summary>
        /// Gets the unique identifier of the thread.
        /// </summary>
        /// <value>The unique identifier associated with a specific thread.</value>
        /// <remarks>The operating system reuses thread identification numbers, which identify threads only during their lifetimes.</remarks>
        public int Id
        {
            get
            {
                return te.th32ThreadID;
            }
        }

        /// <summary>
        /// Gets the time that the operating system started the thread.
        /// </summary>
        /// <value>A <see cref="DateTime"/> representing the time that was on the system when the operating system started the thread.</value>
        public DateTime StartTime
        {
            get
            {
                //only get this once as it won't change for a specific thread
                if (creationTime == 0)
                {
                    GetThreadTimes();
                }

                return DateTime.FromFileTimeUtc(creationTime);
            }
        }

        /// <summary>
        /// Gets the amount of time that the thread has spent running code inside the operating system core.
        /// </summary>
        /// <value>A <see cref="TimeSpan"/> indicating the amount of time that the thread has spent running code inside the operating system core.</value>
        public TimeSpan PrivilegedProcessorTime
        {
            get
            {
                GetThreadTimes();
                return new TimeSpan(kernelTime);
            }
        }

        /// <summary>
        /// Gets the amount of time that the associated thread has spent running code inside the application.
        /// </summary>
        /// <value>A <see cref="TimeSpan"/> indicating the amount of time that the thread has spent running code inside the application, as opposed to inside the operating system core.</value>
        public TimeSpan UserProcessorTime
        {
            get
            {
                GetThreadTimes();
                return new TimeSpan(userTime);
            }
        }

        /// <summary>
        /// Gets the total amount of time that this thread has spent using the processor.
        /// </summary>
        /// <value>A <see cref="TimeSpan"/> that indicates the amount of time that the thread has had control of the processor.</value>
        public TimeSpan TotalProcessorTime
        {
            get
            {
                GetThreadTimes();
                return new TimeSpan(kernelTime + userTime);
            }
        }
    }
}
