// In The Hand - .NET Components for Mobility
//
// InTheHand.Windows.Forms.ControlPaint
// 
// Copyright (c) 2009-2012 In The Hand Ltd, All rights reserved.

using System;
using System.Collections.Generic;

using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using InTheHand.Drawing;
using Microsoft.WindowsCE.Forms;
using InTheHand.WindowsCE.Forms;

namespace InTheHand.Windows.Forms
{
    /// <summary>
    /// Provides methods used to paint common Windows controls and their elements.
    /// </summary>
    /// <remarks><para>Equivalent to System.Windows.Forms.ControlPaint</para>
    /// The methods contained in the <see cref="ControlPaint"/> class enable you to draw your own controls or elements of controls.
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Pocket PC 2003 and later, Windows Mobile Version 5.0 and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows CE 4.1 and later</description></item>
    /// </list>
    /// </remarks>
    public static class ControlPaint
    {
        #region Dark
        /// <summary>
        /// Creates a new dark color object for the control from the specified color.
        /// </summary>
        /// <param name="baseColor">The <see cref="Color"/> to be darkened.</param>
        /// <returns>A Color that represents the dark color on the control.</returns>
        public static Color Dark(Color baseColor)
        {
            return Dark(baseColor, 0.5f);
        }

        /// <summary>
        /// Creates a new dark color object for the control from the specified color.
        /// </summary>
        /// <param name="baseColor">The <see cref="Color"/> to be darkened.</param>
        /// <returns>A <see cref="Color"/> that represents the dark color on the control.</returns>
        public static Color DarkDark(Color baseColor)
        {
            return Dark(baseColor, 1f);
        }

        /// <summary>
        /// Creates a new dark color object for the control from the specified color and darkens it by the specified percentage.
        /// </summary>
        /// <param name="c">The <see cref="Color"/> to be darkened.</param>
        /// <param name="percDarker">The percentage to darken the specified <see cref="Color"/>.</param>
        /// <returns>A <see cref="Color"/> that represent the dark color on the control.</returns>
        public static Color Dark(Color c, float percDarker)
        {
            if (c.ToArgb() == SystemColors.Control.ToArgb())
            {
                if (percDarker == 0f)
                {
                    return SystemColors.ControlDark;
                }
                if (percDarker == 1f)
                {
                    return SystemColors.ControlDarkDark;
                }
                Color controlDark = SystemColors.ControlDark;
                Color controlDarkDark = SystemColors.ControlDarkDark;
                int num = controlDark.R - controlDarkDark.R;
                int num2 = controlDark.G - controlDarkDark.G;
                int num3 = controlDark.B - controlDarkDark.B;
                return Color.FromArgb((byte)(controlDark.R - ((byte)(num * percDarker))), (byte)(controlDark.G - ((byte)(num2 * percDarker))), (byte)(controlDark.B - ((byte)(num3 * percDarker))));
            }
            float hue = c.GetHue();
            float brightness = c.GetBrightness();
            float saturation = c.GetSaturation();

            float newBrightness = (float)(brightness - (brightness * percDarker * 1.5));
            if (newBrightness < 0.0)
            {
                newBrightness = 0.0F;
            }
            return InTheHand.Drawing.ColorInTheHand.FromHSB(hue, saturation, newBrightness);

        }
        #endregion

        #region Light
        /// <summary>
        /// Creates a new light color object for the control from the specified color.
        /// </summary>
        /// <param name="baseColor">The <see cref="Color"/> to be lightened.</param>
        /// <returns>A <see cref="Color"/> that represents the light color on the control.</returns>
        public static Color Light(Color baseColor)
        {
            return Light(baseColor, 0.5f);
        }

        /// <summary>
        /// Creates a new light color object for the control from the specified color.
        /// </summary>
        /// <param name="baseColor">The <see cref="Color"/> to be lightened.</param>
        /// <returns>A <see cref="Color"/> that represents the light color on the control.</returns>
        public static Color LightLight(Color baseColor)
        {
            return Light(baseColor, 1f);
        }

        /// <summary>
        /// Creates a new light color object for the control from the specified color and lightens it by the specified percentage.
        /// </summary>
        /// <param name="baseColor">The <see cref="Color"/> to be lightened.</param>
        /// <param name="percLighter">The percentage to lighten the specified <see cref="Color"/>.</param>
        /// <returns>Creates a new light color object for the control from the specified color and lightens it by the specified percentage.</returns>
        public static Color Light(Color baseColor, float percLighter)
        {
            if (baseColor.ToArgb() == SystemColors.Control.ToArgb())
            {
                if (percLighter == 0f)
                {
                    return SystemColors.ControlLight;
                }
                if (percLighter == 1f)
                {
                    return SystemColors.ControlLightLight;
                }
                Color controlLight = SystemColors.ControlLight;
                Color controlLightLight = SystemColors.ControlLightLight;
                int num = controlLight.R - controlLightLight.R;
                int num2 = controlLight.G - controlLightLight.G;
                int num3 = controlLight.B - controlLightLight.B;
                return Color.FromArgb((byte)(controlLight.R - ((byte)(num * percLighter))), (byte)(controlLight.G - ((byte)(num2 * percLighter))), (byte)(controlLight.B - ((byte)(num3 * percLighter))));
            }
            float hue = baseColor.GetHue();
            float saturation = baseColor.GetSaturation();
            float brightness = baseColor.GetBrightness();
            float newBrightness = (float)((brightness * percLighter * 1.5) + brightness);
            if (newBrightness > 1.0)
            {
                newBrightness = 1.0F;
            }
            return ColorInTheHand.FromHSB(hue, saturation, newBrightness);
        }
        #endregion

