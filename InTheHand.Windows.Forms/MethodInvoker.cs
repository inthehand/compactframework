// In The Hand - .NET Components for Mobility
//
// InTheHand.Windows.Forms.MethodInvoker
// 
// Copyright (c) 2007-2012 In The Hand Ltd, All rights reserved.

using System;
using System.Windows.Forms;


namespace InTheHand.Windows.Forms
{
    /// <summary>
    /// Represents a delegate that can execute any method in managed code that is declared void and takes no parameters.
    /// </summary>
    /// <remarks>MethodInvoker provides a simple delegate that is used to invoke a method with a void parameter list.
    /// This delegate can be used when making calls to a control's <see cref="Control.Invoke(System.Delegate)"/> method, or when you need a simple delegate but do not want to define one yourself.</remarks>
    public delegate void MethodInvoker();
}