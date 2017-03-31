// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SYSTEMTIME.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Runtime.InteropServices
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size = 16)]
    internal struct SYSTEMTIME
    {
        internal short year;
        internal short month;
        internal short dayOfWeek;
        internal short day;
        internal short hour;
        internal short minute;
        internal short second;
        internal short millisecond;

        public static SYSTEMTIME FromDateTime(DateTime dt)
        {
            SYSTEMTIME st = new SYSTEMTIME();
            st.year = (short)dt.Year;
            st.month = (short)dt.Month;
            st.day = (short)dt.Day;
            st.hour = (short)dt.Hour;
            st.minute = (short)dt.Minute;
            st.second = (short)dt.Second;

            return st;
        }

        internal DateTime ToDateTime(DateTimeKind kind)
        {
            if (year == 0 && month == 0 && day == 0 && hour == 0 && minute == 0 && second == 0)
            {
                return DateTime.MinValue;
            }

            return new DateTime(year, month, day, hour, minute, second, kind);
        }
    }
}
