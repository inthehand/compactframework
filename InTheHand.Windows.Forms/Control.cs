// In The Hand - .NET Components for Mobility
//
// InTheHand.Windows.Forms.Control
// 
// Copyright (c) 2007-2012 In The Hand Ltd, All rights reserved.

using System;
using System.Drawing;
using InTheHand.Runtime.InteropServices;
using InTheHand.Drawing;
using System.Windows.Forms;

namespace InTheHand.Windows.Forms
{
    /// <summary>
    /// Provides supporting methods for <see cref="Control"/>.
    /// </summary>
    public static class ControlInTheHand
    {
        internal static bool designMode = AppDomain.CurrentDomain.FriendlyName.IndexOf("DefaultDomain") > -1;

        /// <summary>
        /// Gets a value indicating whether a control is being used on a design surface.
        /// </summary>
        /// <param name="c">The Control.</param>
        /// <returns>true if the control is being used in a designer; otherwise, false.</returns>
        public static bool GetDesignMode(this Control c)
        {
            return designMode;
        }

        /// <summary>
        /// Supports rendering to the specified bitmap.
        /// </summary>
        /// <param name="control">The Control.</param>
        /// <param name="bitmap">The bitmap to be drawn to.</param>
        /// <param name="targetBounds">The bounds within which the control is rendered.</param>
        public static void DrawToBitmap(this Control control, Bitmap bitmap, Rectangle targetBounds)
        {
            IntPtr sourceDC = InTheHand.Drawing.NativeMethods.GdiGetWindowDC(control.Handle);
            Graphics target = Graphics.FromImage(bitmap);
            IntPtr targetDC = target.GetHdc();
            try
            {
                Point p = new Point(0, 0);// control.FindForm().PointToScreen(control.Bounds.Location);
                bool success = InTheHand.Drawing.NativeMethods.GdiBitBlt(targetDC, 0, 0, bitmap.Width, bitmap.Height, sourceDC, targetBounds.X, targetBounds.Y, CopyPixelOperation.SourceCopy);
            }
            finally
            {
                InTheHand.Drawing.NativeMethods.GdiReleaseDC(control.Handle, sourceDC);
                target.ReleaseHdc(targetDC);
            }
        }

        /// <summary>
        /// Retrieves the <see cref="Form"/> that the control is on.
        /// </summary>
        /// <param name="c">The <see cref="Control"/>.</param>
        /// <returns>The <see cref="Form"/> that the control is on.</returns>
        /// <remarks>The control's <see cref="Control.Parent"/> property value might not be the same as the <see cref="Form"/> returned by FindForm method.
        /// For example, if a <see cref="RadioButton"/> control is contained within a <see cref="Panel"/> control, and the <see cref="Panel"/> is on a Form, the RadioButton control's Parent is the <see cref="Panel"/> and the <see cref="Panel"/> control's Parent is the <see cref="Form"/>.</remarks>
        public static Form FindForm(this Control c)
        {
            Control parent = c.Parent;

            if (parent == null)
            {
                return null;
            }

            while (!(parent is Form))
            {
                parent = parent.Parent;

                if (parent == null)
                {
                    return null;
                }
            }

            return (Form)parent;
        }

        internal static int FindString(Control c, int message, string s, int startIndex)
        {
            return NativeMethods.SendMessage(c.Handle, message, startIndex, s + "\0");
        }

        /// <summary>
        /// Gets a value indicating which of the modifier keys (SHIFT, CTRL, and ALT) is in a pressed state.
        /// </summary>
        /// <value>A bitwise combination of the <see cref="Keys"/> values.
        /// The default is <see cref="System.Windows.Forms.Keys.None"/>.</value>
        /// <example>The following code example hides a button when the CTRL key is pressed while the button is clicked.
        /// This example requires that you have a Button named button1 on a Form.
        /// <code lang="vbnet">
        /// Private Sub button1_Click(sender As Object, e As EventArgs) Handles button1.Click
        ///    ' If the CTRL key is pressed when the 
        ///    ' control is clicked, hide the control. 
        ///    If ControlHelper.ModifierKeys = Keys.Control Then
        ///       CType(sender, Control).Hide()
        ///    End If
        /// End Sub
        /// </code>
        /// <code lang="cs">
        /// private void button1_Click(object sender, System.EventArgs e)
        /// {
        ///   /* If the CTRL key is pressed when the 
        ///      * control is clicked, hide the control. */
        ///   if(ControlHelper.ModifierKeys == Keys.Control)
        ///   {
        ///      ((Control)sender).Hide();
        ///   }
        /// }
        /// </code></example>
        public static Keys ModifierKeys
        {
            get
            {
                Keys keys = Keys.None;
                if (NativeMethods.GetAsyncKeyState(Keys.ShiftKey) < 0)
                {
                    keys |= Keys.Shift;
                }
                if (NativeMethods.GetAsyncKeyState(Keys.ControlKey) < 0)
                {
                    keys |= Keys.Control;
                }
                if (NativeMethods.GetAsyncKeyState(Keys.Menu) < 0)
                {
                    keys |= Keys.Alt;
                }
                return keys;
            }
        }


