// In The Hand - .NET Components for Mobility
//
// InTheHand.Guid
// 
// Copyright (c) 2003-2014 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace InTheHand
{
	/// <summary>
	/// Helper class for generating a globally unique identifier (GUID).
	/// </summary>
	/// <seealso cref="System.Guid"/>
    /// <remarks>This method uses the native COM sub-system to generate a Guid rather than a managed random number generator offering significantly better performance than Guid.NewGuid().</remarks>
    public static class GuidInTheHand
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="System.Guid"/> class.
		/// </summary>
		/// <returns>A new <see cref="System.Guid"/> object</returns>
        /// <remarks>This is a convenient static method that you can call to get a new <see cref="System.Guid"/>.
        /// <para>There is a very low probability that the value of the new <see cref="System.Guid"/> is all zeroes or equal to any other <see cref="System.Guid"/>.</para></remarks>
        public static Guid NewGuid()
        {
            System.Guid val;

            int hresult = CoCreateGuid(out val);

            Marshal.ThrowExceptionForHR(hresult);

            return val;
        }

        [DllImport("ole32", EntryPoint = "CoCreateGuid", SetLastError = false)]
        private static extern int CoCreateGuid(out System.Guid pguid);
	}
}