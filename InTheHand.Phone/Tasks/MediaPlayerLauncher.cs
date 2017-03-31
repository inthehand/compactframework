// In The Hand - .NET Components for Mobility
//
// InTheHand.Phone.Tasks.MediaPlayerLauncher
// 
// Copyright (c) 2011 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Phone.Tasks
{
    /// <summary>
    /// Allows an application to launch the media player.
    /// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
    /// </list>
    /// </remarks>  
    public sealed class MediaPlayerLauncher
    {
        private const string mediaPlayerPath = "\\Windows\\WMPlayer.exe";

        /// <summary>
        /// Gets or sets the media played with the media player application.
        /// </summary>
        /// <value>The URI of a media file.</value>
        public Uri Media
        {
            get;
            set;
        }

        /// <summary>
        /// Shows the media player application.
        /// </summary>
        public void Show()
        {
            string path = null;
            if (Media.Scheme == "file")
            {
                //quote to allow for paths with strings
                path = "\"" + Media.LocalPath + "\"";
            }
            else
            {
                path = Media.ToString();
            }

            System.Diagnostics.Process.Start(mediaPlayerPath, path);
        }
    }
}