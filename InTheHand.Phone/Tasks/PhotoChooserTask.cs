// In The Hand - .NET Components for Mobility
//
// InTheHand.Phone.Tasks.PhotoChooserTask
// 
// Copyright (c) 2010-2011 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace InTheHand.Phone.Tasks
{
    /// <summary>
    /// Allows an application to launch the Photo Chooser application.
    /// Use this to allow users to select a photo.
    /// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows Embedded Compact 7</description></item>
    /// </list>
    /// </remarks>  
    public sealed class PhotoChooserTask : ChooserBase<PhotoResult>
    {
        /// <summary>
        /// Gets or sets the maximum height and the height component of the aspect ratio for a cropping region set by the user during the photo choosing process.
        /// </summary>
        /// <remarks>This property is provided for code compatibility with Windows Phone 7 and is not implemented.</remarks>
        public int PixelHeight { set; get; }
        /// <summary>
        /// Gets or sets the maximum height and the height component of the aspect ratio for a cropping region set by the user during the photo choosing process.
        /// </summary>
        /// <remarks>This property is provided for code compatibility with Windows Phone 7 and is not implemented.</remarks>
        public int PixelWidth { set; get; }
        /// <summary>
        /// Gets or sets whether the user is presented with a button for launching the camera during the photo choosing process.
        /// </summary>
        public bool ShowCamera { set; get; }

        /// <summary>
        /// Shows the Photo Chooser application.
        /// </summary>
        /// <remarks>Obtain the result of the chooser operation by handling the <see cref="ChooserBase{T}.Completed"/> event.</remarks>
        public override void Show()
        {
            IntPtr parentHwnd = IntPtr.Zero;
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(BackgroundShow), parentHwnd);
        }

        private void BackgroundShow(object state)
        {
            IntPtr parentHwnd = (IntPtr)state;
            NativeMethods.OPENFILENAMEEX ofx = new NativeMethods.OPENFILENAMEEX();
            ofx.lStructSize = Marshal.SizeOf(ofx);
            
            ofx.hwndOwner = parentHwnd;
            ofx.lpstrFile = Marshal.AllocHGlobal(InTheHand.EnvironmentInTheHand.MaxPath * 2);
            ofx.nMaxFile = InTheHand.EnvironmentInTheHand.MaxPath;
            ofx.lpstrFilter = "All Images\0*.gif|*.jpg|*.png|*.bmp\0";
            //ofx.lpstrTitle = "Choose Picture";
            ofx.ExFlags = NativeMethods.OFN_EXFLAG.THUMBNAILVIEW;
            if (!ShowCamera)
            {
                ofx.ExFlags |= NativeMethods.OFN_EXFLAG.NOFILECREATE;
            }
            PhotoResult photoResult = null;
            try
            {
                TaskResult result = NativeMethods.GetOpenFileNameEx(ref ofx) ? TaskResult.OK : TaskResult.Cancel;
                photoResult = new PhotoResult(result);
                string filename = null;
                if (result == TaskResult.OK)
                {
                    filename = System.Runtime.InteropServices.Marshal.PtrToStringUni(ofx.lpstrFile);
                    photoResult.OriginalFileName = filename;
                }
                
            }
            catch (Exception ex)
            {
                photoResult = new PhotoResult(TaskResult.Cancel);
                photoResult.Error = ex;
            }
            finally
            {
                //free native memory
                if (ofx.lpstrFile != IntPtr.Zero)
                {
                    System.Runtime.InteropServices.Marshal.FreeHGlobal(ofx.lpstrFile);
                }
            }

            FireCompleted(this, photoResult, null);
        }

        private static class NativeMethods
        {
            [DllImport("aygshell", EntryPoint = "GetOpenFileNameEx")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetOpenFileNameEx(ref OPENFILENAMEEX lpofnex);

            //open file dialog
            public enum OFN_SORTORDER
            {
                AUTO,
                DATE,
                NAME,
                SIZE,
                ASCENDING = 0x00008000,
            }

            //
            // Extended Flags  
            //
            [Flags()]
            internal enum OFN_EXFLAG
            {
                DETAILSVIEW = 0x00000001,
                THUMBNAILVIEW = 0x00000002,
                LOCKDIRECTORY = 0x00000100,
                NOFILECREATE = 0x00000200,
                HIDEDRMPROTECTED = 0x00010000,     //If this flag is set and the DRM engine is installed - the PicturePicker will not show ANY DRM content
                HIDEDRMFORWARDLOCKED = 0x00020000,     //If this flag is set and the DRM engine is installed - the PicturePicker will not show ANY DRM FORWARD LOCK content
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct OPENFILENAMEEX
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
                public int nMaxFile;//max_path
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

                // Extended fields
                public OFN_SORTORDER dwSortOrder;
                public OFN_EXFLAG ExFlags;
            }
        }
    }
}
