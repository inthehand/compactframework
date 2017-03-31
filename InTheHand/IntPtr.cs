// In The Hand - .NET Components for Mobility
//
// InTheHand.IntPtr
// 
// Copyright (c) 2010-2014 In The Hand Ltd, All rights reserved.

using System;

namespace InTheHand
{
    /// <summary>
    /// Extends the functionality of <see cref="System.IntPtr"/>.
    /// </summary>
    public static class IntPtrInTheHand
    {
        /// <summary>
        /// Adds an offset to the value of a pointer.
        /// </summary>
        /// <param name="pointer">The pointer to add the offset to.</param>
        /// <param name="offset">The offset to add.</param>
        /// <returns>A new pointer that reflects the addition of offset to pointer.</returns>
        public static IntPtr Add(IntPtr pointer, int offset)
        {
            return new IntPtr(unchecked(pointer.ToInt32() + offset));
        }

        /// <summary>
        /// Subtracts an offset from the value of a pointer.
        /// </summary>
        /// <param name="pointer">The pointer to subtract the offset from.</param>
        /// <param name="offset">The pointer to subtract the offset from.</param>
        /// <returns>A new pointer that reflects the subtraction of offset from pointer.</returns>
        public static IntPtr Subtract(IntPtr pointer, int offset)
        {
            return new IntPtr(unchecked(pointer.ToInt32() - offset));
        }
    }
}