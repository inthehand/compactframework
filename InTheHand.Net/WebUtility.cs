// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebUtility.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using InTheHand.Net;

namespace InTheHand.Net
{
    /// <summary>
    /// Provides methods for encoding and decoding URLs when processing Web requests.
    /// </summary>
    /// <remarks>Equivalent to System.Net.WebUtility in .NET Framework 4.0 or HttpUtility in Silverlight.</remarks>
    public static class WebUtility
    {
        #region Html Encode
        /// <summary>
        /// Converts a string to an HTML-encoded string.
        /// </summary>
        /// <param name="value">The string to encode.</param>
        /// <returns>An encoded string.</returns>
        /// <remarks>If characters such as blanks and punctuation are passed in an HTTP stream, they might be misinterpreted at the receiving end.
        /// HTML encoding converts characters that are not allowed in HTML into character-entity equivalents; HTML decoding reverses the encoding.
        /// For example, when embedded in a block of text, the characters &lt; and &gt; are encoded as &amp;lt; and &amp;gt; for HTTP transmission.
        /// <para>If the value parameter is a null reference (Nothing in Visual Basic), then the returned encoded string is a null reference (Nothing in Visual Basic).
        /// If the value parameter is an empty string, then the returned encoded string is an empty string.</para></remarks>
        public static string HtmlEncode(string value)
        {
            if (value == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlEncode(value, sw);
            sw.Close();
            return sb.ToString();
        }

        /// <summary>
        /// Converts a string into an HTML-encoded string, and returns the output as a <see cref="TextWriter"/> stream of output.
        /// </summary>
        /// <param name="value">The string to encode.</param>
        /// <param name="output">A <see cref="TextWriter"/> output stream.</param>
        /// <remarks>If characters such as blanks and punctuation are passed in an HTTP stream, they might be misinterpreted at the receiving end.
        /// HTML encoding converts characters that are not allowed in HTML into character-entity equivalents; HTML decoding reverses the encoding.
        /// For example, when embedded in a block of text, the characters &lt; and &gt; are encoded as &amp;lt; and &amp;gt; for HTTP transmission.</remarks>
        public static void HtmlEncode(string value, TextWriter output)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            for (int iChar = 0; iChar < value.Length; iChar++)
            {
                switch (value[iChar])
                {
                    case '"':
                        output.Write("&quot;");
                        break;
                    case '&':
                        output.Write("&amp;");
                        break;
                    case '\'':
                        output.Write("&apos;");
                        break;
                    case '<':
                        output.Write("&lt;");
                        break;
                    case '>':
                        output.Write("&gt;");
                        break;
                    default:
                        output.Write(value[iChar]);
                        break;
                }
            }
        }

        #endregion

        #region Html Decode
        /// <summary>
        /// Converts a string that has been HTML-encoded for HTTP transmission into a decoded string.
        /// </summary>
        /// <param name="value">The string to decode.</param>
        /// <returns>A decoded string.</returns>
        /// <remarks>If characters such as blanks and punctuation are passed in an HTTP stream, they might be misinterpreted at the receiving end.
        /// HTML encoding converts characters that are not allowed in HTML into character-entity equivalents; HTML decoding reverses the encoding.
        /// For example, when embedded in a block of text, the characters &lt; and &gt; are encoded as &amp;lt; and &amp;gt; for HTTP transmission.
        /// <para>If the value parameter is a null reference (Nothing in Visual Basic), then the returned decoded string is a null reference (Nothing in Visual Basic).
        /// If the value parameter is an empty string, then the returned decoded string is an empty string.</para></remarks>
        public static string HtmlDecode(string value)
        {
            if (value == null)
            {
                return null;
            }
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlDecode(value, sw);
            sw.Close();
            return sb.ToString();
        }

        /// <summary>
        /// Converts a string that has been HTML-encoded into a decoded string, and sends the decoded string to a <see cref="TextWriter"/> output stream.
        /// </summary>
        /// <param name="value">The string to decode.</param>
        /// <param name="output">A <see cref="TextWriter"/> stream of output.</param>
        /// <remarks>If characters such as blanks and punctuation are passed in an HTTP stream, they might be misinterpreted at the receiving end.
        /// HTML encoding converts characters that are not allowed in HTML into character-entity equivalents; HTML decoding reverses the encoding.
        /// For example, when embedded in a block of text, the characters &lt; and &gt; are encoded as &amp;lt; and &amp;gt; for HTTP transmission.</remarks>
        public static void HtmlDecode(string value, TextWriter output)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            for (int iChar = 0; iChar < value.Length; iChar++)
            {
                if (value[iChar] == '&')
                {
                    //read until ;
                    int semiColonIndex = value.IndexOf(';', iChar + 1);
                    string symbol = value.Substring(iChar + 1, semiColonIndex - (iChar + 1));
                    iChar = semiColonIndex;
                    //added handling for explicit unicode values
                    if (symbol.StartsWith("#"))
                    {
                        int charvalue = 0;
                        //entity is a numbered unicode char
                        if (symbol.ToLower().IndexOf("x") > -1)
                        {
                            //number is hex
                            charvalue = int.Parse(symbol.Substring(2), System.Globalization.NumberStyles.HexNumber);
                        }
                        else
                        {
                            //number is decimal
                            charvalue = int.Parse(symbol.Substring(1));                
                        }
                        output.Write((char)charvalue);
                    }
                    else
                    {
                        switch (symbol)
                        {
                                //html 2.0
                            case "quot":
                            case "ldquo":
                            case "rdquo":
                                output.Write('\"');
                                break;
                            case "amp":
                                output.Write('&');
                                break;
                            case "lt":
                                output.Write('<');
                                break;
                            case "gt":
                                output.Write('>');
                                break;

                            case "apos":
                            case "rsquo":
                            case "lsquo":
                                output.Write('\'');
                                break;
                            
                            case "nbsp":
                                output.Write(' ');
                                break;
                            case "ndash":
                                output.Write('-');
                                break;
                            
                        }
                    }
                }
                else
                {
                    output.Write(value[iChar]);
                }
            }
        }
        #endregion
    }
}