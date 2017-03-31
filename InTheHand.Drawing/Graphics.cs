// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Graphics.cs" company="In The Hand Ltd">
// Copyright (c) 2008-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using InTheHand.Drawing.Drawing2D;

namespace InTheHand.Drawing
{
    /// <summary>
    /// Provides supporting methods for <see cref="Graphics"/>.
    /// </summary>
    /// <seealso cref="Graphics"/>
    public static class GraphicsInTheHand
    {
        static GraphicsInTheHand()
        {
            try
            {
                dpiX = NativeMethods.GdiGetDeviceCaps(IntPtr.Zero, /*LOGPIXELSX*/88);
                dpiY = NativeMethods.GdiGetDeviceCaps(IntPtr.Zero, /*LOGPIXELSY*/90);
            }
            catch
            {
                dpiX = 96;
                dpiY = 96;
            }
        }

        /// <summary>
        /// Performs a bit-block transfer of color data, corresponding to a rectangle of pixels, from the screen to the drawing surface of the Graphics.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="upperLeftSource">The point at the upper-left corner of the source rectangle.</param>
        /// <param name="upperLeftDestination">The point at the upper-left corner of the destination rectangle.</param>
        /// <param name="blockRegionSize">The size of the area to be transferred</param>
        public static void CopyFromScreen(
            this Graphics g,
            Point upperLeftSource,
            Point upperLeftDestination,
            Size blockRegionSize)
        {
            CopyFromScreen(g, upperLeftSource, upperLeftDestination, blockRegionSize, CopyPixelOperation.SourceCopy);
        }

        /// <summary>
        /// Performs a bit-block transfer of color data, corresponding to a rectangle of pixels, from the screen to the drawing surface of the Graphics.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="upperLeftSource">The point at the upper-left corner of the source rectangle.</param>
        /// <param name="upperLeftDestination">The point at the upper-left corner of the destination rectangle.</param>
        /// <param name="blockRegionSize">The size of the area to be transferred</param>
        /// <param name="copyPixelOperation">One of the <see cref="CopyPixelOperation"/> values.</param>
        public static void CopyFromScreen(
            this Graphics g,
            Point upperLeftSource,
            Point upperLeftDestination,
            Size blockRegionSize,
            CopyPixelOperation copyPixelOperation)
        {
            CopyFromScreen(g, upperLeftSource.X, upperLeftSource.Y, upperLeftDestination.X, upperLeftDestination.Y, blockRegionSize, copyPixelOperation);
        }

        /// <summary>
        /// Performs a bit-block transfer of the color data, corresponding to a rectangle of pixels, from the screen to the drawing surface of the Graphics.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="sourceX">The x-coordinate of the point at the upper-left corner of the source rectangle.</param>
        /// <param name="sourceY">The y-coordinate of the point at the upper-left corner of the source rectangle</param>
        /// <param name="destinationX">The x-coordinate of the point at the upper-left corner of the destination rectangle.</param>
        /// <param name="destinationY">The y-coordinate of the point at the upper-left corner of the destination rectangle.</param>
        /// <param name="blockRegionSize">The size of the area to be transferred.</param>
        /// <remarks>The CopyFromScreen methods are useful for layering one image on top of another.</remarks>
        public static void CopyFromScreen(
            this Graphics g,
            int sourceX,
            int sourceY,
            int destinationX,
            int destinationY,
            Size blockRegionSize)
        {
            CopyFromScreen(g, sourceX, sourceY, destinationX, destinationY, blockRegionSize, CopyPixelOperation.SourceCopy);
        }

        /// <summary>
        /// Performs a bit-block transfer of the color data, corresponding to a rectangle of pixels, from the screen to the drawing surface of the Graphics.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="sourceX">The x-coordinate of the point at the upper-left corner of the source rectangle.</param>
        /// <param name="sourceY">The y-coordinate of the point at the upper-left corner of the source rectangle</param>
        /// <param name="destinationX">The x-coordinate of the point at the upper-left corner of the destination rectangle.</param>
        /// <param name="destinationY">The y-coordinate of the point at the upper-left corner of the destination rectangle.</param>
        /// <param name="blockRegionSize">The size of the area to be transferred.</param>
        /// <param name="copyPixelOperation">One of the <see cref="CopyPixelOperation"/> values.</param>
        /// <remarks>The CopyFromScreen methods are useful for layering one image on top of another.
        /// The copyPixelOperation parameter allows you to specify if and how the source colors should be blended with the colors in the destination area.</remarks>
        public static void CopyFromScreen(
            this Graphics g,
            int sourceX,
            int sourceY,
            int destinationX,
            int destinationY,
            Size blockRegionSize,
            CopyPixelOperation copyPixelOperation)
        {
            IntPtr screenHdc = NativeMethods.GdiGetWindowDC(IntPtr.Zero);
            IntPtr hdc = g.GetHdc();
            bool success = NativeMethods.GdiBitBlt(hdc, destinationX, destinationY, blockRegionSize.Width, blockRegionSize.Height, screenHdc, sourceX, sourceY, copyPixelOperation);
            NativeMethods.GdiReleaseDC(IntPtr.Zero, screenHdc);
            g.ReleaseHdc(hdc);
        }

