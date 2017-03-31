// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RECT.cs" company="In The Hand Ltd">
// Copyright (c) 2007-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Drawing
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        public int left;

        public int top;

        public int right;

        public int bottom;

        internal RectangleF ToRectangleF()
        {
            return new RectangleF(this.left, this.top, this.right - this.left, this.bottom - this.top);
        }

        internal Rectangle ToRectangle()
        {
            return new Rectangle(this.left, this.top, this.right - this.left, this.bottom - this.top);
        }

        internal static RECT FromXYWH(int x, int y, int width, int height)
        {
            RECT r;
            r.left = x;
            r.top = y;
            r.right = x + width;
            r.bottom = y + height;

            return r;
        }

        public override string ToString()
        {
            return left.ToString(CultureInfo.InvariantCulture) + ", " + top.ToString(CultureInfo.InvariantCulture)
                   + ", " + right.ToString(CultureInfo.InvariantCulture) + ", "
                   + bottom.ToString(CultureInfo.InvariantCulture);
        }
    }
}