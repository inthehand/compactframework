// In The Hand - .NET Components for Mobility
//
// InTheHand.Windows.Forms.Help
// 
// Copyright (c) 2006-2012 In The Hand Ltd, All rights reserved.

using Microsoft.Win32;
using System.Diagnostics;
using System;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.WindowsCE.Forms;

namespace InTheHand.Windows.Forms
{
    /// <summary>
    /// Provides Help support for all Windows Mobile platforms.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Help"/>
    public static class Help
    {
        /// <summary>
        /// Displays the contents of the Help file at the specified URL.
        /// </summary>
        /// <param name="parent">Not used by the .NET Compact Framework</param>
        /// <param name="url">The path and name of the Help file.</param>
        /// <remarks>Supports Windows Mobile Standard Edition using the default .html application.
        /// You can optionally specify the topic anchor within the file e.g. "MyApp.htm#MyTopic"</remarks>
        public static void ShowHelp(Control parent, string url)
        {

            if (InTheHand.WindowsCE.Forms.SystemSettingsInTheHand.Platform == WinCEPlatform.Smartphone)
            {
                // open with web browser
                Process.Start(url, string.Empty);
            }
            else
            {
                // open with peghelp
                System.Windows.Forms.Help.ShowHelp(parent, url);           
            }
        }
    }
}