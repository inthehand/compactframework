// In The Hand - .NET Components for Mobility
//
// InTheHand.Windows.Forms.NativeWindow
// 
// Copyright (c) 2002-2010 In The Hand Ltd, All rights reserved.

using System;
using Microsoft.WindowsCE.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace InTheHand.Windows.Forms
{
	/// <summary>
	/// Provides a low-level encapsulation of a window handle and a window procedure. 
	/// </summary>
	public class NativeWindow
	{
		private IntPtr handle;
		private IntPtr defWindowProc;
		private IntPtr windowProcPtr = IntPtr.Zero;
		private WndProcDelegate windowProc;
		private bool ownHandle;

        /// <summary>
        /// Initializes an instance of the <see cref="NativeWindow"/> class.
        /// </summary>
		public NativeWindow()
		{
			handle = IntPtr.Zero;
			ownHandle = true;
		}

		#region Handle
        /// <summary>
        /// Gets the handle for this window.
        /// </summary>
		public IntPtr Handle 
		{
			get 
			{
				return handle;
			}
		}

		#endregion

		#region methods

		/// <summary>
		/// Assigns a handle to this window.   
		/// </summary>
		/// <param name="handle">The handle to assign to this window.</param>
		public void AssignHandle(IntPtr handle) 
		{
            if (this.handle != handle)
            {
                if (this.handle != IntPtr.Zero)
                {
                    ReleaseHandle();
                }

                if (handle != IntPtr.Zero)
                {
                    this.handle = handle;
                    this.ownHandle = false;
                    Subclass();
                }

                OnHandleChange();
            }
		}

		/// <summary>
		///  Creates a window and its handle with the specified creation parameters.   
		/// </summary>
		/// <param name="cp">CreateParams that specifies the creation parameters for this window.</param>
		public virtual void CreateHandle(CreateParams cp) 
		{
			IntPtr ptr = IntPtr.Zero;

			if (cp != null) 
			{
				IntPtr hInstance = NativeMethods.GetModuleHandle(null);

				ptr = NativeMethods.CreateWindowEx((uint)cp.ExStyle, cp.ClassName, cp.Caption, (uint)cp.Style, cp.X, cp.Y, cp.Width, cp.Height, cp.Parent, IntPtr.Zero, hInstance, 0);

				if (ptr == IntPtr.Zero)
				{
                    throw InTheHand.ComponentModel.Win32ExceptionInTheHand.Create();
				}

				handle = ptr;
				ownHandle = true;
				// Subclass window
				this.Subclass();
			}
		}

		/// <summary>
		/// Releases the handle associated with this window. 
		/// </summary>
		public void ReleaseHandle()
		{
			
			if (this.handle == IntPtr.Zero)
			{
				return;
			}
			
			this.UnSubclass(false);

			this.handle = IntPtr.Zero;
		
			this.defWindowProc = IntPtr.Zero;
			this.windowProc = null;
			if (ownHandle)
			{
				NativeMethods.DestroyWindow(this.handle);
			}

			this.handle = IntPtr.Zero;		
		}

		/// <summary>
		/// Invokes the default window procedure associated with this window.   
		/// </summary>
        /// <param name="m">A <see cref="Message"/> that is associated with the current Windows message.</param>
        protected virtual void WndProc(ref Microsoft.WindowsCE.Forms.Message m)
		{
			this.DefWndProc(ref m);
		}
		
		/// <summary>
		/// Invokes the default window procedure associated with this window.
        /// It is an error to call this method when the <see cref="Handle"/> property is 0.  
		/// </summary>
		/// <param name="m">A <see cref="Message"/> that is associated with the current Windows message.</param>
        public void DefWndProc(ref Microsoft.WindowsCE.Forms.Message m)
		{
			if (this.defWindowProc == IntPtr.Zero)
			{
				m.Result = NativeMethods.DefWindowProc(m.HWnd, m.Msg, m.WParam, m.LParam);
			}
			else
			{
				m.Result = NativeMethods.CallWindowProc(this.defWindowProc, m.HWnd, (uint)m.Msg, m.WParam, m.LParam);
			}
		}

		/// <summary>
		/// Destroys the window and its handle.  
		/// </summary>
		public virtual void DestroyHandle() 
		{
			ReleaseHandle();
		}

        /// <summary>
        /// Specifies a notification method that is called when the handle for a window is changed.
        /// </summary>
		protected virtual void OnHandleChange()
		{
			
		}

		#endregion // methods

		/// <summary>
		/// Retrieves the window associated with the specified handle.  
		/// </summary>
		/// <param name="handle">A handle to a window.</param>
		/// <returns>The <see cref="NativeWindow"/> associated with the specified handle.
        /// This method returns null when the handle does not have an associated window.</returns>
		public static NativeWindow FromHandle(IntPtr handle) 
		{
			NativeWindow window = new NativeWindow();

			window.AssignHandle(handle);

			return window;
		}
		
		private IntPtr Callback(IntPtr hWnd, uint msg, IntPtr wparam, IntPtr lparam)
		{
            Microsoft.WindowsCE.Forms.Message message = Microsoft.WindowsCE.Forms.Message.Create(hWnd, (int)msg, wparam, lparam);
			this.WndProc(ref message);
			
			if (msg == 130)
			{
				this.ReleaseHandle();
			}
			return message.Result;
		}


		private void Subclass()
		{
			if (this.handle != IntPtr.Zero)
			{
				this.defWindowProc = NativeMethods.GetWindowLong(this.handle, NativeMethods.GWL.WNDPROC);
				windowProc = new WndProcDelegate(Callback);
                windowProcPtr = Marshal.GetFunctionPointerForDelegate(windowProc);
                NativeMethods.SetWindowLong(handle, NativeMethods.GWL.WNDPROC, windowProcPtr.ToInt32());
			}
		}

		private void UnSubclass(bool finalizing)
		{
            if (this.windowProcPtr != NativeMethods.GetWindowLong(this.handle, NativeMethods.GWL.WNDPROC))
			{
				return;
			}

			NativeMethods.SetWindowLong(handle, NativeMethods.GWL.WNDPROC, (int)this.windowProcPtr);
		}

        /// <summary>
        /// Releases the resources associated with this window.
        /// </summary>
		~NativeWindow() 
		{
			if (handle != IntPtr.Zero) 
			{
				ReleaseHandle();
			}
		}
	}
}
