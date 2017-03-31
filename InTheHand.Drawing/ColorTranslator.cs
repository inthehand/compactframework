// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorTranslator.cs" company="In The Hand Ltd">
// Copyright (c) 2002-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace InTheHand.Drawing
{
	/// <summary>
    /// Translates colors to and from GDI+ Color structures.
	/// </summary>
    public static class ColorTranslator
	{
        /// <summary>
        /// Translates a Windows color value to a GDI+ <see cref="Color"/> structure.
        /// </summary>
        /// <param name="win32Color">The Windows color to translate.</param>
        /// <returns>The <see cref="Color"/> structure that represents the translated Windows color.</returns>
        public static Color FromWin32(int win32Color)
        {
            return Color.FromArgb((win32Color & 0xFF), ((win32Color & 0xFF00) >> 8), ((win32Color & 0xFF0000) >> 16));
        }

        /// <summary>
        /// Translates the specified <see cref="Color"/> structure to a Windows color.
        /// </summary>
        /// <param name="c">The <see cref="Color"/> structure to translate.</param>
        /// <returns>The Windows color value.</returns>
        public static int ToWin32(Color c)
        {
            return (int)c.R | ((int)c.G << 8) | ((int)c.B << 16);
		}
	}
}