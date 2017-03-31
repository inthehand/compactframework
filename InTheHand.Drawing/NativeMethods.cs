// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NativeMethods.cs" company="In The Hand Ltd">
// Copyright (c) 2007-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using InTheHand.Drawing.Drawing2D;

namespace InTheHand.Drawing
{

    internal static class NativeMethods
    {
        private const string coredll = "coredll";
        /// <summary>
        /// Converts a managed <see cref="Color"/> object to a COLORREF used by native drawing routines.
        /// </summary>
        /// <param name="c">Managed <see cref="Color"/> instance.</param>
        /// <returns></returns>
        internal static uint ToColorRef(Color c/*, bool a*/)
        {
            return /*(a ? (uint)c.A << 24 : 0) |*/ (uint)c.B << 16 | (uint)c.G << 8 | c.R;
        }

        //Graphics
        [DllImport(coredll, EntryPoint="GetDeviceCaps", SetLastError = true)]
        internal static extern int GdiGetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport(coredll, EntryPoint = "GetWindowDC", SetLastError = true)]
        internal static extern IntPtr GdiGetWindowDC(IntPtr hWnd);
        
        //MeasureString
        [DllImport(coredll, EntryPoint = "DrawText", SetLastError = true)]
        internal static extern int GdiDrawText(IntPtr hDC, string lpString, int nCount, ref RECT lpRect, DT uFormat);

        [Flags()]
        internal enum DT
        {
            TOP = 0x00000000,
            LEFT = 0x00000000,
            CENTER = 0x00000001,
            RIGHT = 0x00000002,
            VCENTER = 0x00000004,
            BOTTOM = 0x00000008,
            WORDBREAK = 0x00000010,
            SINGLELINE = 0x00000020,
            EXPANDTABS = 0x00000040,
            TABSTOP = 0x00000080,
            NOCLIP = 0x00000100,
            EXTERNALLEADING = 0x00000200,
            CALCRECT = 0x00000400,
            NOPREFIX = 0x00000800,
            INTERNAL = 0x00001000,
            EDITCONTROL = 0x00002000,
            END_ELLIPSIS = 0x00008000,
            RTLREADING = 0x00020000,
            WORD_ELLIPSIS = 0x00040000,
        }
        /*[DllImport(InTheHand.NativeMethods.coredll, SetLastError = true)]
        internal static extern IntPtr GetDCEx(IntPtr hWnd, IntPtr hrgnClip, DCX flags);

        [Flags()]
        internal enum DCX
        {
            WINDOW          = 0x00000001,
            CACHE           = 0x00000002,

            CLIPCHILDREN    = 0x00000008,
            CLIPSIBLINGS    = 0x00000010,

            EXCLUDERGN      = 0x00000040,
            INTERSECTRGN    = 0x00000080,

            EXCLUDEUPDATE   = 0x00000100,
            INTERSECTUPDATE = 0x00000200,
        }*/