        /// <summary>
        /// Fills the interior of a rectangle with a horizontal or vertical gradient.
        /// </summary>
        /// <param name="g">Graphics instance to use.</param>
        /// <param name="brush"><see cref="LinearGradientBrush"/> that determines the characteristics of the fill. </param>
        /// <param name="rect">Rectangle structure that represents the rectangle to fill.</param>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE 5.0 and later</description></item>
        /// </list>
        /// </remarks>
        public static void FillRectangle(this Graphics g, LinearGradientBrush brush, Rectangle rect)
        {
            if (brush.gradientMode == LinearGradientMode.Horizontal && brush.rectangle.X > rect.X)
            {
                g.FillRectangle(new SolidBrush(brush.colors[0]), new Rectangle(rect.X, rect.Y, brush.rectangle.X, rect.Bottom));
            }
            else if (brush.gradientMode == LinearGradientMode.Vertical && brush.rectangle.Y > rect.Y)
            {
                g.FillRectangle(new SolidBrush(brush.colors[0]), new Rectangle(rect.X, rect.Y, rect.Right, brush.rectangle.Bottom));
            }

            IntPtr hdc = g.GetHdc();
            NativeMethods.TRIVERTEX[] tv = new NativeMethods.TRIVERTEX[2];
            tv[0] = new NativeMethods.TRIVERTEX();

            tv[0].Red = (ushort)(brush.colors[0].R << 8);
            tv[0].Green = (ushort)(brush.colors[0].G << 8);
            tv[0].Blue = (ushort)(brush.colors[0].B << 8);
            tv[0].Alpha = (ushort)(brush.colors[0].A << 8);

            tv[1] = new NativeMethods.TRIVERTEX();

            tv[1].Red = (ushort)(brush.colors[1].R << 8);
            tv[1].Green = (ushort)(brush.colors[1].G << 8);
            tv[1].Blue = (ushort)(brush.colors[1].B << 8);
            tv[1].Alpha = (ushort)(brush.colors[1].A << 8);

            if (brush.gradientMode == LinearGradientMode.Horizontal)
            {
                tv[0].x = brush.rectangle.X;
                tv[0].y = rect.Y;
                tv[1].x = brush.rectangle.Right;
                tv[1].y = rect.Bottom;
            }
            else
            {
                tv[0].x = rect.X;
                tv[0].y = brush.rectangle.Y;
                tv[1].x = rect.Right;
                tv[1].y = brush.rectangle.Bottom;
            }

            NativeMethods.GRADIENT_RECT gr = new NativeMethods.GRADIENT_RECT();
            gr.UpperLeft = 0;
            gr.LowerRight = 1;

            bool success = NativeMethods.GdiGradientFill(hdc, tv, 2, ref gr, 1, brush.gradientMode);
            g.ReleaseHdc(hdc);

            if (brush.gradientMode == LinearGradientMode.Horizontal && brush.rectangle.Right < rect.Right)
            {
                g.FillRectangle(new SolidBrush(brush.colors[1]), new Rectangle(brush.rectangle.Right, rect.Y, rect.Right - brush.rectangle.Right, rect.Height));
            }
            else if (brush.gradientMode == LinearGradientMode.Vertical && brush.rectangle.Bottom < rect.Bottom)
            {
                g.FillRectangle(new SolidBrush(brush.colors[1]), new Rectangle(rect.X, brush.rectangle.Bottom, rect.Width, rect.Bottom - brush.rectangle.Bottom));
            }
        }

        /// <summary>
        /// Measures the specified string when drawn with the specified <see cref="Font"/> within the specified layout area.
        /// </summary>
        /// <param name="g">Graphics instance.</param>
        /// <param name="text">String to measure.</param>
        /// <param name="font"><see cref="Font"/> defines the text format of the string.</param>
        /// <param name="layoutArea">SizeF structure that specifies the maximum layout area for the text.</param>
        /// <returns>This method returns a <see cref="SizeF"/> structure that represents the size, in the units specified by the PageUnit property, of the string specified by the text parameter as drawn with the font parameter.</returns>
        public static SizeF MeasureString(this Graphics g, string text, Font font, SizeF layoutArea)
        {
            return MeasureString(g, text, font, layoutArea, null);
        }

