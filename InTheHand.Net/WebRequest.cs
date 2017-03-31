// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebRequest.cs" company="In The Hand Ltd">
// Copyright (c) 2011-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Net;

namespace InTheHand.Net
{
    /// <summary>
    /// Helper methods for <see cref="WebRequest"/>.
    /// </summary>
    public static class WebRequestInTheHand
    {
        /// <summary>
        /// Initializes a new <see cref="HttpWebRequest"/> instance for the specified URI string.
        /// </summary>
        /// <param name="requestUriString">A URI string that identifies the Internet resource.</param>
        /// <returns>An <see cref="HttpWebRequest"/> instance for the specific URI string.</returns>
        public static System.Net.HttpWebRequest CreateHttp(string requestUriString)
        {
            return (HttpWebRequest)WebRequest.Create(requestUriString);
        }

        /// <summary>
        /// Initializes a new <see cref="HttpWebRequest"/> instance for the specified URI.
        /// </summary>
        /// <param name="requestUri">A URI that identifies the Internet resource.</param>
        /// <returns>An <see cref="HttpWebRequest"/> instance for the specific URI.</returns>
        /// <exception cref="NotSupportedException">The request scheme specified in requestUri is the http or https scheme.</exception>
        /// <exception cref="ArgumentNullException">requestUri is null.</exception>
        public static HttpWebRequest CreateHttp(Uri requestUri)
        {
            if (requestUri == null)
            {
                throw new ArgumentNullException("requestUri");
            }

            if (!requestUri.Scheme.Contains("http"))
            {
                throw new NotSupportedException();
            }

            return (HttpWebRequest)WebRequest.Create(requestUri);
        }
    }
}
