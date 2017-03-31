// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.NetworkInformation.NetworkInformationException
// 
// Copyright (c) 2003-2010 In The Hand Ltd, All rights reserved.

using System.ComponentModel;
using System.Runtime.InteropServices;

namespace InTheHand.Net.NetworkInformation
{
    /// <summary>
    /// The exception that is thrown when an error occurs while retrieving network information.
    /// </summary>
    /// <remarks>Types in the InTheHand.Net.NetworkInformation namespace throw this exception when a call to a Win32 function fails.
    /// The <see cref="ErrorCode"/> property contains the result returned by the function.</remarks>
    public class NetworkInformationException : Win32Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkInformationException"/> class.
        /// </summary>
        public NetworkInformationException() : base(Marshal.GetLastWin32Error(), InTheHand.ComponentModel.Win32ExceptionInTheHand.GetErrorMessage(Marshal.GetLastWin32Error())) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkInformationException"/> class with the specified error code.
        /// </summary>
        /// <param name="errorCode">A Win32 error code.</param>
        public NetworkInformationException(int errorCode) : base(errorCode, InTheHand.ComponentModel.Win32ExceptionInTheHand.GetErrorMessage(errorCode)) { }

        /// <summary>
        /// Gets the <b>Win32</b> error code for this exception.
        /// </summary>
        /// <remarks>The value of this property is set by the constructor.
        /// This property is overridden to return a <b>Win32</b> error code instead of an HRESULT value.</remarks>
        public override int ErrorCode
        {
            get
            {
                return base.NativeErrorCode;
            }
        }
    }
}
