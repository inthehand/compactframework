// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearGradientMode.cs" company="In The Hand Ltd">
// Copyright (c) 2008-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Drawing.Drawing2D
{
    /// <summary>
    /// Specifies the direction of a linear gradient.
    /// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.2 and later</description></item>
    /// </list>
    /// </remarks>
    public enum LinearGradientMode
    {
        /// <summary>
        /// Specifies a gradient from left to right.
        /// </summary>
        Horizontal = 0x00000000,

        /// <summary>
        /// Specifies a gradient from top to bottom.
        /// </summary>
        Vertical = 0x00000001,
    }
}
