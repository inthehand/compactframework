// In The Hand - .NET Components for Mobility
//
// InTheHand.Type
// 
// Copyright (c) 2003-2014 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace InTheHand
{
    /// <summary>
    /// Helper for <see cref="System.Type"/>.
    /// </summary>
    public static class TypeInTheHand
    {
        /// <summary>
        /// Gets the GUID associated with the Type.
        /// </summary>
        /// <param name="t">The <see cref="Type"/>.</param>
        /// <returns>The GUID associated with the <see cref="Type"/>.</returns>
        public static Guid GetGUID(this Type t)
        {
            object[] attributes = t.GetCustomAttributes(typeof(GuidAttribute),false);
            foreach (GuidAttribute ga in attributes)
            {
                return new Guid(ga.Value);
            }

            return Guid.Empty;
        }

        /// <summary>
        /// Searches for the interface with the specified name.
        /// </summary>
        /// <param name="instance">The <see cref="Type"/>.</param>
        /// <param name="interfaceName">The String containing the name of the interface to get.
        /// For generic interfaces, this is the mangled name.</param>
        /// <returns>A <see cref="Type"/> object representing the interface with the specified name, implemented or inherited by the current <see cref="Type"/>, if found; otherwise, a null reference (Nothing in Visual Basic).</returns>
        /// <remarks>The search for name is case-sensitive.</remarks>
        public static Type GetInterface(this Type instance, string interfaceName)
        {
            return GetInterface(instance, interfaceName, false);
        }

        /// <summary>
        /// Searches for the specified interface, specifying whether to do a case-insensitive search for the interface name.
        /// </summary>
        /// <param name="instance">The <see cref="Type"/>.</param>
        /// <param name="interfaceName">The <see cref="String"/> containing the name of the interface to get.
        /// For generic interfaces, this is the mangled name.</param>
        /// <param name="ignoreCase">true to ignore the case of that part of name that specifies the simple interface name (the part that specifies the namespace must be correctly cased).
        /// <para>-or-</para>
        /// false to perform a case-sensitive search for all parts of name.</param>
        /// <returns>A <see cref="Type"/> object representing the interface with the specified name, implemented or inherited by the current <see cref="Type"/>, if found; otherwise, a null reference (Nothing in Visual Basic).</returns>
        public static Type GetInterface(this Type instance, string interfaceName, bool ignoreCase)
        {
            if (interfaceName == null)
            {
                throw new ArgumentNullException();
            }

            foreach (Type type in instance.GetInterfaces())
            {
                if (string.Compare(type.Name, interfaceName, ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture) == 0)
                {
                    return type;
                }
            }

            return null;
        }
    }
}