        /// <summary>
        /// Invalidates the entire surface of the control and causes a paint message to be sent to the control.
        /// Optionally, invalidates the child controls assigned to the control.
        /// </summary>
        /// <param name="c">The control.</param>
        /// <param name="invalidateChildren">true to invalidate the control's child controls; otherwise, false.</param>
        public static void Invalidate(this Control c, bool invalidateChildren)
        {
            if (invalidateChildren)
            {
                NativeMethods.RedrawWindow(c.Handle, IntPtr.Zero, IntPtr.Zero, NativeMethods.RDW.ALLCHILDREN | NativeMethods.RDW.INVALIDATE | NativeMethods.RDW.ERASE);
            }
            else
            {
                c.Invalidate();
            }
        }

        private static readonly bool supportsOpacity = (System.Environment.OSVersion.Version >= new Version(5, 2, 21000) && InTheHand.WindowsCE.Forms.SystemSettingsInTheHand.Platform != Microsoft.WindowsCE.Forms.WinCEPlatform.WinCEGeneric);      
        /// <summary>
        /// Gets or sets the opacity level of the control.
        /// </summary>
        /// <param name="c">The Control.</param>
        /// <param name="value">The level of opacity for the form. The default is 1.00.</param>
        /// <remarks>The Opacity property enables you to specify a level of transparency for the form and its controls.
        /// When this property is set to a value less than 100 percent (1.00), the entire form, including borders, is made more transparent.
        /// Setting this property to a value of 0 percent (0.00) makes the form completely invisible.
        /// You can use this property to provide different levels of transparency or to provide effects such as phasing a form in or out of view.
        /// For example, you can phase a form into view by setting the Opacity property to a value of 0 percent (0.00) and gradually increasing the value until it reaches 100 percent (1.00).
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile Version 6.5 and later</description></item>
        /// </list>
        /// </remarks>
        public static void SetOpacity(this Control c, float value)
        {
            if (!supportsOpacity)
            {
                return;
            }

            if (value > 1.00 || value < 0.0)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            ModifyExtendedStyles(c.Handle, NativeMethods.WS_EX_LAYERED, 0);
            bool success = NativeMethods.SetLayeredWindowAttributes(c.Handle, 0, Convert.ToByte(255 * value), NativeMethods.LWA.ALPHA);
        }

        /// <summary>
        /// Sets the color that will represent transparent areas of the form.
        /// </summary>
        /// <param name="f">The form.</param>
        /// <param name="value">A <see cref="Color"/> that represents the color to display transparently on the form.</param>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile Version 6.5 and later</description></item>
        /// </list>
        /// </remarks>
        public static void SetTransparencyKey(this Form f, Color value)
        {
            if (!supportsOpacity)
            {
                return;
            }

            ModifyExtendedStyles(f.Handle, NativeMethods.WS_EX_LAYERED, 0);
            uint colorref = InTheHand.Drawing.NativeMethods.ToColorRef(value);
            bool success = NativeMethods.SetLayeredWindowAttributes(f.Handle, colorref, 0, NativeMethods.LWA.COLORKEY);
        }

        internal const int WS_BORDER = 0x00800000;
        internal const int WS_EX_RTLREADING = 0x2000;
        internal const int WS_EX_RIGHT = 0x1000;
        internal const int WS_EX_LEFTSCROLLBAR = 0x4000;
        internal const int ES_RIGHT = 0x0002;
        private const int BS_MULTILINE = 0x00002000;


        /// <summary>
        /// Gets a value which indicates whether the button has multi-line text enabled.
        /// </summary>
        /// <param name="bb">Button, RadioButton or Checkbox control</param>
        /// <param name="value">true to enable multi-line text, else false.</param>
        public static void SetMultiline(this ButtonBase bb, bool value)
        {
            ModifyStyles(bb.Handle, value ? BS_MULTILINE : 0, value ? 0 : BS_MULTILINE);
        }

        /// <summary>
        /// Gets a value which indicates whether the button has multi-line text enabled.
        /// </summary>
        /// <param name="bb">Button, RadioButton or Checkbox control</param>
        /// <returns>true if multi-line text is enabled, else false.</returns>
        public static bool GetMultiline(this ButtonBase bb)
        {
            return ((int)NativeMethods.GetWindowLong(bb.Handle, NativeMethods.GWL.STYLE) & BS_MULTILINE) == BS_MULTILINE;
        }

        /// <summary>
        /// Sets whether the control draws a 1 pixel border.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="value"></param>
        public static void SetBorder(this Control c, bool value)
        {
            ModifyStyles(c.Handle, value ? WS_BORDER : 0, value ? 0 : WS_BORDER);
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether control's elements are aligned to support locales using right-to-left fonts.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="value"></param>
        public static void SetRightToLeft(this Control c, bool value)
        {
            IntPtr handle = c.Handle;
            ModifyExtendedStyles(c.Handle, value ? WS_EX_LAYOUTRTL | WS_EX_RTLREADING : 0, value ? 0 : WS_EX_LAYOUTRTL | WS_EX_RTLREADING); 
        }

        private const int WS_EX_LAYOUTRTL = 0x400000;

        internal static void ModifyStyles(IntPtr handle, int add, int remove)
        {
            int oldStyle = NativeMethods.GetWindowLong(handle, NativeMethods.GWL.STYLE).ToInt32();
            NativeMethods.SetWindowLong(handle, NativeMethods.GWL.STYLE, (add | oldStyle) & ~remove);            
        }

        internal static void ModifyExtendedStyles(IntPtr handle, int add, int remove)
        {
            int oldStyle = NativeMethods.GetWindowLong(handle, NativeMethods.GWL.EXSTYLE).ToInt32();
            NativeMethods.SetWindowLong(handle, NativeMethods.GWL.EXSTYLE, (add | oldStyle) & ~remove);
        }
    }
}
