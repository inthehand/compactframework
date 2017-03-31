// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebBrowserTask.cs" company="In The Hand Ltd">
// Copyright (c) 2010-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Phone.Tasks
{
    /// <summary>
    /// Allows an application to launch the web browser application.
    /// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Pocket PC 2003 and later, Windows Mobile Version 5.0 and later</description></item>
    /// <item><term>Windows Embedded</term><description>Windows CE 4.1 and later (Internet Explorer component required)</description></item>
    /// </list>
    /// </remarks>  
    public sealed class WebBrowserTask
    {
        /// <summary>
        /// Shows the web browser application.
        /// </summary>
        public void Show()
        {
            System.Diagnostics.Process.Start("\\windows\\iexplore.exe", _url);
        }

        private string _url;

        /// <summary>
        /// Gets or sets the URI to which the web browser application will navigate when it is launched.  
        /// </summary>
        public Uri Uri
        {
            get
            {
                if (string.IsNullOrEmpty(_url))
                {
                    return null;
                }

                return new Uri(_url);
            }

            set
            {
                _url = value.ToString();
            }
        }
    }
}