        /// <summary>
        /// Measures the specified string when drawn with the specified <see cref="Font"/> and formatted with the specified <see cref="StringFormat"/>.
        /// </summary>
        /// <param name="g">Graphics instance.</param>
        /// <param name="text">String to measure.</param>
        /// <param name="font"><see cref="Font"/> defines the text format of the string.</param>
        /// <param name="layoutArea">SizeF structure that specifies the maximum layout area for the text.</param>
        /// <param name="stringFormat"><see cref="StringFormat"/> that represents formatting information, such as line spacing, for the string.</param>
        /// <returns>This method returns a <see cref="SizeF"/> structure that represents the size, in the units specified by the PageUnit property, of the string specified by the text parameter as drawn with the font parameter.</returns>
        public static SizeF MeasureString(this Graphics g, string text, Font font, SizeF layoutArea, StringFormat stringFormat)
        {
            IntPtr hdc = g.GetHdc();
            RECT r = new RECT();
            r.right = Convert.ToInt32(layoutArea.Width);
            r.bottom = Convert.ToInt32(layoutArea.Height);
            NativeMethods.DT dt = NativeMethods.DT.CALCRECT | NativeMethods.DT.NOCLIP;
            if (stringFormat != null)
            {
                switch (stringFormat.Alignment)
                {
                    case StringAlignment.Near:
                        dt |= NativeMethods.DT.LEFT;
                        break;
                    case StringAlignment.Center:
                        dt |= NativeMethods.DT.CENTER;
                        break;
                    case StringAlignment.Far:
                        dt |= NativeMethods.DT.RIGHT;
                        break;
                }
            }

            IntPtr hfont = font.ToHfont();
            NativeMethods.GdiSelectObject(hdc, hfont);
            int result = NativeMethods.GdiDrawText(hdc, text, -1, ref r, dt);

            g.ReleaseHdc(hdc);

            layoutArea.Width = r.right;
            layoutArea.Height = r.bottom;
            return layoutArea;
        }

        /// <summary>
        /// Draws a rectangle with rounded corners.
        /// </summary>
        /// <param name="g">The Graphics instance to use.</param>
        /// <param name="p">The Pen to use for the shape outline.</param>
        /// <param name="b">The Brush to use for the shape fill.</param>
        /// <param name="rect">The bounding rectangle for the shape.</param>
        /// <param name="radius">The radius (in pixels) for the corners.</param>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.0 and later</description></item>
        /// </list>
        /// </remarks>
        public static void RoundedRectangle(this Graphics g, Pen p, Brush b, Rectangle rect, int radius)
        {
            IntPtr hdc = g.GetHdc();

            try
            {
                // create a native pen based on the managed pen passed in
                IntPtr hPen = IntPtr.Zero;
                if (p != null)
                {
                    hPen = NativeMethods.GdiCreatePen((int)p.DashStyle, Convert.ToInt32(p.Width), NativeMethods.ToColorRef(p.Color));
                }
                else
                {
                    hPen = NativeMethods.GdiCreatePen(5, 0, 0);
                }

                
                IntPtr hBrush = IntPtr.Zero;
                if (b is SolidBrush)
                {
                    // create a native brush with the specified brush colour
                    hBrush = NativeMethods.GdiCreateSolidBrush(NativeMethods.ToColorRef(((SolidBrush)b).Color));
                }
                else if (b is TextureBrush)
                {
                    NativeMethods.GdiSetBrushOrgEx(hdc, rect.Left, rect.Top, IntPtr.Zero);

                    // create a native brush with the specified brush image
                    hBrush = NativeMethods.GdiCreatePatternBrush(((Bitmap)((TextureBrush)b).Image).GetHbitmap());
                }

                if (hBrush == IntPtr.Zero)
                {
                    hBrush = NativeMethods.GdiGetStockObject(5); 
                }

                // Select the brush and pen into the current drawing context
                NativeMethods.GdiSelectObject(hdc, hBrush);
                NativeMethods.GdiSelectObject(hdc, hPen);
                // draw the rounded rectangle
                bool success = NativeMethods.GdiRoundRect(hdc, rect.Left, rect.Top, rect.Right, rect.Bottom, radius, radius);
                // delete the native drawing objects
                NativeMethods.GdiDeleteObject(hPen);
                NativeMethods.GdiDeleteObject(hBrush);

                System.Diagnostics.Debug.Assert(success, "RoundRect failed: " + Marshal.GetLastWin32Error().ToString());
            }
            finally
            {
                g.ReleaseHdc(hdc);
            }
        }

