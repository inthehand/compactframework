// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenFileDialog.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Windows.Controls
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

	/// <summary>
    /// Provides a dialog box that enables the user to select a file.
	/// </summary>
    /// <remarks>This class enables the user to open a file on the local computer or a networked computer.
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Embedded Compact</term><description>Windows Embedded Compact 7</description></item>
    /// </list>
    /// </remarks>
    public sealed class OpenFileDialog
    {
        // store properties for native function
        private NativeMethods.OPENFILENAME ofx;
        private string fileName;

        #region Constructor
        /// <summary>
		/// Initializes a new instance of <see cref="OpenFileDialog"/>.
		/// </summary>
        public OpenFileDialog()
        {
            initialDirectory = string.Empty;

            ofx.lStructSize = Marshal.SizeOf(ofx);

            ofx.lpstrInitialDir = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }
        #endregion

        #region Show Dialog
        /// <summary>
        /// Displays an OpenFileDialog that is modal to the Silverlight window.
        /// </summary>
        /// <returns>true if the user clicked OK; false if the user clicked Cancel or closed the dialog box.</returns>
        public bool? ShowDialog()
        {
            bool success = false;
            fileName = string.Empty;

            ofx.nMaxFile = 260;
            ofx.lpstrFile = InTheHand.Runtime.InteropServices.MarshalInTheHand.AllocHGlobal(ofx.nMaxFile * 2);

            try
            {
                if (!string.IsNullOrEmpty(initialDirectory))
                {
                    ofx.lpstrInitialDir = initialDirectory + "\0";
                }
                
                //standard open file dialog
                success = NativeMethods.GetOpenFileName(ref ofx);

                if (success && (ofx.lpstrFile != IntPtr.Zero))
                {
                    fileName = Marshal.PtrToStringUni(ofx.lpstrFile);
                    int nullIndex = fileName.IndexOf('\0');
                    if (nullIndex > -1)
                    {
                        fileName = fileName.Substring(0, nullIndex);
                    }
                }

                return success;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (ofx.lpstrFile != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(ofx.lpstrFile);
                    ofx.lpstrFile = IntPtr.Zero;
                }
            }
        }
        #endregion

        #region InitialDirectory
        private string initialDirectory = string.Empty;
        /// <summary>
        /// Gets or sets the directory displayed when the dialog starts.
        /// </summary>
        /// <value>The directory displayed when the dialog starts.
        /// The default is an empty string.</value>
        /// <exception cref="ArgumentException">The directory specified is not a valid file path.</exception>
        public string InitialDirectory
        {
            get
            {
                return initialDirectory;
            }

            set
            {
                // check for invalid path chars
                foreach (char invalidChar in Path.GetInvalidPathChars())
                {
                    if (value.Contains(invalidChar.ToString()))
                    {
                        throw new ArgumentException();
                    }
                }
                
                initialDirectory = value;
            }
        }
        #endregion

        #region File
        /// <summary>
        /// Gets a <see cref="FileInfo"/> object for the selected file.
        /// </summary>
        /// <value>The selected file.</value>
        public FileInfo File
        {
            get
            {
                if (fileName != null)
                {                   
                    return new FileInfo(fileName);
                }

                return null;
            }
        }
        #endregion

        #region Filter
        /// <summary>
        /// Gets or sets the current file name filter string, which determines the choices that appear in the "Save as file type" or "Files of type" box in the dialog box.
        /// </summary>
        /// <value>The file filtering options available in the dialog box.</value>
        /// <exception cref="ArgumentException">Filter format is invalid.</exception>
        /// <remarks>For each filtering option, the filter string contains a description of the filter, followed by the vertical bar (|) and the filter pattern. The strings for different filtering options are separated by the vertical bar.
        /// <para>The following is an example of a filter string:</para>
        /// <code>Text files (*.txt)|*.txt|All files (*.*)|*.*</code>
        /// <para>You can add several filter patterns to a filter by separating the file types with semicolons, for example:</para>
        /// <code>Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*</code>
        /// <para>Use the <see cref="FilterIndex"/> property to set which filtering option is shown first to the user.</para></remarks>
        public string Filter
        {
            get
            {
                return ofx.lpstrFilter.Replace('\0', '|').TrimEnd('|');
            }
            set
            {
                if ((value.IndexOf('|') > -1) && (value.IndexOf('*') > -1))
                {
                    ofx.lpstrFilter = value.Replace('|', '\0') + "\0\0";
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }
        #endregion

        #region Filter Index
        /// <summary>
        /// Gets or sets the index of the filter currently selected in the file dialog box.
        /// </summary>
        /// <value>A value containing the index of the filter currently selected in the file dialog box.
        /// The default value is 1.</value>
        /// <remarks>Use the FilterIndex property to set which filtering option is used.
        /// On Windows Mobile the user cannot change the filter themselves.
        /// The index of the first item in the Filter list is 1.</remarks>
        public int FilterIndex
        {
            get
            {
                return ofx.FilterIndex;
            }
            set
            {
                ofx.FilterIndex = value;
            }
        }
        #endregion

        internal static class NativeMethods
        {
            // open file dialog
            public enum OFN_SORTORDER
            {
                AUTO,
                DATE,
                NAME,
                SIZE,
                ASCENDING = 0x00008000,
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct OPENFILENAME
            {
                // Fields which map to OPENFILENAME
                public int lStructSize;
                public IntPtr hwndOwner;
                internal IntPtr hInstance;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string lpstrFilter;
                internal IntPtr lpstrCustomFilter;//not supported
                internal int nMaxCustFilter;//not supported
                public int FilterIndex;
                public IntPtr lpstrFile;
                public int nMaxFile; //max_path
                internal IntPtr lpstrFileTitle;
                internal int nMaxFileTitle;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string lpstrInitialDir;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string lpstrTitle;
                public int Flags;
                public short nFileOffset;
                public short nFileExtension;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string lpstrDefExt;
                internal int lCustData;//not supported
                internal int lpfnHook;//not supported
                internal int lpTemplateName;//not supported
            }

            [DllImport("coredll", EntryPoint = "GetOpenFileNameW")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetOpenFileName(ref OPENFILENAME lpofn);

            [DllImport("coredll", EntryPoint = "GetSaveFileNameW")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetSaveFileName(ref OPENFILENAME lpofn);
        }
    }
}
