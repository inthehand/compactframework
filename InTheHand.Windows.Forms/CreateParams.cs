// In The Hand - .NET Components for Mobility
//
// InTheHand.Windows.Forms.CreateParams
// 
// Copyright (c) 2008-2012 In The Hand Ltd, All rights reserved.

using System;

namespace InTheHand.Windows.Forms
{
	/// <summary>
	/// Encapsulates the information needed when creating a control.
	/// </summary>
    /// <remarks>For more information about creating control parameters, see the CreateWindow and CreateWindowEx functions and the CREATESTRUCT structure documentation in the Windows Platform SDK reference located in the MSDN Library.</remarks>
	public class CreateParams
	{
        #region Fields

        private string caption;
        private string className;
        private int classStyle;
        private int style;
        private int exStyle;
        private int height;
        private object param;
        private IntPtr parent;
        private int width;
        private int x;
        private int y;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateParams"/> class.
        /// </summary>
        public CreateParams()
        {
            param = null;
            parent = IntPtr.Zero;
            caption = String.Empty;
            className = String.Empty;
        }

        /// <summary>
        /// Gets or sets the control's initial text.
        /// </summary>
        /// <value>The control's initial text.</value>
        public string Caption
        {
            get
            {
                return caption;
            }

            set
            {
                caption = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the Windows class to derive the control from.
        /// </summary>
        /// <value>The name of the Windows class to derive the control from.</value>
        /// <remarks>The default value for this property is a null reference (Nothing in Visual Basic), indicating that the control is not derived from an existing control class. To derive from an existing control class, store the system class name in this property.
        /// For example, to derive from the standard <see cref="System.Windows.Forms.Button"/> control, set this property to "BUTTON".</remarks>
        public string ClassName
        {
            get
            {
                return className;
            }

            set
            {
                className = value;
            }
        }

        /// <summary>
        /// Gets or sets a bitwise combination of class style values.
        /// </summary>
        /// <remarks>The ClassStyle property is ignored if the <see cref="ClassName"/> property is not a null reference (Nothing in Visual Basic).
        /// For more information about creating control parameters, see the CreateWindow and CreateWindowEx functions and the CREATESTRUCT structure documentation in the Windows Platform SDK reference located in the MSDN Library.</remarks>
        public int ClassStyle
        {
            get
            {
                return classStyle;
            }

            set
            {
                classStyle = value;
            }
        }

        /// <summary>
        /// Gets or sets a bitwise combination of window style values.
        /// </summary>
        /// <remarks>The Style property controls the appearance of the control and its initial state.
        /// For more information about creating control parameters, see the CreateWindow and CreateWindowEx functions and the CREATESTRUCT structure documentation in the Windows Platform SDK reference located in the MSDN Library.</remarks>
        public int Style
        {
            get
            {
                return style;
            }

            set
            {
                style = value;
            }
        }


        /// <summary>
        /// Gets or sets additional parameter information needed to create the control.
        /// </summary>
        /// <value>The <see cref="Object"/> that holds additional parameter information needed to create the control.</value>
        public object Param
        {
            get
            {
                return param;
            }

            set
            {
                param = value;
            }
        }

        /// <summary>
        /// Gets or sets a bitwise combination of extended window style values.
        /// </summary>
        public int ExStyle
        {
            get
            {
                return exStyle;
            }

            set
            {
                exStyle = value;
            }
        }

        /// <summary>
        /// Gets or sets the initial height of the control.
        /// </summary>
        /// <value>The numeric value that represents the initial height of the control.</value>
        public int Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
            }
        }

        /// <summary>
        /// Gets or sets the control's parent.
        /// </summary>
        /// <value>An <see cref="IntPtr"/> that contains the window handle of the control's parent.</value>
        public IntPtr Parent
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
            }
        }

        /// <summary>
        /// Gets or sets the initial width of the control.
        /// </summary>
        /// <value>The numeric value that represents the initial width of the control.</value>
        public int Width
        {
            get
            {
                return width;
            }

            set
            {
                width = value;
            }
        }

        /// <summary>
        /// Gets or sets the initial left position of the control.
        /// </summary>
        public int X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        /// <summary>
        /// Gets or sets the initial top position of the control.
        /// </summary>
        public int Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }
	}
}
