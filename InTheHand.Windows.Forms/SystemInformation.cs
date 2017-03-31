// In The Hand - .NET Components for Mobility
//
// InTheHand.Windows.Forms.SystemInformationHelper
// 
// Copyright (c) 2002-2010 In The Hand Ltd, All rights reserved.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace InTheHand.Windows.Forms
{
	/// <summary>
	/// Provides information about the current system environment.
	/// </summary>
    /// <remarks>The <see cref="SystemInformationInTheHand"/> class provides static properties that can be used to get information about the current system environment.
    /// The class provides access to information such as Windows display element sizes, operating system settings and the capabilities of hardware installed on the system.</remarks>
	/// <seealso cref="SystemInformation"/>
    public static class SystemInformationInTheHand
	{
		/// <summary>
		/// Gets the height, in pixels, of the standard title bar area of a window.
		/// </summary>
		public static int CaptionHeight
		{
			get
			{
                if (ControlInTheHand.designMode)
                {
                    return 24;
                }
                else
                {
                    return NativeMethods.GetSystemMetrics(NativeMethods.SM.CYCAPTION);
                }
			}
		}

        /// <summary>
		/// Gets the maximum size, in pixels, that a cursor can occupy.
		/// </summary>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Embedded Compact</term><description>Windows CE .NET 4.2 and later</description></item>
        /// </list>
        /// </remarks>
		public static Size CursorSize
		{
			get
			{
                return new Size(NativeMethods.GetSystemMetrics(NativeMethods.SM.CXCURSOR), NativeMethods.GetSystemMetrics(NativeMethods.SM.CYCURSOR));
			}
		}
        
		/// <summary>
		/// Gets a value indicating whether the debug version of Windows CE is installed.
		/// </summary>
        /// <value>true if the debugging version of Windows CE is installed; otherwise, false.</value>
		public static bool DebugOS
		{
			get
			{
                return (NativeMethods.GetSystemMetrics(NativeMethods.SM.DEBUG) != 0);
			}
		}

		/// <summary>
		/// Gets the dimensions, in pixels, of the area within which the user must click twice for the operating system to consider the two clicks a double-click.
		/// </summary>
		public static Size DoubleClickSize
		{
			get
			{
                return new Size(NativeMethods.GetSystemMetrics(NativeMethods.SM.CXDOUBLECLK), NativeMethods.GetSystemMetrics(NativeMethods.SM.CYDOUBLECLK));
			}
		}
        
		//for completeness wrap the framework property
		/// <summary>
		/// Gets the maximum number of milliseconds allowed between mouse clicks for a double-click to be valid.
		/// </summary>
		public static int DoubleClickTime
		{
			get
			{
				return SystemInformation.DoubleClickTime;
			}
		}

		/// <summary>
		/// Gets the thickness, in pixels, of the frame border of a window that has a caption and is not resizable.
		/// </summary>
		public static Size FixedFrameBorderSize
		{
			get
			{
                return new Size(NativeMethods.GetSystemMetrics(NativeMethods.SM.CXDLGFRAME), NativeMethods.GetSystemMetrics(NativeMethods.SM.CYDLGFRAME));
			}
		}

		/// <summary>
		/// Gets the width, in pixels, of the arrow bitmap on the horizontal scroll bar.
		/// </summary>
		public static int HorizontalScrollBarArrowWidth
		{
			get
			{
                if (ControlInTheHand.designMode)
                {
                    return 16;
                }
                else
                {
                    return NativeMethods.GetSystemMetrics(NativeMethods.SM.CXHSCROLL);
                }
			}
		}

		/// <summary>
		/// Gets the default height, in pixels, of the horizontal scroll bar.
		/// </summary>
		public static int HorizontalScrollBarHeight
		{
			get
			{
                if (ControlInTheHand.designMode)
                {
                    return 16;
                }
                else
                {
                    return NativeMethods.GetSystemMetrics(NativeMethods.SM.CYHSCROLL);
                }
			}
		}

		/// <summary>
		/// Gets the dimensions, in pixels, of the Windows default program icon size.
		/// </summary>
		public static Size IconSize
		{
			get
			{
                if (ControlInTheHand.designMode)
                {
                    return new Size(32, 32);
                }
                else
                {
                    return new Size(NativeMethods.GetSystemMetrics(NativeMethods.SM.CXICON), NativeMethods.GetSystemMetrics(NativeMethods.SM.CYICON));
                }
			}
		}

		/// <summary>
		/// Gets the size, in pixels, of the grid square used to arrange icons in a large-icon view.
		/// </summary>
		public static Size IconSpacingSize
		{
			get
			{
                if (ControlInTheHand.designMode)
                {
                    return new Size(16, 6) ;
                }
                else
                {
                    return new Size(NativeMethods.GetSystemMetrics(NativeMethods.SM.CXICONSPACING), NativeMethods.GetSystemMetrics(NativeMethods.SM.CXICONSPACING));
                }
			}
        }

        /// <summary>
		/// Gets the height, in pixels, of one line of a menu.
		/// </summary>
		public static int MenuHeight
		{
			get
			{
                return SystemInformation.MenuHeight;
			}
        }

        /// <summary>
		/// Gets the number of display monitors.
		/// </summary>
        /// <value>The number of monitors that make up the desktop.</value>
        /// <remarks>Windows Mobile devices use only a single display.
        /// The <b>MonitorCount</b> property indicates the number of monitors currently recognized by the operating system.
        /// This property returns a value greater than one only if multiple monitors are currently recognized by the operating system.</remarks>
		public static int MonitorCount
		{
			get
			{
                int cmons = NativeMethods.GetSystemMetrics(NativeMethods.SM.CMONITORS);
				if(cmons == 0)
				{
					return 1;
				}

				return cmons;
			}
		}

		/// <summary>
		/// Gets a value indicating whether all the display monitors are using the same pixel color format.
		/// </summary>
        /// <value>true if all monitors are using the same pixel color format; otherwise, false.</value>
		public static bool MonitorsSameDisplayFormat
		{
			get
			{
				if(MonitorCount > 1)
				{
                    return (NativeMethods.GetSystemMetrics(NativeMethods.SM.SAMEDISPLAYFORMAT) != 0);
				}
				return true;
			}
		}

        /* Not supported Windows CE
		/// <summary>
		/// Gets a value indicating whether a mouse is installed.
		/// </summary>
		public static bool MousePresent
		{
			get
			{
                return (NativeMethods.GetSystemMetrics(NativeMethods.SM.MOUSEPRESENT) != 0);
			}
		}*/

        
        private static PowerStatus powerStatus;	
		/// <summary>
		/// Gets the current system power status.
		/// </summary>
		public static PowerStatus PowerStatus
		{
			get
			{
				if(powerStatus==null)
				{
					powerStatus = new PowerStatus();
				}

				return powerStatus;

			}
		}

        /// <summary>
		/// Gets the dimensions, in pixels, of the current video mode of the primary display.
		/// </summary>
		public static Size PrimaryMonitorSize
		{
			get
			{
                return Screen.PrimaryScreen.Bounds.Size;
			}
        }

        /// <summary>
		/// Gets the dimensions, in pixels, of a small icon.
		/// </summary>
		public static Size SmallIconSize
		{
			get
			{
                if (ControlInTheHand.designMode)
                {
                    return new Size(16, 16);
                }
                else
                {
                    return new Size(NativeMethods.GetSystemMetrics(NativeMethods.SM.CXSMICON), NativeMethods.GetSystemMetrics(NativeMethods.SM.CYSMICON));
                }
			}
        }

        /// <summary>
		/// Gets the height, in pixels, of the arrow bitmap on the vertical scroll bar.
		/// </summary>
		public static int VerticalScrollBarArrowHeight
		{
			get
			{
                if (ControlInTheHand.designMode)
                {
                    return 16;
                }
                else
                {
                    return NativeMethods.GetSystemMetrics(NativeMethods.SM.CYVSCROLL);
                }
			}
        }

        /// <summary>
		/// Gets the default width, in pixels, of the vertical scroll bar.
		/// </summary>
		public static int VerticalScrollBarWidth
		{
			get
			{
                if (ControlInTheHand.designMode)
                {
                    return 16;
                }
                else
                {
                    return NativeMethods.GetSystemMetrics(NativeMethods.SM.CXVSCROLL);
                }
			}
        }

		/// <summary>
		/// Gets the bounds of the virtual screen.
		/// </summary>
		public static Rectangle VirtualScreen
		{
			get
			{
                return new Rectangle(NativeMethods.GetSystemMetrics(NativeMethods.SM.XVIRTUALSCREEN), NativeMethods.GetSystemMetrics(NativeMethods.SM.YVIRTUALSCREEN), NativeMethods.GetSystemMetrics(NativeMethods.SM.CXVIRTUALSCREEN), NativeMethods.GetSystemMetrics(NativeMethods.SM.CYVIRTUALSCREEN));
			}
		}

		/// <summary>
		/// Gets the size, in pixels, of the working area of the screen.
		/// </summary>
		public static Rectangle WorkingArea
		{
			get
			{
				return Screen.PrimaryScreen.WorkingArea;
			}
		}
	}
}