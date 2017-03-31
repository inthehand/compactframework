// In The Hand - .NET Components for Mobility
//
// InTheHand.Decimal
// 
// Copyright (c) 2003-2014 In The Hand Ltd, All rights reserved.

namespace InTheHand
{
    using System;

	/// <summary>
	/// Provides helper methods to allow conversion of managed <see cref="System.Decimal"/> objects to and from their OLE Automation equivalents.
	/// </summary>
    /// <seealso cref="System.Decimal"/>
    public static class DecimalInTheHand
    {
        /// <summary>
		/// Converts the specified 64-bit signed integer, which contains an OLE Automation Currency value, to the equivalent <see cref="System.Decimal"/> value.
		/// </summary>
		/// <param name="cy">An OLE Automation Currency value</param>
		/// <returns>A <see cref="System.Decimal"/> that contains the equivalent of <paramref name="cy"/>.</returns>
        public static decimal FromOACurrency(long cy)
		{
			return new decimal(cy)/ 10000;
		}

		/// <summary>
		/// Converts the specified <see cref="System.Decimal"/> value to the equivalent OLE Automation Currency value, which is contained in a 64-bit signed integer.
		/// </summary>
		/// <param name="value">A <see cref="System.Decimal"/> value</param>
		/// <returns>A 64-bit signed integer that contains the OLE Automation equivalent of value.</returns>
        public static long ToOACurrency(decimal value)
		{
            return decimal.ToInt64((value * 10000));
		}
	}
}