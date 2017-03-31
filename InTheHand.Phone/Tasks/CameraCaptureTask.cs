// In The Hand - .NET Components for Mobility
//
// InTheHand.Phone.Tasks.CameraCaptureTask
// 
// Copyright (c) 2010-2011 In The Hand Ltd, All rights reserved.

using System;

using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace InTheHand.Phone.Tasks
{
    /// <summary>
    /// Allows an application to launch the Camera application.
    /// Use this to allow users to take a photo from your application.
    /// </summary>
    /// <remarks>Launch the Photo Chooser application by calling the <see cref="PhotoChooserTask.Show"/> method of the <see cref="PhotoChooserTask"/> object.
    /// Obtain the result of the chooser operation by handling the <see cref="ChooserBase{T}.Completed"/> event.</remarks>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
    /// <item><term>Windows Embedded</term><description>Windows Embedded CE 6.0</description></item>
    /// </list>
    /// </remarks>
    public sealed class CameraCaptureTask : ChooserBase<PhotoResult>
    {
        /// <summary>
        /// Shows the camera application.
        /// </summary>
        /// <remarks>Obtain the result of the chooser operation by handling the <see cref="ChooserBase{T}.Completed"/> event.</remarks>
        public override void Show()
        {
            IntPtr parentHwnd = IntPtr.Zero;
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(BackgroundShow), parentHwnd);
        }

        private void BackgroundShow(object state)
        {
            NativeMethods.SHCAMERACAPTURE shcc = new NativeMethods.SHCAMERACAPTURE();
            shcc.cbSize = Marshal.SizeOf(shcc);
            shcc.hwndOwner = (IntPtr)state;
           
            PhotoResult photoResult = null;

            try
            {
                TaskResult result = NativeMethods.CameraCapture(ref shcc) == 0 ? TaskResult.OK : TaskResult.Cancel;
                
                photoResult = new PhotoResult(result);
                if (result == TaskResult.OK)
                {
                    photoResult.OriginalFileName = shcc.szFile;
                }
            }
            catch (Exception ex)
            {
                photoResult = new PhotoResult(TaskResult.Cancel);
                photoResult.Error = ex;
            }

            FireCompleted(this, photoResult, null);
        }

        private static class NativeMethods
        {
            [DllImport("aygshell", EntryPoint = "SHCameraCapture", SetLastError = false)]
            internal static extern int CameraCapture(ref SHCAMERACAPTURE pshcc);

            [StructLayout(LayoutKind.Sequential)]
            internal struct SHCAMERACAPTURE
            {
                internal int cbSize;
                internal IntPtr hwndOwner;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = InTheHand.EnvironmentInTheHand.MaxPath)]
                internal string szFile;
                [MarshalAs(UnmanagedType.LPWStr)]
                internal string pszInitialDir;
                [MarshalAs(UnmanagedType.LPWStr)]
                internal string pszDefaultFileName;
                [MarshalAs(UnmanagedType.LPWStr)]
                internal string pszTitle;
                internal int StillQuality;
                internal int VideoTypes;
                internal int nResolutionWidth;
                internal int nResolutionHeight;
                internal int nVideoTimeLimit;
                internal int Mode;
            }
        }
    }
}
