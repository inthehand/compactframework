// In The Hand - .NET Components for Mobility
//
// InTheHand.Windows.Forms.ListView
// 
// Copyright (c) 2007-2012 In The Hand Ltd, All rights reserved.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using InTheHand.Drawing;
using Microsoft.WindowsCE.Forms;

namespace InTheHand.Windows.Forms
{
    /// <summary>
    /// Provides supporting methods for <see cref="ListView"/>.
    /// </summary>
    /// <seealso cref="ListView"/>
    public static class ListViewInTheHand
    {
        internal const int LVM_SETEXTENDEDLISTVIEWSTYLE = 0x1036;
        internal const int LVM_GETEXTENDEDLISTVIEWSTYLE = 0x1037;

        internal const int LVS_EX_GRIDLINES = 0x00000001;
        internal const int LVS_EX_CHECKBOXES = 0x00000004;
        internal const int LVS_EX_DOUBLEBUFFER = 0x00010000;
        internal const int LVS_EX_GRADIENT = 0x20000000;
        internal const int LVS_EX_THEME = 0x02000000;

        internal const int LVM_GETITEMRECT = 0x100E;
        //internal const int LVM_GETSUBITEMRECT = 0x1037;


        internal const int LVM_SETBKIMAGE = 0x108a;
        internal const int LVM_GETBKIMAGE = 0x108b;

        #region Double Buffered
        /// <summary>
        /// Gets a value indicating whether this control should redraw its surface using a secondary buffer to reduce or prevent flicker.
        /// </summary>
        /// <param name="listView">The <see cref="ListView"/> control.</param>
        /// <returns>true if the surface of the control should be drawn using double buffering; otherwise, false.</returns>
        public static bool GetDoubleBuffered(this ListView listView)
        {
            int lvex = NativeMethods.SendMessage(listView.Handle, LVM_GETEXTENDEDLISTVIEWSTYLE, 0, 0);
            return (lvex & LVS_EX_DOUBLEBUFFER) == LVS_EX_DOUBLEBUFFER;
        }

        /// <summary>
        /// Sets a value indicating whether this control should redraw its surface using a secondary buffer to reduce or prevent flicker.
        /// </summary>
        /// <param name="listView">The <see cref="ListView"/> control.</param>
        /// <param name="value">true if the surface of the control should be drawn using double buffering; otherwise, false.</param>
        public static void SetDoubleBuffered(this ListView listView, bool value)
        {
            int lvex = NativeMethods.SendMessage(listView.Handle, LVM_GETEXTENDEDLISTVIEWSTYLE, 0, 0);
            NativeMethods.SendMessage(listView.Handle, LVM_SETEXTENDEDLISTVIEWSTYLE, 0, value ? (lvex | LVS_EX_DOUBLEBUFFER) : (lvex & ~LVS_EX_DOUBLEBUFFER));
        }
        #endregion

        #region Gradient
        /// <summary>
        /// Gets a value indicating whether a gradient background is drawn for the control.
        /// </summary>
        /// <param name="listView">The <see cref="ListView"/> control.</param>
        /// <returns></returns>
        [Obsolete()]
        public static bool GetGradient(this ListView listView)
        {
            int lvex = NativeMethods.SendMessage(listView.Handle, LVM_GETEXTENDEDLISTVIEWSTYLE, 0, 0);
            return (lvex & LVS_EX_GRADIENT) == LVS_EX_GRADIENT;
        }

        /// <summary>
        /// Applies the gradient extended style to the <see cref="ListView"/> control.
        /// </summary>
        /// <param name="listView">The <see cref="ListView"/> control.</param>
        /// <param name="enable">true if gradient is to be drawn behind list; otherwise, false. The default is false.</param>
        [Obsolete()]
        public static void SetGradient(this ListView listView, bool enable)
        {
            int lvex = NativeMethods.SendMessage(listView.Handle, LVM_GETEXTENDEDLISTVIEWSTYLE, 0, 0);
            NativeMethods.SendMessage(listView.Handle, LVM_SETEXTENDEDLISTVIEWSTYLE, 0, enable ? (lvex | LVS_EX_GRADIENT) : (lvex & ~LVS_EX_GRADIENT));
        }
        #endregion

        #region Theme
        /// <summary>
        /// Gets a value indicating whether this control should draw using the currently selected theme.
        /// </summary>
        /// <param name="listView">The <see cref="ListView"/> control.</param>
        /// <returns></returns>
        /// <remarks>
        /// <list type="table"><listheader><term>Requirements</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile 6.5 and later</description></item>
        /// </list>
        /// </remarks>
        public static bool GetTheme(this ListView listView)
        {
            int lvex = NativeMethods.SendMessage(listView.Handle, LVM_GETEXTENDEDLISTVIEWSTYLE, 0, 0);
            return (lvex & LVS_EX_THEME) == LVS_EX_THEME;
        }