        #region Draw Focus Rectangle
        /// <summary>
        /// Draws a focus rectangle on the specified graphics surface and within the specified bounds.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> to draw on.</param>
        /// <param name="rectangle">The <see cref="Rectangle"/> that represents the dimensions of the grab handle glyph.</param>
        /// <remarks>A focus rectangle is a dotted rectangle that Windows uses to indicate what control has the current keyboard focus.
        /// On Windows Mobile this is drawn in the color specified by the current device theme.</remarks>
        public static void DrawFocusRectangle(Graphics graphics, Rectangle rectangle)
        {
            bool success;
            IntPtr hdc = graphics.GetHdc();

            RECT r = RECT.FromXYWH(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            if (InTheHand.NativeMethods.IsMobile5)
            {
                success = NativeMethods.DrawFocusRectColor(hdc, ref r, 0);
            }
            else
            {
                success = NativeMethods.DrawFocusRect(hdc, ref r);
            }

            graphics.ReleaseHdc(hdc);
        }
        #endregion

        #region Draw Caption Button
        /// <summary>
        /// Draws a caption button control.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> to draw on.</param>
        /// <param name="rectangle">The <see cref="Rectangle"/> that represents the dimensions of the caption button.</param>
        /// <param name="button">One of the <see cref="CaptionButton"/> values that specifies the type of caption button to draw.</param>
        /// <param name="state">A bitwise combination of the <see cref="ButtonState"/> values that specifies the state to draw the button in.</param>
        public static void DrawCaptionButton(Graphics graphics, Rectangle rectangle, CaptionButton button, ButtonState state)
        {
            IntPtr hdc = graphics.GetHdc();

            RECT r = RECT.FromXYWH(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            NativeMethods.DrawFrameControl(hdc, ref r, NativeMethods.DFC.CAPTION, (NativeMethods.DFCS)button | (NativeMethods.DFCS)state);

            graphics.ReleaseHdc(hdc);
        }
        #endregion


        #region Draw Check Box
        /// <summary>
        /// Draws a check box control.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> to draw on.</param>
        /// <param name="rectangle">The <see cref="Rectangle"/> that represents the dimensions of the check box.</param>
        /// <param name="state">A bitwise combination of the <see cref="ButtonState"/> values that specifies the state to draw the check box in.</param>
        public static void DrawCheckBox(Graphics graphics, Rectangle rectangle, ButtonState state)
        {
            IntPtr hdc = graphics.GetHdc();

            RECT r = RECT.FromXYWH(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            NativeMethods.DrawFrameControl(hdc, ref r, NativeMethods.DFC.BUTTON, NativeMethods.DFCS.BUTTONCHECK | (NativeMethods.DFCS)state);

            graphics.ReleaseHdc(hdc);
        }
        #endregion

        #region Draw Button
        /// <summary>
        /// Draws a button control in the specified state, on the specified graphics surface, and within the specified bounds.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> to draw on.</param>
        /// <param name="rectangle">The Rectangle that represents the dimensions of the button.</param>
        /// <param name="state">A bitwise combination of the <see cref="ButtonState"/> values that specifies the state to draw the button in.</param>
        public static void DrawButton(Graphics graphics, Rectangle rectangle, ButtonState state)
        {
            IntPtr hdc = graphics.GetHdc();

            RECT r = RECT.FromXYWH(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            NativeMethods.DrawFrameControl(hdc, ref r, NativeMethods.DFC.BUTTON, NativeMethods.DFCS.BUTTONPUSH | (NativeMethods.DFCS)state);

            graphics.ReleaseHdc(hdc);
        }
        #endregion

        #region Draw Radio Button
        /// <summary>
        /// Draws a radio button control.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> to draw on.</param>
        /// <param name="rectangle">The <see cref="Rectangle"/> that represents the dimensions of the radio button.</param>
        /// <param name="state">A bitwise combination of the <see cref="ButtonState"/> values that specifies the state to draw the radio button in.</param>
        public static void DrawRadioButton(Graphics graphics, Rectangle rectangle, ButtonState state)
        {
            IntPtr hdc = graphics.GetHdc();

            RECT r = RECT.FromXYWH(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            NativeMethods.DrawFrameControl(hdc, ref r, NativeMethods.DFC.BUTTON, NativeMethods.DFCS.BUTTONRADIO | (NativeMethods.DFCS)state);

            graphics.ReleaseHdc(hdc);
        }
        #endregion


        #region Draw Scroll Button
        /// <summary>
        /// Draws a scroll button on a scroll bar control.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> to draw on.</param>
        /// <param name="rectangle">The <see cref="Rectangle"/> that represents the dimensions of the glyph.</param>
        /// <param name="button">One of the <see cref="ScrollButton"/> values that specifies the type of scroll arrow to draw.</param>
        /// <param name="state">A bitwise combination of the <see cref="ButtonState"/> values that specifies the state to draw the scroll button in.</param>
        public static void DrawScrollButton(Graphics graphics, Rectangle rectangle, ScrollButton button, ButtonState state)
        {
            IntPtr hdc = graphics.GetHdc();

            RECT r = RECT.FromXYWH(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            NativeMethods.DrawFrameControl(hdc, ref r, NativeMethods.DFC.SCROLL, (NativeMethods.DFCS)button | (NativeMethods.DFCS)state);

            graphics.ReleaseHdc(hdc);
        }
        #endregion

        private static class NativeMethods
        {


            internal enum DFC
            {
                CAPTION = 1,
                SCROLL = 3,
                BUTTON = 4,
            }

            [Flags()]
            internal enum DFCS
            {
                /*CAPTIONCLOSE = 0x0000,
                CAPTIONMIN = 0x0001,
                CAPTIONMAX = 0x0002,
                CAPTIONRESTORE = 0x0003,
                CAPTIONHELP = 0x0004,
                CAPTIONALL = 0x000F,

                CAPTIONOKBTN = 0x0080,*/

                SCROLLUP = 0x0000,
                SCROLLDOWN = 0x0001,
                SCROLLLEFT = 0x0002,
                SCROLLRIGHT = 0x0003,
                SCROLLCOMBOBOX = 0x0005,

                BUTTONCHECK = 0x0000,
                BUTTONRADIO = 0x0004,
                BUTTON3STATE = 0x0008,
                BUTTONPUSH = 0x0010,

                /*INACTIVE = 0x0100,
                PUSHED = 0x0200,
                CHECKED = 0x0400,*/
            }

            [DllImport("coredll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool DrawFocusRect(IntPtr hDC, ref RECT lprc);

            internal enum DFRC
            {
                FOCUSCOLOR = 0,  // Draw using focus color for current theme
                SELECTEDBRUSH = 1,  // Draw using selected brush (can be used for erasing)
            }

            [DllImport("aygshell")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool DrawFocusRectColor(IntPtr hdc, ref RECT lprc, DFRC uFlags);

            [DllImport("coredll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool DrawFrameControl(IntPtr hdc, ref RECT lprc, DFC uType, DFCS uState);
        }
    }

    /// <summary>
    /// Specifies the type of caption button to display.
    /// </summary>
    /// <remarks>Equivalent to System.Windows.Forms.CaptionButton</remarks>
    [FlagsAttribute]
    public enum CaptionButton
    {
        /// <summary>
        /// A Close button.
        /// </summary>
        Close = 0x0000,
        /// <summary>
        /// A Minimize button.
        /// </summary>
        Minimize = 0x0001,
        /// <summary>
        /// A Maximize button.
        /// </summary>
        Maximize = 0x0002,
        /// <summary>
        /// A Restore button.
        /// </summary>
        Restore = 0x0003,
        /// <summary>
        /// A Help button.
        /// </summary>
        Help = 0x0004,
        /// <summary>
        /// An Ok button.
        /// </summary>
        Ok = 0x0080,
    }

    /// <summary>
    /// Specifies the appearance of a button.
    /// </summary>
    [FlagsAttribute]
    public enum ButtonState
    {
        /// <summary>
        /// The button has its normal appearance (three-dimensional).
        /// </summary>
        Normal = 0,

        /// <summary>
        /// The button is inactive (grayed).
        /// </summary>
        Inactive = 0x100,

        /// <summary>
        /// The button appears pressed.
        /// </summary>
        Pushed = 0x200,

        /// <summary>
        /// The button has a checked or latched appearance.
        /// Use this appearance to show that a toggle button has been pressed. 
        /// </summary>
        Checked = 0x400,
        //Flat //The button has a flat, two-dimensional appearance. 
        //All //All flags except Normal are set. 
    }

    /// <summary>
    /// Specifies the type of scroll arrow to draw on a scroll bar.
    /// </summary>
    [FlagsAttribute]
    public enum ScrollButton
    {
        /// <summary>
        /// An up-scroll arrow.
        /// </summary>
        Up = 0,

        /// <summary>
        /// A down-scroll arrow.
        /// </summary>
        Down = 0x1,

        /// <summary>
        /// A left-scroll arrow.
        /// </summary>
        Left = 0x2,

        /// <summary>
        /// A right-scroll arrow.
        /// </summary>
        Right = 0x3,
        
 //Min //A minimum-scroll arrow. 
 //Max //A maximum-scroll arrow. 
    }
}