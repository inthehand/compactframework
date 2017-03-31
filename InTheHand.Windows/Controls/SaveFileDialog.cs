// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveFileDialog.cs" company="In The Hand Ltd">
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
    /// Provides a dialog box that enables the user to specify options for saving a file.
    /// </summary>
    /// <remarks>This class enables the user to specify a file name.
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Embedded Compact</term><description>Windows Embedded Compact 7</description></item>
    /// </list>
    /// </remarks>
    public sealed class SaveFileDialog
    {
        //store properties for native function
        internal OpenFileDialog.NativeMethods.OPENFILENAME ofx;
        private string defaultFilename;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveFileDialog"/> class.
        /// </summary>
        public SaveFileDialog()
        {
            // send as OPENFILENAME struct
            ofx.lStructSize = Marshal.SizeOf(ofx);
            
            // overwrite prompt on by default
            ofx.Flags = 0x00000002;

            ofx.lpstrInitialDir = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }

        #region Default Ext
        /// <summary>
        /// Gets or sets the default file name extension applied to files that are saved with the <see cref="SaveFileDialog"/>.
        /// </summary>
        /// <value>The default file name extension applied to files that are saved with the <see cref="SaveFileDialog"/>, which can optionally include the dot character (.).</value>
        /// <remarks>The default file name extension is applied when the selected filter does not specify an extension (or the Filter property is not set) and the user does not specify an extension. 
        /// For example, if the DefaultExt property is set to .my, the selected item in the Save as type drop-down list has a filter string set to All Files|*.*, and a file name without a extension is specified in the File name box, the file is saved with the .my extension.</remarks>
        public string DefaultExt
        {
            get
            {
                if (ofx.lpstrDefExt == null)
                {
                    return string.Empty;
                }

                return ofx.lpstrDefExt;
            }

            set
            {
                ofx.lpstrDefExt = value.TrimStart('.');
            }
        }
        #endregion

        #region Default File Name
        /// <summary>
        /// Gets or sets the file name used if a file name is not specified by the user.
        /// </summary>
        /// <value>The file name used if a file name is not specified by the user.</value>
        /// <exception cref="ArgumentException">Occurs if the specified file name is null or contains invalid characters such as quotes ("), less than (&lt;), greater than (&gt;), pipe (|), backspace (\b), null (\0), tab (\t), colon (:), asterisk(*), question mark (?), and slashes (\\, /).</exception>
        public string DefaultFileName
        {
            get
            {
                if (defaultFilename == null)
                {
                    return string.Empty;
                }

                return defaultFilename;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException();
                }
                // can't contain invalid path chars
                foreach (char invalidChar in Path.GetInvalidPathChars())
                {
                    if (value.Contains(invalidChar.ToString()))
                    {
                        throw new ArgumentException();
                    }
                }
                //cannot contain a path - just a filename
                if (value != Path.GetFileName(value))
                {
                    throw new ArgumentException();
                }

                defaultFilename = value;
            }
        }
        #endregion

        #region File Name
        private string safeFileName = string.Empty;
        /// <summary>
        /// Gets the file name for the selected file associated with the <see cref="SaveFileDialog"/>.
        /// </summary>
        /// <value>The file name for the selected file associated with the <see cref="SaveFileDialog"/>.
        /// The default is <see cref="String.Empty"/>.</value>
        public string SafeFileName
        {
            get
            {
                return safeFileName;
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
        /// <para>Use the FilterIndex property to set which filtering option is shown first to the user.</para></remarks>
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

        #region Show Dialog
        /// <summary>
        /// Displays a <see cref="SaveFileDialog"/> that is modal to the Silverlight window.
        /// </summary>
        /// <returns></returns>
        public bool? ShowDialog()
        {
            safeFileName = string.Empty;

            ofx.nMaxFile = 260;
            ofx.lpstrFile = InTheHand.Runtime.InteropServices.MarshalInTheHand.AllocHGlobal(ofx.nMaxFile * 2);
            
            //write the default file name if specified
            if (!string.IsNullOrEmpty(defaultFilename))
            {
                byte[] defaultFileBytes = System.Text.Encoding.Unicode.GetBytes(defaultFilename);
                Marshal.Copy(defaultFileBytes, 0, ofx.lpstrFile, defaultFileBytes.Length);
            }

            try
            {
                bool result = OpenFileDialog.NativeMethods.GetSaveFileName(ref ofx);
                if (result && (ofx.lpstrFile != IntPtr.Zero))
                {
                    safeFileName = Marshal.PtrToStringUni(ofx.lpstrFile);
                    int nullIndex = safeFileName.IndexOf('\0');
                    if (nullIndex > -1)
                    {
                        safeFileName = safeFileName.Substring(0, nullIndex);
                    }
                }
                return result;
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
       
        #region Open File
        /// <summary>
        /// Opens the file specified by the <see cref="SafeFileName"/> property.
        /// </summary>
        /// <returns>The read/write file selected by the user.</returns>
        /// <exception cref="InvalidOperationException">No file was selected in the dialog box.</exception>
        /// <remarks>You should always check that the <see cref="ShowDialog"/> method returns true before accessing the stream associated with the file.
        /// Failure to do this will create an exception.</remarks>
        public Stream OpenFile()
        {
            if (!string.IsNullOrEmpty(SafeFileName))
            {
                return new FileStream(SafeFileName, FileMode.Create, FileAccess.ReadWrite);
            }

            throw new InvalidOperationException();
        }
        #endregion
    }
}
