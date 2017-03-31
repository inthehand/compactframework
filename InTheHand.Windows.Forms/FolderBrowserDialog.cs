// In The Hand - .NET Components for Mobility
//
// InTheHand.Windows.Forms.FolderBrowserDialog
// 
// Copyright (c) 2003-2012 In The Hand Ltd, All rights reserved.

using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;

namespace InTheHand.Windows.Forms
{
	/// <summary>
	/// Represents a common dialog box that allows the user to choose a folder.
	/// </summary>
	/// <remarks>Throws a PlatformNotSupportedException if API is missing.</remarks>
	public class FolderBrowserDialog : CommonDialog
	{
		private BROWSEINFO info;
		private string folder = string.Empty;

        static FolderBrowserDialog()
        {
            InitCommonControls();
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="FolderBrowserDialog"/> class.
		/// </summary>
		public FolderBrowserDialog()
		{
			info = new BROWSEINFO();
            info.ulFlags = BIF.RETURNONLYFSDIRS | BIF.STATUSTEXT;
			info.lpszTitle = string.Empty;
		}

		/// <summary>
		/// Runs a common dialog box with a default owner.
		/// </summary>
        /// <returns><see cref="DialogResult">DialogResult.OK</see> if the user clicks OK in the dialog box; otherwise, <see cref="DialogResult">DialogResult.Cancel</see>.</returns>
        public new DialogResult ShowDialog()
		{
            return ShowDialog(null);
        }

        /// <summary>
        /// Runs a common dialog box with the specified owner.
        /// </summary>
        /// <param name="owner">Any object that implements <see cref="IWin32Window"/> that represents the top-level window that will own the modal dialog box.</param>
        /// <returns><see cref="DialogResult">DialogResult.OK</see> if the user clicks OK in the dialog box; otherwise, <see cref="DialogResult">DialogResult.Cancel</see>.</returns>
        public DialogResult ShowDialog(IWin32Window owner)
        {
            if (owner != null)
            {
                info.hwndOwner = owner.Handle;
            }

            if (InTheHand.WindowsCE.Forms.SystemSettingsInTheHand.Platform == Microsoft.WindowsCE.Forms.WinCEPlatform.WinCEGeneric)
            {
                IntPtr pitemidlist;

                try
                {
                    pitemidlist = SHBrowseForFolder(ref info);
                }
                catch (MissingMethodException mme)
                {
                    throw new PlatformNotSupportedException("Your platform doesn't support the SHBrowseForFolder API", mme);
                }

                if (pitemidlist == IntPtr.Zero)
                {
                    return DialogResult.Cancel;
                }

                // maxpath unicode chars
                byte[] buffer = new byte[520];
                bool success = SHGetPathFromIDList(pitemidlist, buffer);

                // get string from buffer
                if (success)
                {
                    folder = System.Text.Encoding.Unicode.GetString(buffer, 0, buffer.Length);

                    int nullindex = folder.IndexOf('\0');
                    if (nullindex != -1)
                    {
                        folder = folder.Substring(0, nullindex);
                    }
                }

                Marshal.FreeHGlobal(pitemidlist);
            }
            else
            {
                OPENFILENAME ofn = new OPENFILENAME();
                ofn.lStructSize = Marshal.SizeOf(ofn);
                ofn.hwndOwner = info.hwndOwner;
                ofn.lpstrTitle = info.lpszTitle;
                ofn.nMaxFile = (InTheHand.EnvironmentInTheHand.MaxPath + 1);
                ofn.lpstrFile = new string('\0', ofn.nMaxFile);
                ofn.Flags = OFN.PROJECT;
                bool success = GetOpenFileName(ref ofn);
                if (!success)
                {
                    return DialogResult.Cancel;
                }

                folder = ofn.lpstrFile;
                int nullIndex = folder.IndexOf('\0');
                if (nullIndex > -1)
                {
                    folder = folder.Substring(0, nullIndex);
                }
            }

			return DialogResult.OK;
		}

		/// <summary>
		/// Gets the path selected by the user.
		/// </summary>
		public string SelectedPath
		{
			get
			{
				return folder;
			}
		}

		/// <summary>
		/// Gets or sets the descriptive text displayed above the tree view control in the dialog box.
		/// </summary>
		public string Description
		{
			get
			{
				return info.lpszTitle;
			}

			set
			{
				info.lpszTitle = value;
			}
		}

		#region P/Invokes

		[DllImport("commctrl", SetLastError=true)]
		private static extern void InitCommonControls();

		[DllImport("ceshell", SetLastError=true)]
		private static extern IntPtr SHBrowseForFolder(ref BROWSEINFO lpbi);

		[DllImport("ceshell", SetLastError=true)]
		private static extern bool SHGetPathFromIDList(IntPtr pidl, byte[] pszPath); 

		#endregion

		#region helper class for BROWSEINFO struct

        [StructLayout(LayoutKind.Sequential)]
        private struct BROWSEINFO
        {
            internal IntPtr hwndOwner;
            internal IntPtr pidlRoot;
            [MarshalAs(UnmanagedType.LPWStr)]
            internal string pszDisplayName;
            [MarshalAs(UnmanagedType.LPWStr)]
            internal string lpszTitle;
            internal BIF ulFlags;
            internal IntPtr lpfn;
            internal IntPtr lParam;
            internal int iImage;
        }

        [Flags]
        private enum BIF : uint
        {
            RETURNONLYFSDIRS = 0x0001,  // For finding a folder to start document searching
            // DONTGOBELOWDOMAIN = 0x0002,  // For starting the Find Computer
            STATUSTEXT = 0x0004,
            // RETURNFSANCESTORS = 0x0008,
            // BROWSEFORCOMPUTER = 0x1000, // Browsing for Computers.
            // BROWSEFORPRINTER = 0x2000, // Browsing for Printers
        }

        // Windows Mobile

        [DllImport("coredll", EntryPoint = "GetOpenFileNameW")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetOpenFileName(ref OPENFILENAME lpofn);

        [StructLayout(LayoutKind.Sequential)]
        private struct OPENFILENAME
        {
            internal int lStructSize;
            internal IntPtr hwndOwner;
            private IntPtr hInstance;
            [MarshalAs(UnmanagedType.LPWStr)]
            internal string lpstrFilter;
            [MarshalAs(UnmanagedType.LPWStr)]
            private string lpstrCustomFilter;
            private int nMaxCustFilter;
            internal int nFilterIndex;
            [MarshalAs(UnmanagedType.LPWStr)]
            internal string lpstrFile;
            internal int nMaxFile;
            [MarshalAs(UnmanagedType.LPWStr)]
            internal string lpstrFileTitle;
            internal int nMaxFileTitle;
            [MarshalAs(UnmanagedType.LPWStr)]
            internal string lpstrInitialDir;
            [MarshalAs(UnmanagedType.LPWStr)]
            internal string lpstrTitle;
            internal OFN Flags;
            internal ushort nFileOffset;
            internal ushort nFileExtension;
            [MarshalAs(UnmanagedType.LPWStr)]
            internal string lpstrDefExt;
            private IntPtr lCustData;
            private IntPtr lpfnHook;
            [MarshalAs(UnmanagedType.LPWStr)]
            private string lpTemplateName;
        }

        [Flags]
        private enum OFN
        {
            OVERWRITEPROMPT = 0x00000002,
            HIDEREADONLY = 0x00000004,
            EXTENSIONDIFFERENT = 0x00000400,
            PATHMUSTEXIST = 0x00000800,
            FILEMUSTEXIST = 0x00001000,
            CREATEPROMPT = 0x00002000,
            PROJECT = 0x00400000,     // If this flag is set, the GetOpenFileName API will open the Project dialog for Pocket PC
            PROPERTY = 0x00800000,     // If this flag is set, the GetSaveFileName API will open the Propery dialog for Pocket PC
            SHOW_ALL = 0x01000000,
        }
		#endregion
	}
}
