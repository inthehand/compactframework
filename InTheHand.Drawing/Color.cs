// In The Hand - .NET Components for Mobility
//
// InTheHand.Drawing.Color
// 
// Copyright (c) 2010-2012 In The Hand Ltd, All rights reserved.

using System;
using System.Drawing;
using InTheHand.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace InTheHand.Drawing
{
    /// <summary>
    /// Provides helper methods for the <see cref="Color"/> class.
    /// </summary>
    /// <seealso cref="Color"/>
    public static class ColorInTheHand
    {
        /// <summary>
        /// Translates the specified Color structure to an HTML string color representation.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string ToHtml(this Color c)
        {
            foreach (System.Reflection.PropertyInfo pi in typeof(Color).GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public))
            {
                if ((Color)pi.GetValue(null, null) == c)
                {
                    return pi.Name;
                }
            }

            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        /// <summary>
        /// Gets the hue-saturation-brightness (HSB) hue value, in degrees, for this Color structure.
        /// </summary>
        /// <param name="color">The hue, in degrees, of this Color. The hue is measured in degrees, ranging from 0.0 through 360.0, in HSB color space.</param>
        /// <returns></returns>
        public static float GetHue(this Color color)
        {
            if ((color.R == color.G) && (color.G == color.B))
            {
                return 0f;
            }
            float num = ((float)color.R) / 255f;
            float num2 = ((float)color.G) / 255f;
            float num3 = ((float)color.B) / 255f;
            float num7 = 0f;
            float num4 = num;
            float num5 = num;
            if (num2 > num4)
            {
                num4 = num2;
            }
            if (num3 > num4)
            {
                num4 = num3;
            }
            if (num2 < num5)
            {
                num5 = num2;
            }
            if (num3 < num5)
            {
                num5 = num3;
            }
            float num6 = num4 - num5;
            if (num == num4)
            {
                num7 = (num2 - num3) / num6;
            }
            else if (num2 == num4)
            {
                num7 = 2f + ((num3 - num) / num6);
            }
            else if (num3 == num4)
            {
                num7 = 4f + ((num - num2) / num6);
            }
            num7 *= 60f;
            if (num7 < 0f)
            {
                num7 += 360f;
            }
            return num7;
        }

        /// <summary>
        /// Gets the hue-saturation-brightness (HSB) saturation value for this Color structure.
        /// </summary>
        /// <param name="color"></param>
        /// <returns>The saturation of this Color. The saturation ranges from 0.0 through 1.0, where 0.0 is grayscale and 1.0 is the most saturated.</returns>
        public static float GetSaturation(this Color color)
        {
            float num = ((float)color.R) / 255f;
            float num2 = ((float)color.G) / 255f;
            float num3 = ((float)color.B) / 255f;
            float num7 = 0f;
            float num4 = num;
            float num5 = num;
            if (num2 > num4)
            {
                num4 = num2;
            }

            if (num3 > num4)
            {
                num4 = num3;
            }

            if (num2 < num5)
            {
                num5 = num2;
            }

            if (num3 < num5)
            {
                num5 = num3;
            }

            if (num4 == num5)
            {
                return num7;
            }

            float num6 = (num4 + num5) / 2f;
            if (num6 <= 0.5)
            {
                return ((num4 - num5) / (num4 + num5));
            }

            return ((num4 - num5) / ((2f - num4) - num5));
        }

        /// <summary>
        /// Gets the hue-saturation-brightness (HSB) brightness value for this Color structure.
        /// </summary>
        /// <param name="color"></param>
        /// <returns>The brightness of this Color. The brightness ranges from 0.0 through 1.0, where 0.0 represents black and 1.0 represents white.</returns>
        public static float GetBrightness(this Color color)
        {
            float num = ((float)color.R) / 255f;
            float num2 = ((float)color.G) / 255f;
            float num3 = ((float)color.B) / 255f;
            float num4 = num;
            float num5 = num;
            if (num2 > num4)
            {
                num4 = num2;
            }

            if (num3 > num4)
            {
                num4 = num3;
            }

            if (num2 < num5)
            {
                num5 = num2;
            }

            if (num3 < num5)
            {
                num5 = num3;
            }

            return ((num4 + num5) / 2f);
        }

        // Based on Chris Jackson's Code
        // http://blogs.msdn.com/cjacks/archive/2006/04/12/575476.aspx
        // Modified for .NETCF (no Alpha support in intrinsic Color struct)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Color FromHSB(float h, float s, float b)
        {
            if (0f > h || 360f < h)
            {
                throw new ArgumentOutOfRangeException("h");
            }

            if (0f > s || 1f < s)
            {
                throw new ArgumentOutOfRangeException("s");
            }

            if (0f > b || 1f < b)
            {
                throw new ArgumentOutOfRangeException("b");
            }

            if (0 == s)
            {
                return Color.FromArgb(Convert.ToInt32(b * 255),
                  Convert.ToInt32(b * 255), Convert.ToInt32(b * 255));
            }

            float fMax, fMid, fMin;
            int iSextant, iMax, iMid, iMin;

            if (0.5 < b)
            {
                fMax = b - (b * s) + s;
                fMin = b + (b * s) - s;
            }
            else
            {
                fMax = b + (b * s);
                fMin = b - (b * s);
            }

            iSextant = (int)Math.Floor(h / 60f);
            if (300f <= h)
            {
                h -= 360f;
            }
            h /= 60f;
            h -= 2f * (float)Math.Floor(((iSextant + 1f) % 6f) / 2f);
            if (0 == iSextant % 2)
            {
                fMid = h * (fMax - fMin) + fMin;
            }
            else
            {
                fMid = fMin - h * (fMax - fMin);
            }

            iMax = Convert.ToInt32(fMax * 255);
            iMid = Convert.ToInt32(fMid * 255);
            iMin = Convert.ToInt32(fMin * 255);

            switch (iSextant)
            {
                case 1:
                    return Color.FromArgb(iMid, iMax, iMin);

                case 2:
                    return Color.FromArgb(iMin, iMax, iMid);

                case 3:
                    return Color.FromArgb(iMin, iMid, iMax);

                case 4:
                    return Color.FromArgb(iMid, iMin, iMax);

                case 5:
                    return Color.FromArgb(iMax, iMin, iMid);

                default:
                    return Color.FromArgb(iMax, iMid, iMin);
            }
        }
    }
}