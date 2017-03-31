// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearGradientBrush.cs" company="In The Hand Ltd">
// Copyright (c) 2008-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Drawing;

namespace InTheHand.Drawing.Drawing2D
{
    /// <summary>
    /// Encapsulates a <see cref="Brush"/> with a linear gradient.
    /// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.2 and later</description></item>
    /// </list>
    /// </remarks>
    public sealed class LinearGradientBrush : IDisposable
    {
        internal Rectangle rectangle;
        internal Color[] colors;
        internal LinearGradientMode gradientMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGradientBrush"/> class with the specified points and colors.
        /// </summary>
        /// <param name="rect">A Rectangle structure that specifies the bounds of the linear gradient.</param>
        /// <param name="color1">A <see cref="Color"/> structure that represents the starting color for the gradient.</param>
        /// <param name="color2">A <see cref="Color"/> structure that represents the ending color for the gradient.</param>
        /// <param name="linearGradientMode">A <see cref="LinearGradientMode"/> enumeration element that specifies the orientation of the gradient.
        /// The orientation determines the starting and ending points of the gradient.</param>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.2 and later</description></item>
        /// </list>
        /// </remarks>
        public LinearGradientBrush(Rectangle rect, Color color1, Color color2, LinearGradientMode linearGradientMode)
        {
            rectangle = rect;
            colors = new Color[2] { color1, color2 };
            gradientMode = linearGradientMode;
        }

        /// <summary>
        /// Initializes a new instance of the LinearGradientBrush class with the specified points and colors.
        /// </summary>
        /// <param name="rect">A RectangleF structure that specifies the bounds of the linear gradient.</param>
        /// <param name="color1">A Color structure that represents the starting color for the gradient.</param>
        /// <param name="color2">A Color structure that represents the ending color for the gradient.</param>
        /// <param name="linearGradientMode">A <see cref="LinearGradientMode"/> enumeration element that specifies the orientation of the gradient.
        /// The orientation determines the starting and ending points of the gradient.</param>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.2 and later</description></item>
        /// </list>
        /// </remarks>
        public LinearGradientBrush(RectangleF rect, Color color1, Color color2, LinearGradientMode linearGradientMode)
        {
            rectangle = new Rectangle(Convert.ToInt32(rect.X), Convert.ToInt32(rect.Y), Convert.ToInt32(rect.Width), Convert.ToInt32(rect.Height));
            colors = new Color[2] { color1, color2 };
            gradientMode = linearGradientMode;
        }

        /// <summary>
        /// A <see cref="RectangleF"/> structure that specifies the starting and ending points of the gradient.
        /// </summary>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.2 and later</description></item>
        /// </list>
        /// </remarks>
        public RectangleF Rectangle { get { return new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height); } }

        private IntPtr hbrush = IntPtr.Zero;
        private Image bTemp;

        internal IntPtr GetHBrush()
        {
            if (hbrush == IntPtr.Zero)
            {
                //draw gradient into buffer bitmap
                bTemp = new Bitmap(rectangle.Width, rectangle.Height);
                Graphics gb = Graphics.FromImage(bTemp);

                IntPtr hdc = gb.GetHdc();
                try
                {
                    NativeMethods.TRIVERTEX[] tv = new NativeMethods.TRIVERTEX[2];
                    tv[0] = new NativeMethods.TRIVERTEX();

                    tv[0].Red = (ushort)(colors[0].R << 8);
                    tv[0].Green = (ushort)(colors[0].G << 8);
                    tv[0].Blue = (ushort)(colors[0].B << 8);
                    tv[0].Alpha = (ushort)(colors[0].A << 8);

                    tv[1] = new NativeMethods.TRIVERTEX();

                    tv[1].Red = (ushort)(colors[1].R << 8);
                    tv[1].Green = (ushort)(colors[1].G << 8);
                    tv[1].Blue = (ushort)(colors[1].B << 8);
                    tv[1].Alpha = (ushort)(colors[1].A << 8);


                    tv[0].x = 0;
                    tv[0].y = 0;
                    tv[1].x = rectangle.Width;
                    tv[1].y = rectangle.Height;


                    NativeMethods.GRADIENT_RECT gr = new NativeMethods.GRADIENT_RECT();
                    gr.UpperLeft = 0;
                    gr.LowerRight = 1;

                    bool success = NativeMethods.GdiGradientFill(hdc, tv, 2, ref gr, 1, gradientMode);
                }
                finally
                {
                    gb.ReleaseHdc(hdc);
                    gb.Dispose();
                }

                //get native handle for temporary bitmap
                IntPtr hBitmap = ((Bitmap)bTemp).GetHbitmap();
                //create native brush from temporary bitmap
                hbrush = NativeMethods.GdiCreatePatternBrush(hBitmap);
                bTemp.Dispose();
            }

            return hbrush;
        }

        #region IDisposable Members

        private void Dispose(bool disposing)
        {
            if (hbrush != IntPtr.Zero)
            {
                InTheHand.Drawing.NativeMethods.GdiDeleteObject(hbrush);
                hbrush = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Releases all resources used by this <see cref="LinearGradientBrush"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Allows a <see cref="LinearGradientBrush"/> to attempt to free resources and perform other cleanup operations before the <see cref="LinearGradientBrush"/> is reclaimed by garbage collection.
        /// </summary>
        ~LinearGradientBrush()
        {
            Dispose(false);
        }
        #endregion
    }
}
