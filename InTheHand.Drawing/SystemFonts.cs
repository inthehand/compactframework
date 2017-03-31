// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemFonts.cs" company="In The Hand Ltd">
// Copyright (c) 2009-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.WindowsCE.Forms;

namespace InTheHand.Drawing
{
    /// <summary>
    /// Specifies the fonts used to display text in Windows display elements.
    /// </summary>
    /// <remarks>Equivalent to System.Drawing.SystemFonts</remarks>
    public static class SystemFonts
    {
        private static int FontSizePixels
        {
            get
            {
                int fontSizePixel;
                int req;
                int hresult = NativeMethods.GetUIMetrics(NativeMethods.SHUIMETRIC.FONTSIZE_PIXEL, out fontSizePixel, 4, out req);
                if (hresult != 0)
                {
                    return 28;
                }

                return fontSizePixel;
            }
        }

        /// <summary>
        /// Gets the default font that applications can use for dialog boxes and forms.
        /// </summary>
        /// <value>The default <see cref="Font"/> of the system.
        /// The value returned will vary depending on UI metrics and culture of the operating system.</value>
        public static Font DefaultFont
        {
            get
            {
                Microsoft.WindowsCE.Forms.LogFont lf = new Microsoft.WindowsCE.Forms.LogFont();
                lf.PitchAndFamily = Microsoft.WindowsCE.Forms.LogFontPitchAndFamily.Default;
                lf.Height = -FontSizePixels;
                lf.Quality = Microsoft.WindowsCE.Forms.LogFontQuality.ClearType;

                return Font.FromLogFont(lf);
            }
        }
        
        /// <summary>
        /// Gets a <see cref="Font"/> that is used to display text in the title bars of windows.
        /// </summary>
        /// <value>A <see cref="Font"/> that is used to display text in the title bars of windows.</value>
        public static Font CaptionFont
        {
            get
            {
                float fontSize = 10;
                if (InTheHand.WindowsCE.Forms.SystemSettingsInTheHand.Platform == WinCEPlatform.PocketPC)
                {
                    fontSize = 8;
                }

                return new Font(FontFamily.GenericSansSerif, fontSize, FontStyle.Bold);
            }
        }
    }
}