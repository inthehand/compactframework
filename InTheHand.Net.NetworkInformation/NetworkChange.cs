// In The Hand - .NET Components for Mobility
//
// InTheHand.InTheHand.Net.NetworkInformation.NetworkChange
// 
// Copyright (c) 2010-2011 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Net.NetworkInformation
{
    /// <summary>
    /// Allows applications to receive notification when the Internet Protocol (IP) address of a network interface, also called a network card or adapter, changes.
    /// </summary>
    /// <remarks>
    /// The <see cref="NetworkChange"/> class provides address change notification by raising <see cref="NetworkAddressChanged"/> events.
    /// An interface address can change for many reasons, such as a disconnected network cable, moving out of range of a wireless Local Area Network, or hardware failure.
    /// <para>To receive notification, you must identify your application's event handlers, which are one or more methods that perform your application-specific tasks each time the event is raised.
    /// To have a <see cref="NetworkChange"/> object call your event-handling methods when a <see cref="NetworkAddressChanged"/> event occurs, you must associate the methods with a <see cref="NetworkAddressChangedEventHandler"/> delegate, and add this delegate to the event.</para></remarks>
    public sealed class NetworkChange
    {
        private static NetworkChange instance = new NetworkChange();
        
        private NetworkChange()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        ~NetworkChange()
        {
            if (threadRunning)
            {
                handles[1].Set();
            }
        }

        private static InTheHand.Threading.EventWaitHandle[] handles = new InTheHand.Threading.EventWaitHandle[2];
        private static bool threadRunning = false;

        private void EventThread()
        {
            threadRunning = true;
            //handles[0] = new InTheHand.Threading.EventWaitHandle(false, InTheHand.Threading.EventResetMode.AutoReset);
            handles[1] = new InTheHand.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset);
            IntPtr rawHandle = IntPtr.Zero;
            int result = NativeMethods.NotifyAddrChange(ref rawHandle, IntPtr.Zero);
            handles[0] = new InTheHand.Threading.EventWaitHandle(rawHandle);
            while (threadRunning)
            {
                int handle = InTheHand.Threading.EventWaitHandle.WaitAny(handles);
                switch (handle)
                {
                    case 0:
                        //event triggered
                        if (networkAddressChanged != null)
                        {
                            networkAddressChanged(null, EventArgs.Empty);
                        }
                        break;
                    default:
                        //break the loop
                        threadRunning = false;
                        handles[0].Close();
                        handles[1].Close();
                        return;
                }
            }
        }

        private static event NetworkAddressChangedEventHandler networkAddressChanged;
        /// <summary>
        /// Indicates that the network address has changed.
        /// </summary>
        public static event NetworkAddressChangedEventHandler NetworkAddressChanged
        {
            add
            {
                networkAddressChanged = (NetworkAddressChangedEventHandler)Delegate.Combine(networkAddressChanged, value);
                if (!threadRunning)
                {
                    System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(instance.EventThread));
                    t.IsBackground = true;
                    t.Name = "NetworkChange";
                    t.Start();
                }
            }

            remove
            {
                networkAddressChanged = (NetworkAddressChangedEventHandler)Delegate.Remove(networkAddressChanged, value);
                if (networkAddressChanged == null)
                {
                    if (threadRunning)
                    {
                        // stop thread
                        handles[1].Set();
                    }
                }
            }
        }
    }

    /// <summary>
    /// References one or more methods to be called when the address of a network interface changes.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">An EventArgs object that contains data about the event.</param>
    public delegate void NetworkAddressChangedEventHandler(Object sender, EventArgs e);
}