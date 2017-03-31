// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileVersionInfo.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Diagnostics
{
    using System;
    using System.Runtime.InteropServices;
    using InTheHand.Runtime.InteropServices;
    
    /// <summary>
    /// Provides version information for a physical file in storage memory.
    /// </summary>
    /// <remarks>Equivalent to System.Diagnostics.FileVersionInfo</remarks>
    public sealed class FileVersionInfo
    {

        //the name of the file (sans path)
        private string filename;

        //the language independent version info
        private VS_FIXEDFILEINFO fixedFileInfo;

        private const string valueNameMask = "\\StringFileInfo\\{0:X4}{1:X4}\\{2}";

        private FileVersionInfo(string fileName)
        {
            //get the filename sans path
            this.filename = System.IO.Path.GetFileName(fileName);

            int handle;
            ushort language;
            ushort codepage;
            int len = 0;

            //get size of version info
            len = NativeMethods.GetInfoSize(fileName, out handle);

            if (len > 0)
            {
                //allocate buffer
                IntPtr buffer = MarshalInTheHand.AllocHGlobal(len);

                try
                {
                    //get version information
                    if (NativeMethods.GetInfo(fileName, handle, len, buffer))
                    {
                        IntPtr fixedbuffer = IntPtr.Zero;
                        int fixedlen;
                        //get language independent version info
                        //this is a pointer within the main buffer so don't free it
                        if (NativeMethods.QueryValue(buffer, "\\", out fixedbuffer, out fixedlen))
                        {
                            fixedFileInfo = (VS_FIXEDFILEINFO)Marshal.PtrToStructure(fixedbuffer, typeof(VS_FIXEDFILEINFO));
                        }
                        else
                        {
                            throw InTheHand.ComponentModel.Win32ExceptionInTheHand.Create();
                        }
                        IntPtr langBuffer = IntPtr.Zero;
                        int langLen;
                        if (NativeMethods.QueryValue(buffer, "\\VarFileInfo\\Translation", out langBuffer, out langLen))
                        {
                            if (langLen > 0)
                            {
                                language = (ushort)Marshal.PtrToStructure(langBuffer, typeof(ushort));
                                codepage = (ushort)Marshal.PtrToStructure(IntPtrInTheHand.Add(langBuffer, 2), typeof(ushort));

                                IntPtr stringPtr;
                                int stringLen;

                                if (NativeMethods.QueryValue(buffer, string.Format(valueNameMask, language, codepage, "Comments"), out stringPtr, out stringLen))
                                {
                                    if (stringLen > 0)
                                    {
                                        comments = Marshal.PtrToStringUni(stringPtr, stringLen - 1);
                                    }
                                }

                                if (NativeMethods.QueryValue(buffer, string.Format(valueNameMask, language, codepage, "CompanyName"), out stringPtr, out stringLen))
                                {
                                    if (stringLen > 0)
                                    {
                                        companyName = Marshal.PtrToStringUni(stringPtr, stringLen - 1);
                                    }
                                }

                                if (NativeMethods.QueryValue(buffer, string.Format(valueNameMask, language, codepage, "FileDescription"), out stringPtr, out stringLen))
                                {
                                    if (stringLen > 0)
                                    {
                                        fileDescription = Marshal.PtrToStringUni(stringPtr, stringLen - 1);
                                    }
                                }

                                if (NativeMethods.QueryValue(buffer, string.Format(valueNameMask, language, codepage, "FileVersion"), out stringPtr, out stringLen))
                                {
                                    if (stringLen > 0)
                                    {
                                        fileVersion = Marshal.PtrToStringUni(stringPtr, stringLen - 1);
                                    }
                                }

                                if (NativeMethods.QueryValue(buffer, string.Format(valueNameMask, language, codepage, "InternalName"), out stringPtr, out stringLen))
                                {
                                    if (stringLen > 0)
                                    {
                                        internalName = Marshal.PtrToStringUni(stringPtr, stringLen - 1);
                                    }
                                    else
                                    {
                                        internalName = System.IO.Path.GetFileNameWithoutExtension(fileName);
                                    }
                                }

                                if (NativeMethods.QueryValue(buffer, string.Format(valueNameMask, language, codepage, "LegalCopyright"), out stringPtr, out stringLen))
                                {
                                    if (stringLen > 0)
                                    {
                                        legalCopyright = Marshal.PtrToStringUni(stringPtr, stringLen - 1);
                                    }
                                }

                                if (NativeMethods.QueryValue(buffer, string.Format(valueNameMask, language, codepage, "LegalTrademarks"), out stringPtr, out stringLen))
                                {
                                    if (stringLen > 0)
                                    {
                                        legalTrademarks = Marshal.PtrToStringUni(stringPtr, stringLen - 1);
                                    }
                                }

                                if (NativeMethods.QueryValue(buffer, string.Format(valueNameMask, language, codepage, "OriginalFilename"), out stringPtr, out stringLen))
                                {
                                    if (stringLen > 0)
                                    {
                                        originalFilename = Marshal.PtrToStringUni(stringPtr, stringLen - 1);
                                    }
                                }

                                if (NativeMethods.QueryValue(buffer, string.Format(valueNameMask, language, codepage, "ProductName"), out stringPtr, out stringLen))
                                {
                                    if (stringLen > 0)
                                    {
                                        productName = Marshal.PtrToStringUni(stringPtr, stringLen - 1);
                                    }
                                }

                                if (NativeMethods.QueryValue(buffer, string.Format(valueNameMask, language, codepage, "ProductVersion"), out stringPtr, out stringLen))
                                {
                                    if (stringLen > 0)
                                    {
                                        productVersion = Marshal.PtrToStringUni(stringPtr, stringLen - 1);
                                    }
                                }

                                if (NativeMethods.QueryValue(buffer, string.Format(valueNameMask, language, codepage, "ProductName"), out stringPtr, out stringLen))
                                {
                                    if (stringLen > 0)
                                    {
                                        productName = Marshal.PtrToStringUni(stringPtr, stringLen - 1);
                                    }
                                }

                                if (IsPrivateBuild)
                                {
                                    if (NativeMethods.QueryValue(buffer, string.Format(valueNameMask, language, codepage, "PrivateBuild"), out stringPtr, out stringLen))
                                    {
                                        if (stringLen > 0)
                                        {
                                            privateBuild = Marshal.PtrToStringUni(stringPtr, stringLen - 1);
                                        }
                                    }
                                }

                                if (IsSpecialBuild)
                                {
                                    if (NativeMethods.QueryValue(buffer, string.Format(valueNameMask, language, codepage, "SpecialBuild"), out stringPtr, out stringLen))
                                    {
                                        if (stringLen > 0)
                                        {
                                            specialBuild = Marshal.PtrToStringUni(stringPtr, stringLen - 1);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        throw InTheHand.ComponentModel.Win32ExceptionInTheHand.Create();
                    }

                }
                finally
                {
                    if (buffer != IntPtr.Zero)
                    {
                        //free native buffer
                        Marshal.FreeHGlobal(buffer);
                    }
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="FileVersionInfo"/> representing the version information associated with the specified file.
        /// </summary>
        /// <param name="fileName">The fully qualified path and name of the file to retrieve the version information for.</param>
        /// <returns>A <see cref="FileVersionInfo"/> containing information about the file.
        /// If the file information was not found, the <see cref="FileVersionInfo"/> contains only the name of the file requested.</returns>
        /// <exception cref="System.IO.FileNotFoundException">The file specified cannot be found.</exception>
        public static FileVersionInfo GetVersionInfo(string fileName)
        {
            //check if file exists first
            if (System.IO.File.Exists(fileName))
            {
                return new FileVersionInfo(fileName);
            }
            else
            {
                throw new System.IO.FileNotFoundException(fileName);
            }
        }


        #region File Name
        /// <summary>
        /// Gets the name of the file that this instance of <see cref="FileVersionInfo"/> describes.
        /// </summary>
        /// <value>The name of the file described by this instance of <see cref="FileVersionInfo"/>.</value>
        public string FileName
        {
            get
            {
                return filename;
            }
        }
        #endregion

        #region File Major Part
        /// <summary>
        /// Gets the major part of the version number.
        /// </summary>
        /// <value>A value representing the major part of the version number.</value>
        /// <remarks>Typically, a version number is displayed as "major number.minor number.build number.private part number".
        /// A file version number is a 64-bit number that holds the version number for a file as follows: 
        /// <list type="bullet"><item>The first 16 bits are the <see cref="FileMajorPart"/> number.</item> 
        /// <item>The next 16 bits are the <see cref="FileMinorPart"/> number.</item> 
        /// <item>The third set of 16 bits are the <see cref="FileBuildPart"/> number.</item>
        /// <item>The last 16 bits are the <see cref="FilePrivatePart"/> number.</item></list>
        /// This property gets the first set of 16 bits.</remarks>
        public int FileMajorPart
        {
            get
            {
                return (int)fixedFileInfo.dwFileVersionMajor;
            }
        }
        #endregion

        #region File Minor Part
        /// <summary>
        /// Gets the minor part of the version number.
        /// </summary>
        /// <value>A value representing the minor part of the version number of the file.</value>
        /// <remarks>Typically, a version number is displayed as "major number.minor number.build number.private part number".
        /// A file version number is a 64-bit number that holds the version number for a file as follows: 
        /// <list type="bullet"><item>The first 16 bits are the <see cref="FileMajorPart"/> number.</item> 
        /// <item>The next 16 bits are the <see cref="FileMinorPart"/> number.</item> 
        /// <item>The third set of 16 bits are the <see cref="FileBuildPart"/> number.</item>
        /// <item>The last 16 bits are the <see cref="FilePrivatePart"/> number.</item></list>
        /// This property gets the second set of 16 bits.</remarks>
        public int FileMinorPart
        {
            get
            {
                return (int)fixedFileInfo.dwFileVersionMinor;
            }
        }
        #endregion

        #region File Build Part
        /// <summary>
        /// Gets the build number of the file.
        /// </summary>
        /// <value>A value representing the build number of the file.</value>
        /// <remarks>Typically, a version number is displayed as "major number.minor number.build number.private part number".
        /// A file version number is a 64-bit number that holds the version number for a file as follows: 
        /// <list type="bullet"><item>The first 16 bits are the <see cref="FileMajorPart"/> number.</item> 
        /// <item>The next 16 bits are the <see cref="FileMinorPart"/> number.</item> 
        /// <item>The third set of 16 bits are the <see cref="FileBuildPart"/> number.</item>
        /// <item>The last 16 bits are the <see cref="FilePrivatePart"/> number.</item></list>
        /// This property gets the third set of 16 bits.</remarks>
        public int FileBuildPart
        {
            get
            {
                return (int)fixedFileInfo.dwFileVersionBuild;
            }
        }
        #endregion

        #region File Private Part
        /// <summary>
        /// Gets the file private part number.
        /// </summary>
        /// <value>A value representing the file private part number.</value>
        /// <remarks>Typically, a version number is displayed as "major number.minor number.build number.private part number".
        /// A file version number is a 64-bit number that holds the version number for a file as follows: 
        /// <list type="bullet"><item>The first 16 bits are the <see cref="FileMajorPart"/> number.</item> 
        /// <item>The next 16 bits are the <see cref="FileMinorPart"/> number.</item> 
        /// <item>The third set of 16 bits are the <see cref="FileBuildPart"/> number.</item>
        /// <item>The last 16 bits are the <see cref="FilePrivatePart"/> number.</item></list>
        /// This property gets the last set of 16 bits.</remarks>
        public int FilePrivatePart
        {
            get
            {
                return (int)fixedFileInfo.dwFileVersionPrivate;
            }
        }
        #endregion


        #region Product Major Part
        /// <summary>
        /// Gets the major part of the version number for the product this file is associated with.
        /// </summary>
        /// <value>A value representing the major part of the product version number.</value>
        /// <remarks>Typically, a version number is displayed as "major number.minor number.build number.private part number".
        /// A product version number is a 64-bit number that holds the version number for a product as follows: 
        /// <list type="bullet"><item>The first 16 bits are the <see cref="ProductMajorPart"/> number.</item> 
        /// <item>The next 16 bits are the <see cref="ProductMinorPart"/> number.</item> 
        /// <item>The third set of 16 bits are the <see cref="ProductBuildPart"/> number.</item>
        /// <item>The last 16 bits are the <see cref="ProductPrivatePart"/> number.</item></list>
        /// This property gets the first set of 16 bits.</remarks>
        public int ProductMajorPart
        {
            get
            {
                return (int)fixedFileInfo.dwProductVersionMajor;
            }
        }
        #endregion

        #region Product Minor Part
        /// <summary>
        /// Gets the minor part of the version number for the product the file is associated with.
        /// </summary>
        /// <value>A value representing the minor part of the product version number.</value>
        /// <remarks>Typically, a version number is displayed as "major number.minor number.build number.private part number".
        /// A product version number is a 64-bit number that holds the version number for a product as follows: 
        /// <list type="bullet"><item>The first 16 bits are the <see cref="ProductMajorPart"/> number.</item> 
        /// <item>The next 16 bits are the <see cref="ProductMinorPart"/> number.</item> 
        /// <item>The third set of 16 bits are the <see cref="ProductBuildPart"/> number.</item>
        /// <item>The last 16 bits are the <see cref="ProductPrivatePart"/> number.</item></list>
        /// This property gets the second set of 16 bits.</remarks>
        public int ProductMinorPart
        {
            get
            {
                return (int)fixedFileInfo.dwProductVersionMinor;
            }
        }
        #endregion

        #region Product Build Part
        /// <summary>
        /// Gets the build number of the product this file is associated with.
        /// </summary>
        /// <value>A value representing the build part of the product version number.</value>
        /// <remarks>Typically, a version number is displayed as "major number.minor number.build number.private part number".
        /// A product version number is a 64-bit number that holds the version number for a product as follows: 
        /// <list type="bullet"><item>The first 16 bits are the <see cref="ProductMajorPart"/> number.</item> 
        /// <item>The next 16 bits are the <see cref="ProductMinorPart"/> number.</item> 
        /// <item>The third set of 16 bits are the <see cref="ProductBuildPart"/> number.</item>
        /// <item>The last 16 bits are the <see cref="ProductPrivatePart"/> number.</item></list>
        /// This property gets the third set of 16 bits.</remarks>
        public int ProductBuildPart
        {
            get
            {
                return (int)fixedFileInfo.dwProductVersionBuild;
            }
        }
        #endregion

        #region Product Private Part
        /// <summary>
        /// Gets the private part number of the product this file is associated with.
        /// </summary>
        /// <value>A value representing the private part of the product version number.</value>
        /// <remarks>Typically, a version number is displayed as "major number.minor number.build number.private part number".
        /// A product version number is a 64-bit number that holds the version number for a product as follows: 
        /// <list type="bullet"><item>The first 16 bits are the <see cref="ProductMajorPart"/> number.</item> 
        /// <item>The next 16 bits are the <see cref="ProductMinorPart"/> number.</item> 
        /// <item>The third set of 16 bits are the <see cref="ProductBuildPart"/> number.</item>
        /// <item>The last 16 bits are the <see cref="ProductPrivatePart"/> number.</item></list>
        /// This property gets the last set of 16 bits.</remarks>
        public int ProductPrivatePart
        {
            get
            {
                return (int)fixedFileInfo.dwProductVersionPrivate;
            }
        }
        #endregion


        #region Is Debug
        /// <summary>
        /// Gets a value that specifies whether the file contains debugging information or is compiled with debugging features enabled.
        /// </summary>
        /// <value>true if the file contains debugging information or is compiled with debugging features enabled; otherwise, false.</value>\
        /// <remarks><para>The FileVersionInfo properties are based on version resource information built into the file.
        /// Version resources are often built into binary files such as .exe or .dll files; text files do not have version resource information.</para>
        /// Version resources are typically specified in a Win32 resource file, or in assembly attributes.
        /// The <b>IsDebug</b> property reflects the <b>VS_FF_DEBUG</b> flag value in the file's <b>VS_FIXEDFILEINFO</b> block, which is built from the VERSIONINFO resource in a Win32 resource file.
        /// For more information about specifying version resources in a Win32 resource file, see the Platform SDK About Resource Files topic and VERSIONINFO Resource topic topics.</remarks>
        public bool IsDebug
        {
            get
            {
                return (fixedFileInfo.dwFileFlagsMask.HasFlag(VS_FF.DEBUG) && fixedFileInfo.dwFileFlags.HasFlag(VS_FF.DEBUG));
            }
        }
        #endregion

        #region Is Patched
        /// <summary>
        /// Gets a value that specifies whether the file has been modified and is not identical to the original shipping file of the same version number.
        /// </summary>
        /// <value>true if the file is patched; otherwise, false.</value>
        public bool IsPatched
        {
            get
            {
                return (fixedFileInfo.dwFileFlagsMask.HasFlag(VS_FF.PATCHED) && fixedFileInfo.dwFileFlags.HasFlag(VS_FF.PATCHED));
            }
        }
        #endregion

        #region Is Pre-Release
        /// <summary>
        /// Gets a value that specifies whether the file is a development version, rather than a commercially released product.
        /// </summary>
        /// <value>true if the file is prerelease; otherwise, false.</value>
        public bool IsPreRelease
        {
            get
            {
                return (fixedFileInfo.dwFileFlagsMask.HasFlag(VS_FF.PRERELEASE) && fixedFileInfo.dwFileFlags.HasFlag(VS_FF.PRERELEASE));
            }
        }
        #endregion

        #region Is Private Build
        /// <summary>
        /// Gets a value that specifies whether the file was built using standard release procedures.
        /// </summary>
        /// <value>true if the file is a private build; false if the file was built using standard release procedures or if the file did not contain version information.</value>
        /// <remarks>If this value is true, <see cref="PrivateBuild"/> will describe how this version of the file differs from the standard version.</remarks>
        public bool IsPrivateBuild
        {
            get
            {
                return (fixedFileInfo.dwFileFlagsMask.HasFlag(VS_FF.PRIVATEBUILD) && fixedFileInfo.dwFileFlags.HasFlag(VS_FF.PRIVATEBUILD));
            }
        }
        #endregion

        #region Is Special Build
        /// <summary>
        /// Gets a value that specifies whether the file is a special build.
        /// </summary>
        /// <value>true if the file is a special build; otherwise, false.</value>
        /// <remarks>A file that is a special build was built using standard release procedures, but the file differs from a standard file of the same version number.
        /// If this value is true, the <see cref="SpecialBuild"/> property must specify how this file differs from the standard version.</remarks>
        public bool IsSpecialBuild
        {
            get
            {
                return (fixedFileInfo.dwFileFlagsMask.HasFlag(VS_FF.SPECIALBUILD) && fixedFileInfo.dwFileFlags.HasFlag(VS_FF.SPECIALBUILD));
            }
        }
        #endregion

        #region Comments
        private string comments = null;
        /// <summary>
        /// Gets the comments associated with the file.
        /// </summary>
        /// <value>The comments associated with the file or a null reference (Nothing in Visual Basic) if the file did not contain version information.</value>
        /// <remarks>This property contains additional information that can be displayed for diagnostic purposes.</remarks>
        public string Comments
        {
            get
            {
                return comments;
            }
        }

        #endregion

        #region Company Name
        private string companyName = null;
        /// <summary>
        /// Gets the name of the company that produced the file.
        /// </summary>
        /// <value>The name of the company that produced the file or a null reference (Nothing in Visual Basic) if the file did not contain version information.</value>
        public string CompanyName
        {
            get
            {
                return companyName;
            }
        }

        #endregion

        #region File Description
        private string fileDescription = null;
        /// <summary>
        /// Gets the description of the file.
        /// </summary>
        /// <value>The description of the file or a null reference (Nothing in Visual Basic) if the file did not contain version information.</value>
        public string FileDescription
        {
            get
            {
                return fileDescription;
            }
        }

        #endregion

        #region File Version
        private string fileVersion = null;
        /// <summary>
        /// Gets the file version number.
        /// </summary>
        /// <value>The version number of the file or a null reference (Nothing in Visual Basic) if the file did not contain version information.</value>
        public string FileVersion
        {
            get
            {
                return fileVersion;
            }
        }

        #endregion

        #region Internal Name
        private string internalName = null;
        /// <summary>
        /// Gets the internal name of the file, if one exists.
        /// </summary>
        /// <value>The internal name of the file.
        /// If none exists, this property will contain the original name of the file without the extension.</value>
        public string InternalName
        {
            get
            {
                return internalName;
            }
        }

        #endregion

        #region Legal Copyright
        private string legalCopyright = null;
        /// <summary>
        /// Gets all copyright notices that apply to the specified file.
        /// </summary>
        /// <value>The copyright notices that apply to the specified file.</value>
        /// <remarks>This should include the full text of all notices, legal symbols, copyright dates, and so on or a null reference (Nothing in Visual Basic) if the file did not contain version information.</remarks>
        public string LegalCopyright
        {
            get
            {
                return legalCopyright;
            }
        }

        #endregion

        #region Legal Trademarks
        private string legalTrademarks = null;
        /// <summary>
        /// Gets the trademarks and registered trademarks that apply to the file.
        /// </summary>
        /// <value>The trademarks and registered trademarks that apply to the file or a null reference (Nothing in Visual Basic) if the file did not contain version information.</value>
        /// <remarks>The legal trademarks include the full text of all notices, legal symbols, and trademark numbers.</remarks>
        public string LegalTrademarks
        {
            get
            {
                return legalTrademarks;
            }
        }
        #endregion

        #region Original Filename
        private string originalFilename = null;
        /// <summary>
        /// Gets the name the file was created with.
        /// </summary>
        /// <value>The name the file was created with or a null reference (Nothing in Visual Basic) if the file did not contain version information.</value>
        /// <remarks>This property enables an application to determine whether a file has been renamed.</remarks>
        public string OriginalFilename
        {
            get
            {
                return originalFilename;
            }
        }

        #endregion

        #region Private Build
        private string privateBuild = null;
        /// <summary>
        /// Gets information about a private version of the file.
        /// </summary>
        /// <value>Information about a private version of the file or a null reference (Nothing in Visual Basic) if the file did not contain version information.</value>
        /// <remarks>This information is present when <see cref="IsPrivateBuild"/> is true.</remarks>
        public string PrivateBuild
        {
            get
            {
                return privateBuild;
            }
        }
        #endregion

        #region Product Name
        private string productName = null;
        /// <summary>
        /// Gets the name of the product this file is distributed with.
        /// </summary>
        /// <value>The name of the product this file is distributed with or a null reference (Nothing in Visual Basic) if the file did not contain version information.</value>
        public string ProductName
        {
            get
            {
                return productName;
            }
        }
        #endregion

        #region Product Version
        private string productVersion = null;
        /// <summary>
        /// Gets the version of the product this file is distributed with.
        /// </summary>
        /// <value>The version of the product this file is distributed with or a null reference (Nothing in Visual Basic) if the file did not contain version information.</value>
        public string ProductVersion
        {
            get
            {
                return productVersion;
            }
        }

        #endregion

        #region Special Build
        private string specialBuild = null;
        /// <summary>
        /// Gets the special build information for the file.
        /// </summary>
        /// <value>The special build information for the file or a null reference (Nothing in Visual Basic) if the file did not contain version information.</value>
        public string SpecialBuild
        {
            get
            {
                return specialBuild;
            }
        }
        #endregion

        private static class NativeMethods
        {
            [DllImport("coredll", EntryPoint = "GetFileVersionInfo", SetLastError = true)]
            internal static extern bool GetInfo(string filename, int handle, int len, IntPtr buffer);

            [DllImport("coredll", EntryPoint = "GetFileVersionInfoSize", SetLastError = true)]
            internal static extern int GetInfoSize(string filename, out int handle);

            [DllImport("coredll", EntryPoint = "VerQueryValue", SetLastError = true)]
            internal static extern bool QueryValue(IntPtr buffer, string subblock, out IntPtr blockbuffer, out int len);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct VS_FIXEDFILEINFO
    {
        internal int dwSignature;
        internal int dwStrucVersion;
        internal ushort dwFileVersionMinor;
        internal ushort dwFileVersionMajor;
        internal ushort dwFileVersionPrivate;
        internal ushort dwFileVersionBuild;
        internal ushort dwProductVersionMinor;
        internal ushort dwProductVersionMajor;
        internal ushort dwProductVersionPrivate;
        internal ushort dwProductVersionBuild;
        internal VS_FF dwFileFlagsMask;
        internal VS_FF dwFileFlags;
        internal int dwFileOS;
        internal int dwFileType;
        internal int dwFileSubtype;

        internal long dwFileDate;
    }

    [Flags()]
    internal enum VS_FF : int
    {
        DEBUG = 0x00000001,
        PRERELEASE = 0x00000002,
        PATCHED = 0x00000004,
        PRIVATEBUILD = 0x00000008,
        INFOINFERRED = 0x00000010,
        SPECIALBUILD = 0x00000020,
    }
}
