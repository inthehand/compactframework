// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CopyPixelOperation.cs" company="In The Hand Ltd">
// Copyright (c) 2008-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Drawing;

namespace InTheHand.Drawing
{
    /// <summary>
    /// Determines how the source color in a copy pixel operation is combined with the destination color to result in a final color.
    /// </summary>
    /// <remarks>The <b>CopyPixelOperation</b> enumeration is used by the <see cref="GraphicsInTheHand.CopyFromScreen(Graphics,Point,Point,Size)"/> method.</remarks>
    public enum CopyPixelOperation
    {
        /// <summary>
        /// The destination area is filled by using the color associated with index 0 in the physical palette.
        /// (This color is black for the default physical palette.)
        /// </summary>
        Blackness = 0x42,

        /// <summary>
        /// Windows that are layered on top of your window are included in the resulting image.
        /// By default, the image contains only your window.
        /// Note that this generally cannot be used for printing device contexts.
        /// </summary>
        CaptureBlt = 0x40000000,

        /// <summary>
        /// The destination area is inverted.
        /// </summary>
        DestinationInvert = 0x550009,

        /// <summary>
        /// The colors of the source area are merged with the colors of the selected brush of the destination device context using the Boolean AND operator.
        /// </summary>
        MergeCopy = 0xc000ca,

        /// <summary>
        /// The colors of the inverted source area are merged with the colors of the destination area by using the Boolean OR operator.
        /// </summary>
        MergePaint = 0xbb0226,

        /// <summary>
        /// The bitmap is not mirrored.
        /// </summary>
        NoMirrorBitmap = -2147483648,

        /// <summary>
        /// The inverted source area is copied to the destination.
        /// </summary>
        NotSourceCopy = 0x330008,

        /// <summary>
        /// The source and destination colors are combined using the Boolean OR operator, and then resultant color is then inverted.
        /// </summary>
        NotSourceErase = 0x1100a6,

        /// <summary>
        /// The brush currently selected in the destination device context is copied to the destination bitmap.
        /// </summary>
        PatCopy = 0xf00021,

        /// <summary>
        /// The colors of the brush currently selected in the destination device context are combined with the colors of the destination are using the Boolean XOR operator.
        /// </summary>
        PatInvert = 0x5a0049,

        /// <summary>
        /// The colors of the brush currently selected in the destination device context are combined with the colors of the inverted source area using the Boolean OR operator.
        /// The result of this operation is combined with the colors of the destination area using the Boolean OR operator.
        /// </summary>
        PatPaint = 0xfb0a09,

        /// <summary>
        /// The colors of the source and destination areas are combined using the Boolean AND operator.
        /// </summary>
        SourceAnd = 0x8800c6,

        /// <summary>
        /// The source area is copied directly to the destination area.
        /// </summary>
        SourceCopy = 0xcc0020,

        /// <summary>
        /// The inverted colors of the destination area are combined with the colors of the source area using the Boolean AND operator.
        /// </summary>
        SourceErase = 0x440328,

        /// <summary>
        /// The colors of the source and destination areas are combined using the Boolean XOR operator.
        /// </summary>
        SourceInvert = 0x660046,

        /// <summary>
        /// The colors of the source and destination areas are combined using the Boolean OR operator.
        /// </summary>
        SourcePaint = 0xee0086,

        /// <summary>
        /// The destination area is filled by using the color associated with index 1 in the physical palette.
        /// (This color is white for the default physical palette.)
        /// </summary>
        Whiteness = 0xff0062
    }
}