        /// <summary>
        /// Sets a value indicating whether this control should draw using the currently selected theme.
        /// </summary>
        /// <param name="listView">The <see cref="ListView"/> control.</param>
        /// <param name="enable">true if theme is to be used; otherwise, false. The default is false.</param>
        /// <remarks>
        /// <list type="table"><listheader><term>Requirements</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile 6.5 and later</description></item>
        /// </list>
        /// </remarks>
        public static void SetTheme(this ListView listView, bool enable)
        {
            int lvex = NativeMethods.SendMessage(listView.Handle, LVM_GETEXTENDEDLISTVIEWSTYLE, 0, 0);
            NativeMethods.SendMessage(listView.Handle, LVM_SETEXTENDEDLISTVIEWSTYLE, 0, enable ? (lvex | LVS_EX_THEME) : (lvex & ~LVS_EX_THEME));
            listView.Invalidate();
        }
        #endregion

        #region Grid Lines
        /// <summary>
        /// Gets a value indicating whether grid lines appear between the rows and columns containing the items and subitems in the control.
        /// </summary>
        /// <param name="listView">The <see cref="ListView"/> control.</param>
        /// <returns>true if grid lines are drawn around items and subitems; otherwise, false.
        /// The default is false.</returns>
        public static bool GetGridLines(this ListView listView)
        {
            int lvex = NativeMethods.SendMessage(listView.Handle, LVM_GETEXTENDEDLISTVIEWSTYLE, 0, 0);
            return (lvex & LVS_EX_GRIDLINES) == LVS_EX_GRIDLINES;
        }
        
        /// <summary>
        /// Sets a value indicating whether grid lines appear between the rows and columns containing the items and subitems in the control.
        /// </summary>
        /// <param name="listView">The <see cref="ListView"/> control.</param>
        /// <param name="enable">true if grid lines are drawn around items and subitems; otherwise, false. The default is false.</param>
        /// <remarks>The GridLines property has no effect unless the <see cref="ListView.View"/> property of the <see cref="ListView"/> control is set to Details.
        /// The GridLines property allows you to display lines to identify the rows and columns that are displayed in the <see cref="ListView"/> control when it displays items and their subitems.
        /// The grid lines that are displayed do not provide the ability to resize rows and columns as an application such as Microsoft Excel does.
        /// Only columns can be resized, if column headers are displayed, by moving the mouse pointer to the right side of the column to resize and then clicking and dragging until the column is the size you want.
        /// The grid lines feature is used to provide the user of the control with visible boundaries around items and subitems.</remarks>
        public static void SetGridLines(this ListView listView, bool enable)
        {
            int lvex = NativeMethods.SendMessage(listView.Handle, LVM_GETEXTENDEDLISTVIEWSTYLE, 0, 0);
            NativeMethods.SendMessage(listView.Handle, LVM_SETEXTENDEDLISTVIEWSTYLE, 0, enable ? (lvex | LVS_EX_GRIDLINES) : (lvex & ~LVS_EX_GRIDLINES));
        }
        #endregion

        /// <summary>
        /// Retrieves the bounding rectangle for a specific item within the list view control.
        /// </summary>
        /// <param name="listView">The <see cref="ListView"/> control.</param>
        /// <param name="index">The zero-based index of the item within the <see cref="ListView.ListViewItemCollection"/> whose bounding rectangle you want to return.</param>
        /// <returns>A <see cref="Rectangle"/> that represents the bounding rectangle for the specified portion of the specified <see cref="ListViewItem"/>.</returns>
        public static Rectangle GetItemRect(this ListView listView, int index)
        {
            return GetItemRect(listView, index, ItemBoundsPortion.Entire);
        }

        /// <summary>
        /// Retrieves the specified portion of the bounding rectangle for a specific item within the list view control.
        /// </summary>
        /// <param name="listView">The <see cref="ListView"/> control.</param>
        /// <param name="index">The zero-based index of the item within the <see cref="ListView.ListViewItemCollection"/> whose bounding rectangle you want to return.</param>
        /// <param name="portion">One of the <see cref="ItemBoundsPortion"/> values that represents a portion of the <see cref="ListViewItem"/> for which to retrieve the bounding rectangle.</param>
        /// <returns>A <see cref="Rectangle"/> that represents the bounding rectangle for the specified portion of the specified <see cref="ListViewItem"/>.</returns>
        public static Rectangle GetItemRect(this ListView listView, int index, ItemBoundsPortion portion)
        {
            RECT r = new RECT();
            r.left = (int)portion;
           
            int result = NativeMethods.SendMessage(listView.Handle, LVM_GETITEMRECT, index, ref r);

            return r.ToRectangle();
        }

