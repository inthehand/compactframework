// In The Hand - .NET Components for Mobility
//
// InTheHand.Globalization.TextInfoInTheHand
// 
// Copyright (c) 2012-14 In The Hand Ltd, All rights reserved.

namespace InTheHand.Globalization
{
    using System;

    /// <summary>
    /// Helper class for TextInfo.
    /// </summary>
    public static class TextInfoInTheHand
    {
        /// <summary>
        /// Gets a value indicating whether the TextInfo object represents a writing system where text flows from right to left.
        /// </summary>
        /// <param name="textInfo">A TextInfo instance.</param>
        /// <returns>true if text flows from right to left; otherwise, false.</returns>
        public static bool IsRightToLeft(this System.Globalization.TextInfo textInfo)
        {
            if (textInfo == null)
            {
                throw new ArgumentNullException("textInfo");
            }

            switch (textInfo.CultureName)
            {
                case "ar-SA":
                case "fa-IR":
                case "he-IL":
                    return true;
                default:
                    return false;
            }
        }
    }
}