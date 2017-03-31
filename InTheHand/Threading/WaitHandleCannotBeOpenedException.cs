// In The Hand - .NET Components for Mobility
//
// InTheHand.Threading.WaitHandleCannotBeOpenedException
// 
// Copyright (c) 2003-2009 In The Hand Ltd, All rights reserved.

#region Using directives

using System;

#endregion

namespace InTheHand.Threading
{
    /// <summary>
    /// The exception that is thrown when an attempt is made to open a system named sychronization event that does not exist.
    /// </summary>
    /// <remarks>Instances of the <see cref="EventWaitHandle"/> class can represent named system synchronization objects.
    /// When you use the <see cref="EventWaitHandle.OpenExisting"/> method to open a named system object that does not exist, a WaitHandleCannotBeOpenedException is thrown.</remarks>
    public sealed class WaitHandleCannotBeOpenedException : ApplicationException
    {
    }
}
