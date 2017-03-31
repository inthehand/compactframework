// In The Hand - .NET Components for Mobility
//
// InTheHand.Windows.Forms.IWin32Window
// 
// Copyright (c) 2007-2012 In The Hand Ltd, All rights reserved.

using System;

namespace InTheHand.Windows.Forms
{
    /// <summary>
    /// Provides an interface to expose Win32 HWND handles.
    /// </summary>
    public interface IWin32Window
    {
        /// <summary>
        /// Gets the handle to the window represented by the implementer.
        /// </summary>
        /// <value>A handle to the window represented by the implementer.</value>
        /// <remarks>Depending on the implementer, the value of the Handle property could change during the life of the window.</remarks>
        IntPtr Handle { get; }
    }
}