        [DllImport(coredll, EntryPoint = "ReleaseDC", SetLastError = true)]
        internal static extern int GdiReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport(coredll, EntryPoint = "SetBkMode")]
        internal static extern int GdiSetBkMode(IntPtr hdc, int iBkMode);

        [DllImport(coredll, EntryPoint = "SetTextColor")]
        internal static extern int GdiSetTextColor(IntPtr hdc, int crColor);

        //[DllImport(InTheHand.NativeMethods.coredll)]
        //internal static extern int FillRect(IntPtr hDC, ref InTheHand.Drawing.RECT lprc, IntPtr hbr); 

        [DllImport(coredll, EntryPoint = "BitBlt", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GdiBitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight,
            IntPtr hdcSrc, int nXSrc, int nYSrc, CopyPixelOperation dwRop);


        //Draws a rounded rectangle using the current pen for the outline and current brush for fill
        [DllImport(coredll, EntryPoint = "RoundRect", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GdiRoundRect(IntPtr hdc,
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidth, int nHeight);

        //Selects the specified object (Pen, Brush etc) into the device context
        [DllImport(coredll, EntryPoint = "SelectObject", SetLastError = true)]
        internal static extern IntPtr GdiSelectObject(IntPtr hdc, IntPtr hgdiobj);

        //gets the current object (Pen, Brush etc) into the device context
        [DllImport(coredll, EntryPoint = "GetCurrentObject", SetLastError = true)]
        internal static extern IntPtr GdiGetCurrentObject(IntPtr hdc, uint uObjectType);

        //gets a stock object
        [DllImport(coredll, EntryPoint = "GetStockObject", SetLastError = true)]
        internal static extern IntPtr GdiGetStockObject(int fnObject); 

        //Creates a Pen handle with specific colour, width etc
        [DllImport(coredll, EntryPoint = "CreatePen", SetLastError = true)]
        internal static extern IntPtr GdiCreatePen(int fnPenStyle, int nWidth, uint crColor);

        //Creates a solid Brush from a specific colour.
        [DllImport(coredll, EntryPoint = "CreateSolidBrush", SetLastError = true)]
        internal static extern IntPtr GdiCreateSolidBrush(uint crColor);

        //Creates a patterned Brush from an image
        [DllImport(coredll, EntryPoint = "CreatePatternBrush", SetLastError = true)]
        internal static extern IntPtr GdiCreatePatternBrush(IntPtr hbmp);

        //Sets the brush origin (for patterned brushes)
        [DllImport(coredll, EntryPoint = "SetBrushOrgEx", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GdiSetBrushOrgEx(IntPtr hdc, int nXOrg, int nYOrg, IntPtr lppt);

        //Deletes a native drawing object
        [DllImport(coredll, EntryPoint = "DeleteObject", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GdiDeleteObject(IntPtr hObject);

        //Icon

        [DllImport(coredll)]
        internal static extern IntPtr LoadIcon(IntPtr hInstance, int lpIconName); 

        [DllImport(coredll, EntryPoint = "SHGetFileInfo")]
        internal static extern int GetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO psfi,
            int cbFileInfo, SHGFI uFlags);

        [StructLayout(LayoutKind.Sequential)]
        internal struct SHFILEINFO
        {
            internal IntPtr hIcon;                      // out: icon
            internal int iIcon;                      // out: icon index
            internal int dwAttributes;               // out: SFGAO_ flags
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            internal string szDisplayName;    // out: display name (or path)
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            internal string szTypeName;             // out: type name
        }

        [Flags()]
        internal enum SHGFI : int
        {
            ICON = 0x000000100,     // get icon
            DISPLAYNAME = 0x000000200,     // get display name
            TYPENAME = 0x000000400,     // get type name
            ATTRIBUTES = 0x000000800,     // get attributes
            SYSICONINDEX = 0x000004000,     // get system icon index
            SELECTICON = 0x000040000,     // get icon specifying selection state
            LARGEICON = 0x000000000,     // get large icon
            SMALLICON = 0x000000001,     // get small icon
            USEFILEATTRIBUTES = 0x000000010,     // use passed dwFileAttribute
            PIDL = 0x000000008,     // pszPath is a pidl
        }


        //Alpha Blend
        [DllImport(coredll, EntryPoint = "AlphaBlend", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GdiAlphaBlend(IntPtr hdcDest, 
            int nXOriginDest, int nYOriginDest,
            int nWidthDest, int nHeightDest,
            IntPtr hdcSrc,
            int nXOriginSrc, int nYOriginSrc,
            int nWidthSrc, int nHeightSrc,
            ref BLENDFUNCTION blendFunction);

        [StructLayout(LayoutKind.Sequential)]
        internal struct BLENDFUNCTION
        {
            internal byte BlendOp;
            internal byte BlendFlags;
            internal byte SourceConstantAlpha;
            internal byte AlphaFormat;
        }

        //Gradient
        [DllImport(coredll, EntryPoint = "GradientFill", SetLastError = true)]
        internal static extern bool GdiGradientFill(IntPtr hdc, TRIVERTEX[] pVertex, int nVertex, ref GRADIENT_RECT pMesh, int nCount, InTheHand.Drawing.Drawing2D.LinearGradientMode ulMode);

        [StructLayout(LayoutKind.Sequential)]
        internal struct GRADIENT_RECT
        {
            public int UpperLeft;
            public int LowerRight;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct TRIVERTEX
        {
            internal int x;
            internal int y;
            internal ushort Red;
            internal ushort Green;
            internal ushort Blue;
            internal ushort Alpha;
        }


        //SystemFonts
        [DllImport("aygshell.dll", EntryPoint = "SHGetUIMetrics", SetLastError = true)]
        internal static extern int GetUIMetrics(SHUIMETRIC shuim, out int pvBuffer, int cbBufferSize, out int pcbRequired);

        internal enum SHUIMETRIC
        {
            INVALID = 0,
            FONTSIZE_POINT,
            FONTSIZE_PIXEL,
            FONTSIZE_PERCENTAGE,
        }

    }

    
}