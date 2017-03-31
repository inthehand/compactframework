// In The Hand - .NET Components for Mobility
//
// InTheHand.Windows.Forms.TextBox
// 
// Copyright (c) 2008-2012 In The Hand Ltd, All rights reserved.

using InTheHand.Runtime.InteropServices;

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.WindowsCE.Forms;

namespace InTheHand.Windows.Forms
{
    /// <summary>
    /// Provides supporting methods for <see cref="TextBox"/>.
    /// </summary>
    /// <seealso cref="TextBox"/>
    public static class TextBoxInTheHand
    {
        #region Append Text
        /// <summary>
        /// Appends text to the current text of a text box.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox"/> control.</param>
        /// <param name="text">The text to append to the current contents of the text box.</param>
        public static void AppendText(this TextBox textBox, string text)
        {
            if (textBox == null)
            {
                throw new ArgumentNullException("textBox");
            }

            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }

            IntPtr pText = MarshalInTheHand.StringToHGlobalUni(text);
            try
            {
                textBox.SelectionStart = textBox.TextLength;
                Microsoft.WindowsCE.Forms.Message m = Microsoft.WindowsCE.Forms.Message.Create(textBox.Handle, EM_REPLACESEL, new IntPtr(-1), pText);
                Microsoft.WindowsCE.Forms.MessageWindow.SendMessage(ref m);
            }
            finally
            {
                Marshal.FreeHGlobal(pText);
            }
        }
        private const int EM_REPLACESEL = 0x00C2;
        #endregion

        #region Cut
        /// <summary>
        /// Moves the current selection in the text box to the Clipboard.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox"/> control.</param>
        /// <remarks>This method will only cut text from the text box if text is selected in the control.
        /// You can use this method, instead of using the Clipboard class, to copy text in the text box and move it to the Clipboard.</remarks>
        public static void Cut(this TextBox textBox)
        {
            if (textBox == null)
            {
                throw new ArgumentNullException("textBox");
            }

            Microsoft.WindowsCE.Forms.Message m = Microsoft.WindowsCE.Forms.Message.Create(textBox.Handle, WM_CUT, IntPtr.Zero, IntPtr.Zero);
            Microsoft.WindowsCE.Forms.MessageWindow.SendMessage(ref m);         
        }
        private const int WM_CUT = 0x0300;
        #endregion

        #region Copy
        /// <summary>
        /// Copies the current selection of the text editing control to the Clipboard.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox"/> control.</param>
        /// <remarks>A copy operation copies the selected text to the Clipboard.
        /// Note that the selected text is not removed from the text editing control in the process.
        /// A similar method, <see cref="Cut"/>, moves the current selection to the Clipboard and removes the selected text from the text editing control in the process.</remarks>
        public static void Copy(this TextBox textBox)
        {
            if (textBox == null)
            {
                throw new ArgumentNullException("textBox");
            }

            Microsoft.WindowsCE.Forms.Message m = Microsoft.WindowsCE.Forms.Message.Create(textBox.Handle, WM_COPY, IntPtr.Zero, IntPtr.Zero);
            Microsoft.WindowsCE.Forms.MessageWindow.SendMessage(ref m);
        }
        private const int WM_COPY = 0x0301;
        #endregion

        #region Paste
        /// <summary>
        /// Replaces the current selection in the text box with the contents of the Clipboard.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox"/> control.</param>
        /// <remarks>The Paste method will only paste text into the control if text is currently stored in the Clipboard.</remarks>
        public static void Paste(this TextBox textBox)
        {
            if (textBox == null)
            {
                throw new ArgumentNullException("textBox");
            }

            Microsoft.WindowsCE.Forms.Message m = Microsoft.WindowsCE.Forms.Message.Create(textBox.Handle, WM_PASTE, IntPtr.Zero, IntPtr.Zero);
            Microsoft.WindowsCE.Forms.MessageWindow.SendMessage(ref m);
        }
        private const int WM_PASTE = 0x0302;
        #endregion

        #region Clear
        /// <summary>
        /// Clears all text from the text box control.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox"/> control.</param>
        /// <remarks>You can use this method to clear the contents of the control instead of assigning the <see cref="Text"/> property an empty string.</remarks>
        public static void Clear(this TextBox textBox)
        {
            if (textBox == null)
            {
                throw new ArgumentNullException("textBox");
            }

            textBox.Text = string.Empty;
        }       
        #endregion


