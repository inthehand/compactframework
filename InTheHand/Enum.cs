// In The Hand - .NET Components for Mobility
//
// InTheHand.Enum
// 
// Copyright (c) 2002-2014 In The Hand Ltd, All rights reserved.

namespace InTheHand
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Extends the functionality of <see cref="System.Enum"/>
    /// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
    /// <item><term>Windows Phone</term><description>Windows Phone 7</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
    /// </list></remarks>
    public static class EnumInTheHand
    {
        /// <summary>
        /// Retrieves an array of the values of the constants in a specified enumeration.
        /// </summary>
        /// <param name="enumType">An enumeration type.</param>
        /// <returns>An array that contains the values of the constants in enumType.
        /// The elements of the array are sorted by the binary values of the enumeration constants.</returns>
        /// <exception cref="ArgumentNullException">enumType is null.</exception>
        /// <exception cref="ArgumentException">enumType parameter is not an <see cref="Enum"/>.</exception>
        public static Array GetValues(Type enumType)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }

            if (enumType.BaseType == typeof(Enum))
            {
                // get the public static fields (members of the enum)
                System.Reflection.FieldInfo[] fi = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);

                // create a new enum array
                System.Enum[] values = new System.Enum[fi.Length];

                // populate with the values
                for (int iEnum = 0; iEnum < fi.Length; iEnum++)
                {
                    values[iEnum] = (System.Enum)fi[iEnum].GetValue(null);
                }

                // return the array
                return values;
            }
            else
            {
                //the type supplied does not derive from enum
                throw new ArgumentException("enumType parameter is not an System.Enum");
            }
        }

        /// <summary>
        /// Determines whether one or more bit fields are set in the current instance.
        /// </summary>
        /// <param name="theEnum">The <see cref="Enum"/> value.</param>
        /// <param name="flag">An enumeration value.</param>
        /// <returns>true if the bit field or bit fields that are set in flag are also set in the current instance; otherwise, false.</returns>
        /// <remarks>The <see cref="HasFlag"/> method returns the result of the following <see cref="Boolean"/> expression.
        /// <code lang="vbnet">thisInstance And flag = flag</code>
        /// If the underlying value of flag is zero, the method returns true.
        /// If this behavior is not desirable, you can use the <see cref="Enum.Equals"/> method to test for equality with zero and call <see cref="HasFlag"/> only if the underlying value of flag is non-zero.
        /// <para>The <see cref="HasFlag"/> method is designed to be used with enumeration types that are marked with the <see cref="FlagsAttribute"/> attribute.
        /// For enumeration types that are not marked with the <see cref="FlagsAttribute"/> attribute, call either the <see cref="Enum.Equals"/> method or the <see cref="Enum.CompareTo"/> method.
        /// </para>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
        /// <item><term>Windows Phone</term><description>Windows Phone 7 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.1 and later</description></item>
        /// </list></remarks>
        /// <exception cref="ArgumentException">flag is a different type than the current instance.</exception>
        public static bool HasFlag(this Enum theEnum, Enum flag)
        {
            if (theEnum.GetType() != flag.GetType())
            {
                throw new ArgumentException();
            }

            switch (theEnum.GetTypeCode())
            {
                case TypeCode.Byte:
                    return (Convert.ToByte(theEnum) & Convert.ToByte(flag)) == Convert.ToByte(flag);

                case TypeCode.SByte:
                    return (Convert.ToSByte(theEnum) & Convert.ToSByte(flag)) == Convert.ToSByte(flag);

                case TypeCode.Int16:
                    return (Convert.ToInt16(theEnum) & Convert.ToInt16(flag)) == Convert.ToInt16(flag);

                case TypeCode.UInt16:
                    return (Convert.ToUInt16(theEnum) & Convert.ToUInt16(flag)) == Convert.ToUInt16(flag);

                case TypeCode.Int32:
                    return (Convert.ToInt32(theEnum) & Convert.ToInt32(flag)) == Convert.ToInt32(flag);

                case TypeCode.UInt32:
                    return (Convert.ToUInt32(theEnum) & Convert.ToUInt32(flag)) == Convert.ToUInt32(flag);

                case TypeCode.Int64:
                    return (Convert.ToInt64(theEnum) & Convert.ToInt64(flag)) == Convert.ToInt64(flag);

                case TypeCode.UInt64:
                    return (Convert.ToUInt64(theEnum) & Convert.ToUInt64(flag)) == Convert.ToUInt64(flag);
            }

            // should never get here for current enum rules
            return false;
        }
    }
}