        /// <summary>
        /// Sets the background image displayed in the <see cref="ListView"/>.
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="bitmap">A <see cref="Bitmap"/> that represents the image to display in the background of the control.</param>
        /// <param name="tileLayout">Sets a value indicating whether the background image of the <see cref="ListView"/> should be tiled.</param>
        public static void SetBackgroundImage(this ListView listView, Bitmap bitmap, bool tileLayout)
        {
            LVBKIMAGE lvBkImage = new LVBKIMAGE();
            if (bitmap == null)
                lvBkImage.ulFlags = 0;
            else
            {
                lvBkImage.ulFlags = LVBKIF.SOURCE_HBITMAP | (tileLayout ? LVBKIF.STYLE_TILE : 0);
                lvBkImage.hbm = bitmap.GetHbitmap();
                //lvBkImage.xOffsetPercent = xOffsetPercent;
                //lvBkImage.yOffsetPercent = yOffsetPercent;
            }

            NativeMethods.SendMessage(listView.Handle, LVM_SETBKIMAGE, 0, ref lvBkImage);
        }

        /// <summary>
        /// Gets or sets the background image displayed in the <see cref="ListView"/>.
        /// </summary>
        /// <param name="listView"></param>
        /// <returns></returns>
        public static Bitmap GetBackgroundImage(this ListView listView)
        {
            LVBKIMAGE lvBkImage = new LVBKIMAGE();
            //lvBkImage.ulFlags = LVBKIF.SOURCE_HBITMAP;
 
            NativeMethods.SendMessage(listView.Handle, LVM_GETBKIMAGE, 0, ref lvBkImage);
 
            if (lvBkImage.hbm == IntPtr.Zero)
                return null;
            else
                return Bitmap.FromHbitmap(lvBkImage.hbm);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the background image of the <see cref="ListView"/> should be tiled.
        /// </summary>
        /// <param name="listView"></param>
        /// <returns>true if the background image of the <see cref="ListView"/> should be tiled; otherwise, false.
        /// The default is false.</returns>
        public static bool GetBackgroundImageTiled(this ListView listView)
        {
            LVBKIMAGE lvBkImage = new LVBKIMAGE();
            //lvBkImage.ulFlags = LVBKIF.SOURCE_HBITMAP;

            NativeMethods.SendMessage(listView.Handle, LVM_GETBKIMAGE, 0, ref lvBkImage);

            if ((lvBkImage.ulFlags & LVBKIF.STYLE_TILE) == LVBKIF.STYLE_TILE)
                return true;
            else
                return false;
        }
    }

    /// <summary>
    /// Specifies a portion of the list view item from which to retrieve the bounding rectangle.
    /// </summary>
    public enum ItemBoundsPortion
    {
        /// <summary>
        /// The bounding rectangle of the entire item, including the icon, the item text, and the subitem text (if displayed), should be retrieved.
        /// </summary>
        Entire = 0,

        /// <summary>
        /// The bounding rectangle of the icon or small icon should be retrieved.
        /// </summary>
        Icon = 1,

        /// <summary>
        /// The bounding rectangle of the item text should be retrieved.
        /// </summary>
        Label = 2,

        /// <summary>
        /// The bounding rectangle of the icon or small icon and the item text should be retrieved.
        /// In all views except the details view of the <see cref="ListView"/>, this value specifies the same bounding rectangle as the <see cref="Entire"/> value.
        /// In details view, this value specifies the bounding rectangle specified by the <see cref="Entire"/> value without the subitems.
        /// If the CheckBoxes property is set to true, this property does not include the area of the check boxes in its bounding rectangle.
        /// To include the entire item, including the check boxes, use the <see cref="Entire"/> value when calling the <see cref="ListViewInTheHand.GetItemRect(ListView,int)"/> method.
        /// </summary>
        ItemOnly = 3,
    }

    internal struct LVBKIMAGE
    {
        public LVBKIF ulFlags;
        public IntPtr hbm;
        IntPtr pszImage; // not supported
        int cchImageMax;
        public int xOffsetPercent;
        public int yOffsetPercent;
    }

    [Flags()]
    internal enum LVBKIF
    {
        SOURCE_HBITMAP = 0x00000001,

        STYLE_TILE = 0x00000010,
    }

    [Flags()]
    internal enum LVCDI
    {
        ITEM = 0x00000000,
        GROUP = 0x00000001,
    }

    internal enum LVGA
    {
        HEADER_LEFT = 0x00000001,
        HEADER_CENTER = 0x00000002,
        HEADER_RIGHT = 0x00000004,  // Don't forget to validate exclusivity
        //#define LVGA_FOOTER_LEFT    0x00000008
        //#define LVGA_FOOTER_CENTER  0x00000010
        //#define LVGA_FOOTER_RIGHT   0x00000020  // Don't forget to validate exclusivity
    }
}