        #region Clear Undo
        /// <summary>
        /// Clears information about the most recent operation from the undo buffer of the text box.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox"/> control.</param>
        /// <remarks>You can use this method to prevent an undo operation from repeating, based on the state of your application.</remarks>
        /// <returns></returns>
        public static void ClearUndo(this TextBox textBox)
        {
            if (textBox == null)
            {
                throw new ArgumentNullException("textBox");
            }

            Microsoft.WindowsCE.Forms.Message m = Microsoft.WindowsCE.Forms.Message.Create(textBox.Handle, EM_EMPTYUNDOBUFFER, IntPtr.Zero, IntPtr.Zero);
            Microsoft.WindowsCE.Forms.MessageWindow.SendMessage(ref m);
        }

        private const int EM_EMPTYUNDOBUFFER = 0x00CD;
        #endregion

        #region Caption
        /// <summary>
        /// Gets the caption of the associated full-screen edit control
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox"/> control.</param>
        /// <returns></returns>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile Standard Edition only</description></item>
        /// </list>
        /// </remarks>
        public static string GetCaption(this TextBox textBox)
        {
            if (textBox == null)
            {
                throw new ArgumentNullException("textBox");
            }

            if (InTheHand.WindowsCE.Forms.SystemSettingsInTheHand.Platform == WinCEPlatform.Smartphone)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder(260);

                // Obtain the native window handle of the up/down spinner control
                IntPtr hWndEdit = NativeMethods.GetWindow(textBox.Handle, NativeMethods.GW.HWNDNEXT);

                // get the title of the spinner
                int chars = NativeMethods.GetWindowText(hWndEdit, sb, sb.Capacity);
                return sb.ToString(0, chars);
            }

            return textBox.Text;
        }

        /// <summary>
        /// Sets the caption of the associated full-screen edit control
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox"/> control.</param>
        /// <param name="caption">Caption to display in the Titlebar.</param>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile Standard Edition only</description></item>
        /// </list>
        /// </remarks>
        public static void SetCaption(this TextBox textBox, string caption)
        {
            if (InTheHand.WindowsCE.Forms.SystemSettingsInTheHand.Platform == WinCEPlatform.Smartphone)
            {
                // Obtain the native window handle of the up/down spinner control
                IntPtr hWndEdit = NativeMethods.GetWindow(textBox.Handle, NativeMethods.GW.HWNDNEXT);

                // Set the title of the spinner
                NativeMethods.SetWindowText(hWndEdit, caption);
            }

        }
        #endregion

        #region Lines
        /// <summary>
        /// Gets the lines of text in a text box control.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox"/> control.</param>
        /// <returns>An array of strings that contains the text in a text box control.</returns>
        public static string[] GetLines(this TextBox textBox)
        {
            if (textBox == null)
            {
                throw new ArgumentNullException("textBox");
            }

            Microsoft.WindowsCE.Forms.Message m = Microsoft.WindowsCE.Forms.Message.Create(textBox.Handle, EM_GETLINECOUNT, IntPtr.Zero, IntPtr.Zero);
            Microsoft.WindowsCE.Forms.MessageWindow.SendMessage(ref m);
            int count = m.Result.ToInt32();
            string[] lines = new string[count];
            if (count > 0)
            {
                IntPtr buffer = Marshal.AllocHGlobal(512);
                for (int i = 0; i < count; i++)
                {
                    Marshal.WriteInt16(buffer, 0x255);
                    Microsoft.WindowsCE.Forms.Message ml = Microsoft.WindowsCE.Forms.Message.Create(textBox.Handle, EM_GETLINE, (IntPtr)i, buffer);
                    Microsoft.WindowsCE.Forms.MessageWindow.SendMessage(ref ml);
                    int lineLen = ml.Result.ToInt32();
                    lines[i] = Marshal.PtrToStringUni(buffer, lineLen);
                }
                Marshal.FreeHGlobal(buffer);
            }
            return lines;
        }
        private const int EM_GETLINECOUNT = 0x00BA;
        private const int EM_GETLINE = 0x00C4;
        #endregion

        #region Get Line From Char Index
        /// <summary>
        /// Retrieves the line number from the specified character position within the text of the control.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox"/> control.</param>
        /// <param name="index">The character index position to search.</param>
        /// <returns>The zero-based line number in which the character index is located.</returns>
        public static int GetLineFromCharIndex(this TextBox textBox, int index)
        {
            if (textBox == null)
            {
                throw new ArgumentNullException("textBox");
            }

            Microsoft.WindowsCE.Forms.Message m = Microsoft.WindowsCE.Forms.Message.Create(textBox.Handle, EM_LINEFROMCHAR, (IntPtr)index, IntPtr.Zero);
            Microsoft.WindowsCE.Forms.MessageWindow.SendMessage(ref m);
            return m.Result.ToInt32();
        }
        private const int EM_LINEFROMCHAR = 0x00C9;
        #endregion
    }
}