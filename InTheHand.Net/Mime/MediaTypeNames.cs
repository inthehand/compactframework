// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaTypeNames.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace InTheHand.Net.Mime
{
	/// <summary>
	/// Specifies the media type information for an object.
	/// </summary>
	public static class MediaTypeNames
	{
        /// <summary>
		/// Specifies the type of text data in an object.
		/// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
        public static class Application
        {
            /// <summary>
            /// Specifies that the application data is not interpreted.
            /// </summary>
            /// <remarks>The Octet member designates that the attachment contains generic binary data.</remarks>
            public const string Octet = "application/octet-stream";

            /// <summary>
            /// Specifies that the application data is in Portable Document Format (PDF).
            /// </summary>
            public const string Pdf = "application/pdf";

            /// <summary>
            /// Specifies that the application data is in Rich Text Format (RTF).
            /// </summary>
            public const string Rtf = "text/rtf";

            /// <summary>
            /// Specifies that the application data is a SOAP document.
            /// </summary>
            public const string Soap = "application/soap";

            /// <summary>
            /// Specifies that the application data is compressed.
            /// </summary>
            public const string Zip = "application/zip";
        }

		/// <summary>
		/// Specifies the type of image data in an object.
		/// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public static class Image
		{
			/// <summary>
            /// Specifies that the image data is in Graphics Interchange Format (GIF).
			/// </summary>
			public const string Gif = "image/gif";

			/// <summary>
            /// Specifies that the image data is in Joint Photographic Experts Group (JPEG) format.
			/// </summary>
			public const string Jpg = "image/jpg";

            /// <summary>
            /// Specifies that the image data is in Tagged Image File Format (TIFF).
            /// </summary>
            public const string Tiff = "image/tiff";
		}

		/// <summary>
		/// Specifies the type of text data in an object.
		/// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
		public static class Text
		{
			/// <summary>
			/// Specifies that the data is in HTML format.
			/// </summary>
			public const string Html = "text/html";

			/// <summary>
			/// Specifies that the data is in plain text format.
			/// </summary>
			public const string Plain = "text/plain";

            /// <summary>
            /// Specifies that the data is in Rich Text Format (RTF).
            /// </summary>
            public const string RichText = "text/rtf";
            
			/// <summary>
			/// Specifies that the data is in XML format.
			/// </summary>
			public const string Xml = "text/xml";
		}
	}
}