        /// <summary>
        /// Draws a rounded rectangle with a gradient fill.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> instance to use.</param>
        /// <param name="p">The <see cref="Pen"/> to use for the shape outline.</param>
        /// <param name="b">The <see cref="Brush"/> to use for the shape fill.</param>
        /// <param name="rect">The bounding rectangle for the shape.</param>
        /// <param name="radius">The radius (in pixels) for the corners.</param>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.0 and later</description></item>
        /// </list>
        /// </remarks>
        public static void RoundedRectangle(this Graphics g, Pen p, LinearGradientBrush b, Rectangle rect, int radius)
        {
            IntPtr hdc = g.GetHdc();
            try
            {
                IntPtr hPen = IntPtr.Zero;
                IntPtr hBrush = IntPtr.Zero;

                if (p != null)
                {
                    hPen = NativeMethods.GdiCreatePen((int)p.DashStyle, Convert.ToInt32(p.Width), NativeMethods.ToColorRef(p.Color));
                }
                else
                {
                    hPen = NativeMethods.GdiCreatePen(5, 0, 0);
                }

                NativeMethods.GdiSelectObject(hdc, hPen);

                if (b != null)
                {
                    // set brush offset to top-left of rectangle
                    NativeMethods.GdiSetBrushOrgEx(hdc, rect.X, rect.Y, IntPtr.Zero);
                    hBrush = b.GetHBrush();                  
                }
                else
                {
                    // get native null brush
                    hBrush = NativeMethods.GdiSelectObject(hdc, NativeMethods.GdiGetStockObject(5));
                }

                // get native brush from the gradient brush
                NativeMethods.GdiSelectObject(hdc, hBrush);

                bool success = NativeMethods.GdiRoundRect(hdc, rect.Left, rect.Top, rect.Right, rect.Bottom, radius, radius);
                NativeMethods.GdiDeleteObject(hPen);
                
                System.Diagnostics.Debug.Assert(success, "RoundRect failed: " + Marshal.GetLastWin32Error().ToString());
            }
            finally
            {
                g.ReleaseHdc(hdc);
            }
        }

        /// <summary>
        /// Draws the supplied image with the alpha value specified.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="image"></param>
        /// <param name="destRect"></param>
        /// <param name="srcRect"></param>
        /// <param name="srcUnit"></param>
        /// <param name="alpha">Per-image alpha constant from 0 (Transparent) to 255 (Opaque).</param>
        public static void DrawImageAlpha(this Graphics g, System.Drawing.Image image, System.Drawing.Rectangle destRect, System.Drawing.Rectangle srcRect, System.Drawing.GraphicsUnit srcUnit, byte alpha)
        {
            using(Graphics g2 = Graphics.FromImage(image))
            {
                NativeMethods.BLENDFUNCTION bf = new NativeMethods.BLENDFUNCTION();
                bf.SourceConstantAlpha = alpha;
                bf.AlphaFormat = 0;
                IntPtr hdcTarget = g.GetHdc();
                IntPtr hdcSource = g2.GetHdc();

                bool success = NativeMethods.GdiAlphaBlend(hdcTarget, destRect.X, destRect.Y, destRect.Width, destRect.Height, hdcSource, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, ref bf);
                g2.ReleaseHdc(hdcSource);
                g.ReleaseHdc(hdcTarget);
            }
        }

        private static int dpiX;
        /// <summary>
        /// Gets the horizontal resolution of the screen.
        /// </summary>
        /// <returns>The value, in dots per inch, for the horizontal resolution supported by the device.</returns>
        public static float DpiX
        {
            get
            {
                
                return (float)dpiX;
            }
        }

        private static int dpiY;
        /// <summary>
        /// Gets the vertical resolution of the screen.
        /// </summary>
        /// <returns>The value, in dots per inch, for the vertical resolution supported by the device.</returns>
        public static float DpiY
        {
            get
            {               
                return (float)dpiY;
            }
        }

        /// <summary>
        /// Converts a co-ordinate value based on 96 dpi to the actual device dpi.
        /// </summary>
        /// <param name="value">96dpi based co-ordinate.</param>
        /// <returns></returns>
        public static int Scale(int value)
        {
            return Convert.ToInt32((value / 96.00) * dpiX);
        }
    }
}