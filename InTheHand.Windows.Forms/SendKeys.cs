// In The Hand - .NET Components for Mobility
//
// InTheHand.Windows.Forms.SendKeys
// 
// Copyright (c) 2003-2012 In The Hand Ltd, All rights reserved.

using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace InTheHand.Windows.Forms
{
    /// <summary>
    /// Provides methods for sending keystrokes to an application.
    /// </summary>
    /// <remarks>Use SendKeys to send keystrokes and keystroke combinations to the active application.
    /// To send a keystroke to a class and immediately continue with the flow of your program, use <see cref="Send(string)"/>.
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile 2003 and later</description></item>
    /// <item><term>Windows Embedded</term><description>Windows CE 4.1 and later</description></item>
    /// </list>
    /// </remarks>
    public static class SendKeys
    {

        private static System.Collections.Generic.Dictionary<string,Keys> specialKeys = new System.Collections.Generic.Dictionary<string,Keys>();

        static SendKeys()
        {
            specialKeys.Add("BS", Keys.Back);
            specialKeys.Add("BKSP", Keys.Back);
            specialKeys.Add("BACKSPACE", Keys.Back);
            specialKeys.Add("CAPSLOCK", Keys.CapsLock);
			specialKeys.Add("DEL", Keys.Delete);
            specialKeys.Add("DELETE", Keys.Delete);
            specialKeys.Add("DOWN", Keys.Down);
            specialKeys.Add("END", Keys.End);
            specialKeys.Add("ENTER", Keys.Enter);
            specialKeys.Add("ESC", Keys.Escape);
            specialKeys.Add("HOME", Keys.Home);
            specialKeys.Add("INS", Keys.Insert);
            specialKeys.Add("INSERT", Keys.Insert);
            specialKeys.Add("LEFT", Keys.Left);
            specialKeys.Add("PGDN", Keys.PageDown);
            specialKeys.Add("PGUP", Keys.PageUp);
            specialKeys.Add("PRTSC", Keys.PrintScreen);
            specialKeys.Add("RIGHT", Keys.Right);
            specialKeys.Add("SCROLLLOCK", Keys.Scroll);
            specialKeys.Add("TAB", Keys.Tab);
            specialKeys.Add("+", Keys.Add);
            specialKeys.Add("^", Keys.D6 | Keys.Shift);
            specialKeys.Add("%", Keys.D5 | Keys.Shift);
            specialKeys.Add("{", (Keys)219 | Keys.Shift);
            specialKeys.Add("}", (Keys)221 | Keys.Shift);
            specialKeys.Add("[", (Keys)219);
            specialKeys.Add("]", (Keys)221);
            specialKeys.Add("~", (Keys)192 | Keys.Shift);
        }

        /// <summary>
        /// Sends keystrokes to the active application.
        /// </summary>
        /// <param name="keys">The string of keystrokes to send.</param>
        /// <exception cref="ArgumentException">keys does not represent valid keystrokes.</exception>
        public static void Send(string keys)
        {
            Keys startModifiers = InTheHand.Windows.Forms.ControlInTheHand.ModifierKeys;

            for (int iChar = 0; iChar < keys.Length; iChar++)
            {
                switch (keys[iChar])
                {
                    case '{':
                        iChar++;
                        int endPos = keys.IndexOf('}', iChar+1);
                        if (endPos < 0)
                        {
                            throw new ArgumentException("keys");
                        }
                        string substring = keys.Substring(iChar, endPos - iChar);
                        if (specialKeys.ContainsKey(substring))
                        {
                            Send((Keys)specialKeys[substring]);
                        }
                        else
                        {
                            object k = Enum.Parse(typeof(System.Windows.Forms.Keys), substring, true);

                            if (k != null)
                            {
                                Send((Keys)k);
                            }
                        }
                        iChar = endPos;
                        break;
                    default:
                        Keys vk = ToVirtualKey(keys[iChar]);
                        Send((Keys)vk);
                        
                        break;
                }
            }

            Keys endModifiers = InTheHand.Windows.Forms.ControlInTheHand.ModifierKeys;

            if (startModifiers != endModifiers)
            {
                if (((startModifiers & Keys.Shift) == Keys.Shift) && ((endModifiers & Keys.Shift) != Keys.Shift))
                {
                    keyb_event((byte)Keys.ShiftKey, 0, 0, 0);
                }
                if (((startModifiers & Keys.Control) == Keys.Control) && ((endModifiers & Keys.Control) != Keys.Control))
                {
                    keyb_event((byte)Keys.ControlKey, 0, 0, 0);
                }
            }
        }

        internal static void Send(Keys key)
        {
            bool shift = false;
            if (((int)key & 0xf0000) == (int)Keys.Shift)
            {
                shift = true;
            }
            if (shift)
            {
                NativeMethods.keyb_event((byte)Keys.ShiftKey, 0, 0, 0);
            }
            keyb_event((byte)key, 0, 0, 0);
            keyb_event((byte)key, 0, NativeMethods.KEYEVENTF_KEYUP, 0);
            if (shift)
            {
                keyb_event((byte)Keys.ShiftKey, 0, NativeMethods.KEYEVENTF_KEYUP, 0);
            }
        }

        //use function based on OS version
        private static void keyb_event(byte key, byte scan, int flags, int extra)
        {
            if (System.Environment.OSVersion.Version.Major >= 7)
            {
                NativeMethods.keyb_eventEx(key, scan, flags, 0);
            }
            else
            {
                NativeMethods.keyb_event(key, scan, flags, 0);
            }
        }

        internal static Keys ToVirtualKey(char c)
        {
            bool shift = false;
            byte vKey = Convert.ToByte(c);

            //letters
            if (Char.IsUpper(c))
            {
                shift = true;
            }
            else if (Char.IsLower(c))
            {
                vKey = (byte)Char.ToUpper(c);
            }
            /*else if (Char.IsDigit(c))
            {
                vKey = (byte)c;
            }*/
            else
            {
                switch (c)
                {
                    case '!':
                        vKey = 49;
                        shift = true;
                        break;
                    case '@':
                        vKey = 50;
                        shift = true;
                        break;
                    case '#':
                        vKey = 51;
                        shift = true;
                        break;
                    case '$':
                        vKey = 52;
                        shift = true;
                        break;
                    case '%':
                        vKey = 53;
                        shift = true;
                        break;
                    case '^':
                        vKey = 54;
                        shift = true;
                        break;
                    case '&':
                        vKey = 55;
                        shift = true;
                        break;
                    case '*':
                        vKey = 56;
                        shift = true;
                        break;
                    case '(':
                        vKey = 57;
                        shift = true;
                        break;
                    case ')':
                        vKey = 48;
                        shift = true;
                        break;

                    case ';':
                        vKey = 186;
                        break;
                    case ':':
                        vKey = 186;
                        shift = true;
                        break;
                    case '=':
                        vKey = 187;
                        break;
                    case '+':
                        vKey = 187;
                        shift = true;
                        break;
                    case ',':
                        vKey = 188;
                        break;
                    case '<':
                        vKey = 188;
                        shift = true;
                        break;
                    case '-':
                        vKey = 189;
                        break;
                    case '_':
                        vKey = 189;
                        shift = true;
                        break;
                    case '.':
                        vKey = 190;
                        break;
                    case '>':
                        vKey = 190;
                        shift = true;
                        break;
                    case '/':
                        vKey = 191;
                        break;
                    case '?':
                        vKey = 191;
                        shift = true;
                        break;
                    case '`':
                        vKey = 193;
                        break;
                    case '¬':
                        vKey = 194;
                        shift = true;
                        break;

                    case '[':
                        vKey = 219;
                        break;
                    case '{':
                        vKey = 219;
                        shift = true;
                        break;
                    case '\\':
                        vKey = 220;
                        break;
                    case '|':
                        vKey = 220;
                        shift = true;
                        break;
                    case ']':
                        vKey = 221;
                        break;
                    case '}':
                        vKey = 221;
                        shift = true;
                        break;

                    case '\t':
                        vKey = (byte)Keys.Tab;
                        break;
                    case '~':
                        //enter
                        vKey = (byte)Keys.Enter;
                        break;
                }
            }

            if (shift)
            {
                return (Keys)vKey | Keys.Shift;
            }
            else
            {
                return (Keys)vKey;
            }
        }

        private static class NativeMethods
        {
            internal const int KEYEVENTF_KEYUP = 0x0002;

            [DllImport("coredll", EntryPoint = "keybd_event")]
            internal static extern void keyb_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

            // CE7
            [DllImport("coredll", EntryPoint = "keybd_eventEx")]
            internal static extern void keyb_eventEx(byte bVk, byte bScan, int dwFlags, int guidPDD);
        }
    }
}