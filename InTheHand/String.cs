// In The Hand - .NET Components for Mobility
//
// InTheHand.String
// 
// Copyright (c) 2010-2014 In The Hand Ltd, All rights reserved.

using System;

namespace InTheHand
{
    /// <summary>
    /// Provides extension methods for the <see cref="String"/> type. 
    /// </summary>
    public static class StringInTheHand
    {
        /// <summary>
        /// Returns a copy of the <see cref="String"/> object converted to uppercase using the casing rules of the invariant culture.
        /// </summary>
        /// <param name="instance">The <see cref="String"/> value.</param>
        /// <returns>A <see cref="String"/> object in uppercase.</returns>
        /// <remarks>If your application depends on the case of a string changing in a predictable way that is unaffected by the current culture, use the ToUpperInvariant method.
        /// The ToUpperInvariant method is equivalent to <see cref="String.ToUpper(System.Globalization.CultureInfo)">ToUpper(CultureInfo.InvariantCulture)</see>.</remarks>
        public static string ToUpperInvariant(this string instance)
        {
            return instance.ToUpper(System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns a copy of the <see cref="String"/> object converted to lowercase using the casing rules of the invariant culture.
        /// </summary>
        /// <param name="instance">The <see cref="String"/> value.</param>
        /// <returns>A <see cref="String"/> object in lowercase.</returns>
        /// <remarks>If your application depends on the case of a string changing in a predictable way that is unaffected by the current culture, use the ToLowerInvariant method.
        /// The ToLowerInvariant method is equivalent to <see cref="String.ToLower(System.Globalization.CultureInfo)">ToLower(CultureInfo.InvariantCulture)</see>.</remarks>
        public static string ToLowerInvariant(this string instance)
        {
            return instance.ToLower(System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}