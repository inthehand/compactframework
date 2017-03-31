// In The Hand - .NET Components for Mobility
//
// InTheHand.DateTimeOffset
// 
// Copyright (c) 2010-2014 In The Hand Ltd, All rights reserved.

using System;
using System.Globalization;

namespace InTheHand
{
    /// <summary>
    /// Represents a point in time, typically expressed as a date and time of day, relative to Coordinated Universal Time (UTC).
    /// </summary>
    [Serializable]
    public struct DateTimeOffset : IComparable, /*IFormattable,*/ IComparable<DateTimeOffset>, IEquatable<DateTimeOffset>
    {
        private DateTime m_dateTime;
        private short m_offsetMinutes;

        internal const long MaxOffset = 0x7558bdb000L;
        internal const long MinOffset = -504000000000L;

        /// <summary>
        /// Represents the earliest possible DateTimeOffset value.
        /// </summary>
        public static readonly DateTimeOffset MinValue = new DateTimeOffset(0L, TimeSpan.Zero);
        /// <summary>
        /// Represents the greatest possible value of DateTimeOffset.
        /// </summary>
        public static readonly DateTimeOffset MaxValue = new DateTimeOffset(0x2bca2875f4373fffL, TimeSpan.Zero);
        
        /// <summary>
        /// Initializes a new instance of the DateTimeOffset structure using the specified <see cref="DateTime"/> value.
        /// </summary>
        /// <param name="dateTime"></param>
        public DateTimeOffset(DateTime dateTime)
        {
            TimeSpan utcOffset;
            if (dateTime.Kind != DateTimeKind.Utc)
            {
                utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);
            }
            else
            {
                utcOffset = new TimeSpan(0L);
            }
            this.m_offsetMinutes = ValidateOffset(utcOffset);
            this.m_dateTime = ValidateDate(dateTime, utcOffset);
        }

        /// <summary>
        /// Initializes a new instance of the DateTimeOffset structure using the specified DateTime value and offset.
        /// </summary>
        /// <param name="dateTime">A date and time.</param>
        /// <param name="offset">The time's offset from Coordinated Universal Time (UTC).</param>
        public DateTimeOffset(DateTime dateTime, TimeSpan offset)
        {
            if (dateTime.Kind == DateTimeKind.Local)
            {
                if (offset != TimeZone.CurrentTimeZone.GetUtcOffset(dateTime))
                {
                    throw new ArgumentException(Properties.Resources.Argument_OffsetLocalMismatch, "offset");
                }
            }
            else if ((dateTime.Kind == DateTimeKind.Utc) && (offset != TimeSpan.Zero))
            {
                throw new ArgumentException(Properties.Resources.Argument_OffsetUtcMismatch, "offset");
            }
            this.m_offsetMinutes = ValidateOffset(offset);
            this.m_dateTime = ValidateDate(dateTime, offset);
        }

        /// <summary>
        /// Initializes a new instance of the DateTimeOffset structure using the specified number of ticks and offset.
        /// </summary>
        /// <param name="ticks">A date and time expressed as the number of 100-nanosecond intervals that have elapsed since 12:00:00 midnight on January 1, 0001.</param>
        /// <param name="offset">The time's offset from Coordinated Universal Time (UTC).</param>
        public DateTimeOffset(long ticks, TimeSpan offset)
        {
            this.m_offsetMinutes = ValidateOffset(offset);
            DateTime dateTime = new DateTime(ticks);
            this.m_dateTime = ValidateDate(dateTime, offset);
        }

        /// <summary>
        /// Initializes a new instance of the DateTimeOffset structure using the specified year, month, day, hour, minute, second and offset.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <param name="offset">The time's offset from Coordinated Universal Time (UTC).</param>    
        public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, TimeSpan offset)
        {
            this.m_offsetMinutes = ValidateOffset(offset);
            this.m_dateTime = ValidateDate(new DateTime(year, month, day, hour, minute, second), offset);
        }

        /// <summary>
        /// Initializes a new instance of the DateTimeOffset structure using the specified year, month, day, hour, minute, second, millisecond, and offset.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <param name="millisecond">The milliseconds (0 through 999</param>
        /// <param name="offset">The time's offset from Coordinated Universal Time (UTC).</param>    
        public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, int millisecond, TimeSpan offset)
        {
            this.m_offsetMinutes = ValidateOffset(offset);
            this.m_dateTime = ValidateDate(new DateTime(year, month, day, hour, minute, second, millisecond), offset);
        }

        /// <summary>
        /// Initializes a new instance of the DateTimeOffset structure using the specified year, month, day, hour, minute, second, millisecond, and offset of a specified calendar.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <param name="millisecond">The milliseconds (0 through 999</param>
        /// <param name="calendar">The calendar whose time is defined.</param>
        /// <param name="offset">The time's offset from Coordinated Universal Time (UTC).</param>
        public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar, TimeSpan offset)
        {
            this.m_offsetMinutes = ValidateOffset(offset);
            this.m_dateTime = ValidateDate(new DateTime(year, month, day, hour, minute, second, millisecond, calendar), offset);
        }

        /// <summary>
        /// Adds a specified time interval to a DateTimeOffset object.
        /// </summary>
        /// <param name="timeSpan">A TimeSpan object that represents a positive or a negative time interval.</param>
        /// <returns>A DateTimeOffset object whose value is the sum of the date and time represented by the current DateTimeOffset object and the time interval represented by timeSpan.</returns>
        public DateTimeOffset Add(TimeSpan timeSpan)
        {
            return new DateTimeOffset(this.ClockDateTime.Add(timeSpan), this.Offset);
        }

        /// <summary>
        /// Adds a specified number of whole and fractional days to the current DateTimeOffset object.
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public DateTimeOffset AddDays(double days)
        {
            return new DateTimeOffset(this.ClockDateTime.AddDays(days), this.Offset);
        }

        /// <summary>
        /// Adds a specified number of whole and fractional hours to the current DateTimeOffset object.
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        public DateTimeOffset AddHours(double hours)
        {
            return new DateTimeOffset(this.ClockDateTime.AddHours(hours), this.Offset);
        }

        /// <summary>
        /// Adds a specified number of milliseconds to the current DateTimeOffset object.
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public DateTimeOffset AddMilliseconds(double milliseconds)
        {
            return new DateTimeOffset(this.ClockDateTime.AddMilliseconds(milliseconds), this.Offset);
        }

        /// <summary>
        /// Adds a specified number of whole and fractional minutes to the current DateTimeOffset object.
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public DateTimeOffset AddMinutes(double minutes)
        {
            return new DateTimeOffset(this.ClockDateTime.AddMinutes(minutes), this.Offset);
        }

        /// <summary>
        /// Adds a specified number of months to the current DateTimeOffset object.
        /// </summary>
        /// <param name="months"></param>
        /// <returns></returns>
        public DateTimeOffset AddMonths(int months)
        {
            return new DateTimeOffset(this.ClockDateTime.AddMonths(months), this.Offset);
        }

        /// <summary>
        /// Adds a specified number of whole and fractional seconds to the current DateTimeOffset object.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public DateTimeOffset AddSeconds(double seconds)
        {
            return new DateTimeOffset(this.ClockDateTime.AddSeconds(seconds), this.Offset);
        }

        /// <summary>
        /// Adds a specified number of whole and fractional seconds to the current DateTimeOffset object.
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public DateTimeOffset AddTicks(long ticks)
        {
            return new DateTimeOffset(this.ClockDateTime.AddTicks(ticks), this.Offset);
        }

        /// <summary>
        /// Adds a specified number of years to the DateTimeOffset object.
        /// </summary>
        /// <param name="years"></param>
        /// <returns></returns>
        public DateTimeOffset AddYears(int years)
        {
            return new DateTimeOffset(this.ClockDateTime.AddYears(years), this.Offset);
        }

        /// <summary>
        /// Compares two DateTimeOffset objects and indicates whether the first is earlier than the second, equal to the second, or later than the second.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static int Compare(DateTimeOffset first, DateTimeOffset second)
        {
            return DateTime.Compare(first.UtcDateTime, second.UtcDateTime);
        }

        /// <summary>
        /// Compares the current DateTimeOffset object to a specified DateTimeOffset object and indicates whether the current object is earlier than, the same as, or later than the second DateTimeOffset object.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(DateTimeOffset other)
        {
            DateTime utcDateTime = other.UtcDateTime;
            DateTime time2 = this.UtcDateTime;
            if (time2 > utcDateTime)
            {
                return 1;
            }
            if (time2 < utcDateTime)
            {
                return -1;
            }
            return 0;
        }

        /// <summary>
        /// Determines whether the current DateTimeOffset object represents the same point in time as a specified DateTimeOffset object.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(DateTimeOffset other)
        {
            return this.UtcDateTime.Equals(other.UtcDateTime);
        }

        /// <summary>
        /// Determines whether a DateTimeOffset object represents the same point in time as a specified object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is DateTimeOffset)
            {
                DateTimeOffset offset = (DateTimeOffset)obj;
                return this.UtcDateTime.Equals(offset.UtcDateTime);
            }
            return false;
        }

        /// <summary>
        /// Determines whether two specified DateTimeOffset objects represent the same point in time.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool Equals(DateTimeOffset first, DateTimeOffset second)
        {
            return DateTime.Equals(first.UtcDateTime, second.UtcDateTime);
        }

        /// <summary>
        /// Determines whether the current DateTimeOffset object represents the same time and has the same offset as a specified DateTimeOffset object.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool EqualsExact(DateTimeOffset other)
        {
            return (((this.ClockDateTime == other.ClockDateTime) && (this.Offset == other.Offset)) && (this.ClockDateTime.Kind == other.ClockDateTime.Kind));
        }

        /// <summary>
        /// Converts the specified Windows file time to an equivalent local time.
        /// </summary>
        /// <param name="fileTime"></param>
        /// <returns></returns>
        public static DateTimeOffset FromFileTime(long fileTime)
        {
            return new DateTimeOffset(DateTime.FromFileTime(fileTime));
        }

        /// <summary>
        /// Returns the hash code for the current DateTimeOffset object.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.UtcDateTime.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTimeTz"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static DateTimeOffset operator +(DateTimeOffset dateTimeTz, TimeSpan timeSpan)
        {
            return new DateTimeOffset(dateTimeTz.ClockDateTime + timeSpan, dateTimeTz.Offset);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(DateTimeOffset left, DateTimeOffset right)
        {
            return (left.UtcDateTime == right.UtcDateTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >(DateTimeOffset left, DateTimeOffset right)
        {
            return (left.UtcDateTime > right.UtcDateTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >=(DateTimeOffset left, DateTimeOffset right)
        {
            return (left.UtcDateTime >= right.UtcDateTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static implicit operator DateTimeOffset(DateTime dateTime)
        {
            return new DateTimeOffset(dateTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(DateTimeOffset left, DateTimeOffset right)
        {
            return (left.UtcDateTime != right.UtcDateTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <(DateTimeOffset left, DateTimeOffset right)
        {
            return (left.UtcDateTime < right.UtcDateTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <=(DateTimeOffset left, DateTimeOffset right)
        {
            return (left.UtcDateTime <= right.UtcDateTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static TimeSpan operator -(DateTimeOffset left, DateTimeOffset right)
        {
            return (TimeSpan)(left.UtcDateTime - right.UtcDateTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTimeTz"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static DateTimeOffset operator -(DateTimeOffset dateTimeTz, TimeSpan timeSpan)
        {
            return new DateTimeOffset(dateTimeTz.ClockDateTime - timeSpan, dateTimeTz.Offset);
        }

        /// <summary>
        /// Converts the specified string representation of a date and time to its DateTimeOffset equivalent.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTimeOffset Parse(string input)
        {
            //TimeSpan span;
            DateTime dt = DateTime.Parse(input, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None);
            TimeSpan span = dt.Subtract(dt.ToUniversalTime());
            return new DateTimeOffset(dt.Ticks, span);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static DateTimeOffset Parse(string input, IFormatProvider formatProvider)
        {
            return Parse(input, formatProvider, DateTimeStyles.None);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="formatProvider"></param>
        /// <param name="styles"></param>
        /// <returns></returns>
        public static DateTimeOffset Parse(string input, IFormatProvider formatProvider, DateTimeStyles styles)
        {
            styles = ValidateStyles(styles, "styles");
            DateTime dt = DateTime.Parse(input, DateTimeFormatInfo.GetInstance(formatProvider), styles);
            TimeSpan span = dt.Subtract(dt.ToUniversalTime());
            return new DateTimeOffset(dt.Ticks, span);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static DateTimeOffset ParseExact(string input, string format, IFormatProvider formatProvider)
        {
            return ParseExact(input, format, formatProvider, DateTimeStyles.None);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <param name="styles"></param>
        /// <returns></returns>
        public static DateTimeOffset ParseExact(string input, string format, IFormatProvider formatProvider, DateTimeStyles styles)
        {
            styles = ValidateStyles(styles, "styles");
            DateTime dt = DateTime.ParseExact(input, format, DateTimeFormatInfo.GetInstance(formatProvider), styles);
            TimeSpan span = dt.Subtract(dt.ToUniversalTime());
            return new DateTimeOffset(dt.Ticks, span);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="formats"></param>
        /// <param name="formatProvider"></param>
        /// <param name="styles"></param>
        /// <returns></returns>
        public static DateTimeOffset ParseExact(string input, string[] formats, IFormatProvider formatProvider, DateTimeStyles styles)
        {
            styles = ValidateStyles(styles, "styles");
            DateTime dt = DateTime.ParseExact(input, formats, DateTimeFormatInfo.GetInstance(formatProvider), styles);
            TimeSpan span = dt.Subtract(dt.ToUniversalTime());
            return new DateTimeOffset(dt.Ticks, span);
        }

        /// <summary>
        /// Subtracts a DateTimeOffset value that represents a specific date and time from the current DateTimeOffset object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public TimeSpan Subtract(DateTimeOffset value)
        {
            return this.UtcDateTime.Subtract(value.UtcDateTime);
        }

        /// <summary>
        /// Subtracts a specified time interval from the current DateTimeOffset object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public DateTimeOffset Subtract(TimeSpan value)
        {
            return new DateTimeOffset(this.ClockDateTime.Subtract(value), this.Offset);
        }

        /// <summary>
        /// Compares the value of the current DateTimeOffset object with another object of the same type.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        int IComparable.CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }
            if (!(obj is DateTimeOffset))
            {
                throw new ArgumentException(Properties.Resources.Arg_MustBeDateTimeOffset);
            }
            DateTimeOffset offset = (DateTimeOffset)obj;
            DateTime utcDateTime = offset.UtcDateTime;
            DateTime time2 = this.UtcDateTime;
            if (time2 > utcDateTime)
            {
                return 1;
            }
            if (time2 < utcDateTime)
            {
                return -1;
            }
            return 0;
        }

        /// <summary>
        /// Converts the value of the current DateTimeOffset object to a Windows file time.
        /// </summary>
        /// <returns></returns>
        public long ToFileTime()
        {
            return this.UtcDateTime.ToFileTime();
        }

        /// <summary>
        /// Converts the current DateTimeOffset object to a DateTimeOffset object that represents the local time.
        /// </summary>
        /// <returns></returns>
        public DateTimeOffset ToLocalTime()
        {
            return new DateTimeOffset(this.UtcDateTime.ToLocalTime());
        }

        /// <summary>
        /// Converts the value of the current DateTimeOffset object to the date and time specified by an offset value.
        /// </summary>
        /// <param name="offset">The offset to convert the DateTimeOffset value to.</param>
        /// <returns>A DateTimeOffset object that is equal to the original DateTimeOffset object (that is, their ToUniversalTime methods return identical points in time) but whose Offset property is set to offset.</returns>
        public DateTimeOffset ToOffset(TimeSpan offset)
        {
            DateTime time = this.m_dateTime + offset;
            return new DateTimeOffset(time.Ticks, offset);
        }

        /// <summary>
        /// Converts the value of the current DateTimeOffset object to its equivalent string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            TimeSpan offset = new TimeSpan(0, m_offsetMinutes, 0);
            return m_dateTime.ToString() + " " + (offset.Ticks < 0 ? "-" : "+") + Convert.ToInt32(offset.Hours).ToString("d2") + ":" + Convert.ToInt32(offset.Minutes).ToString("d2");
        }

        /*public string ToString(IFormatProvider formatProvider)
        {
            return DateTimeFormat.Format(this.ClockDateTime, null, DateTimeFormatInfo.GetInstance(formatProvider), this.Offset);
        }

        public string ToString(string format)
        {
            return DateTimeFormat.Format(this.ClockDateTime, format, DateTimeFormatInfo.CurrentInfo, this.Offset);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return DateTimeFormat.Format(this.ClockDateTime, format, DateTimeFormatInfo.GetInstance(formatProvider), this.Offset);
        }*/

        /// <summary>
        /// Converts the current DateTimeOffset object to a DateTimeOffset value that represents the Coordinated Universal Time (UTC).
        /// </summary>
        /// <returns>A DateTimeOffset object that represents the date and time of the current DateTimeOffset object converted to Coordinated Universal Time (UTC).</returns>
        public DateTimeOffset ToUniversalTime()
        {
            return new DateTimeOffset(this.UtcDateTime);
        }

        /*public static bool TryParse(string input, out DateTimeOffset result)
        {
            TimeSpan span;
            DateTime time;
            bool flag = DateTimeParse.TryParse(input, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out time, out span);
            result = new DateTimeOffset(time.Ticks, span);
            return flag;
        }

        public static bool TryParse(string input, IFormatProvider formatProvider, DateTimeStyles styles, out DateTimeOffset result)
        {
            TimeSpan span;
            DateTime time;
            styles = ValidateStyles(styles, "styles");
            bool flag = DateTimeParse.TryParse(input, DateTimeFormatInfo.GetInstance(formatProvider), styles, out time, out span);
            result = new DateTimeOffset(time.Ticks, span);
            return flag;
        }

        public static bool TryParseExact(string input, string[] formats, IFormatProvider formatProvider, DateTimeStyles styles, out DateTimeOffset result)
        {
            TimeSpan span;
            DateTime time;
            styles = ValidateStyles(styles, "styles");
            bool flag = DateTimeParse.TryParseExactMultiple(input, formats, DateTimeFormatInfo.GetInstance(formatProvider), styles, out time, out span);
            result = new DateTimeOffset(time.Ticks, span);
            return flag;
        }

        public static bool TryParseExact(string input, string format, IFormatProvider formatProvider, DateTimeStyles styles, out DateTimeOffset result)
        {
            TimeSpan span;
            DateTime time;
            styles = ValidateStyles(styles, "styles");
            bool flag = DateTimeParse.TryParseExact(input, format, DateTimeFormatInfo.GetInstance(formatProvider), styles, out time, out span);
            result = new DateTimeOffset(time.Ticks, span);
            return flag;
        }*/

        private static DateTime ValidateDate(DateTime dateTime, TimeSpan offset)
        {
            long ticks = dateTime.Ticks - offset.Ticks;
            if ((ticks < 0L) || (ticks > 0x2bca2875f4373fffL))
            {
                throw new ArgumentOutOfRangeException("offset", Properties.Resources.Argument_UTCOutOfRange);
            }
            return new DateTime(ticks, DateTimeKind.Unspecified);
        }

        private static short ValidateOffset(TimeSpan offset)
        {
            long ticks = offset.Ticks;
            if ((ticks % 0x23c34600L) != 0L)
            {
                throw new ArgumentException(Properties.Resources.Argument_OffsetPrecision, "offset");
            }
            if ((ticks < MinOffset) || (ticks > MaxOffset))
            {
                throw new ArgumentOutOfRangeException("offset", Properties.Resources.Argument_OffsetOutOfRange);
            }
            return (short)(offset.Ticks / 0x23c34600L);
        }

        private static DateTimeStyles ValidateStyles(DateTimeStyles style, string parameterName)
        {
            if ((style & ~(DateTimeStyles.RoundtripKind | DateTimeStyles.AssumeUniversal | DateTimeStyles.AssumeLocal | DateTimeStyles.AdjustToUniversal | DateTimeStyles.NoCurrentDateDefault | DateTimeStyles.AllowWhiteSpaces)) != DateTimeStyles.None)
            {
                throw new ArgumentException(Properties.Resources.Argument_InvalidDateTimeStyles, parameterName);
            }
            if (((style & DateTimeStyles.AssumeLocal) != DateTimeStyles.None) && ((style & DateTimeStyles.AssumeUniversal) != DateTimeStyles.None))
            {
                throw new ArgumentException(Properties.Resources.Argument_ConflictingDateTimeStyles, parameterName);
            }
            if ((style & DateTimeStyles.NoCurrentDateDefault) != DateTimeStyles.None)
            {
                throw new ArgumentException(Properties.Resources.Argument_DateTimeOffsetInvalidDateTimeStyles, parameterName);
            }
            style &= ~DateTimeStyles.RoundtripKind;
            style &= ~DateTimeStyles.AssumeLocal;
            return style;
        }

        private DateTime ClockDateTime
        {
            get
            {
                DateTime time = this.m_dateTime + this.Offset;
                return new DateTime(time.Ticks, DateTimeKind.Unspecified);
            }
        }

        /// <summary>
        /// Gets a DateTime value that represents the date component of the current DateTimeOffset object.
        /// </summary>
        public DateTime Date
        {
            get
            {
                return this.ClockDateTime.Date;
            }
        }

        /// <summary>
        /// Gets a DateTime value that represents the date and time of the current DateTimeOffset object.
        /// </summary>
        public DateTime DateTime
        {
            get
            {
                return this.ClockDateTime;
            }
        }

        /// <summary>
        /// Gets the day of the month represented by the current DateTimeOffset object.
        /// </summary>
        public int Day
        {
            get
            {
                return this.ClockDateTime.Day;
            }
        }

        /// <summary>
        /// Gets the day of the week represented by the current DateTimeOffset object.
        /// </summary>
        public DayOfWeek DayOfWeek
        {
            get
            {
                return this.ClockDateTime.DayOfWeek;
            }
        }

        /// <summary>
        /// Gets the day of the year represented by the current DateTimeOffset object.
        /// </summary>
        public int DayOfYear
        {
            get
            {
                return this.ClockDateTime.DayOfYear;
            }
        }

        /// <summary>
        /// Gets the hour component of the time represented by the current DateTimeOffset object.
        /// </summary>
        public int Hour
        {
            get
            {
                return this.ClockDateTime.Hour;
            }
        }

        /// <summary>
        /// Gets a DateTime value that represents the local date and time of the current DateTimeOffset object.
        /// </summary>
        public DateTime LocalDateTime
        {
            get
            {
                return this.UtcDateTime.ToLocalTime();
            }
        }

        /// <summary>
        /// Gets the millisecond component of the time represented by the current DateTimeOffset object.
        /// </summary>
        public int Millisecond
        {
            get
            {
                return this.ClockDateTime.Millisecond;
            }
        }

        /// <summary>
        /// Gets the minute component of the time represented by the current DateTimeOffset object.
        /// </summary>
        public int Minute
        {
            get
            {
                return this.ClockDateTime.Minute;
            }
        }

        /// <summary>
        /// Gets the month component of the date represented by the current DateTimeOffset object.
        /// </summary>
        public int Month
        {
            get
            {
                return this.ClockDateTime.Month;
            }
        }

        /// <summary>
        /// Gets a DateTimeOffset object that is set to the current date and time on the current computer, with the offset set to the local time's offset from Coordinated Universal Time (UTC).
        /// </summary>
        public static DateTimeOffset Now
        {
            get
            {
                return new DateTimeOffset(DateTime.Now);
            }
        }

        /// <summary>
        /// Gets the time's offset from Coordinated Universal Time (UTC). 
        /// </summary>
        public TimeSpan Offset
        {
            get
            {
                return new TimeSpan(0, this.m_offsetMinutes, 0);
            }
        }

        /// <summary>
        /// Gets the second component of the clock time represented by the current DateTimeOffset object.
        /// </summary>
        public int Second
        {
            get
            {
                return this.ClockDateTime.Second;
            }
        }

        /// <summary>
        /// Gets the number of ticks that represents the date and time of the current DateTimeOffset object in clock time.
        /// </summary>
        public long Ticks
        {
            get
            {
                return this.ClockDateTime.Ticks;
            }
        }

        /// <summary>
        /// Gets the time of day for the current DateTimeOffset object.
        /// </summary>
        public TimeSpan TimeOfDay
        {
            get
            {
                return this.ClockDateTime.TimeOfDay;
            }
        }

        /// <summary>
        /// Gets a DateTime value that represents the Coordinated Universal Time (UTC) date and time of the current DateTimeOffset object.
        /// </summary>
        public DateTime UtcDateTime
        {
            get
            {
                return DateTime.SpecifyKind(this.m_dateTime, DateTimeKind.Utc);
            }
        }

        /// <summary>
        /// Gets a DateTimeOffset object whose date and time are set to the current Coordinated Universal Time (UTC) date and time and whose offset is TimeSpan.Zero.
        /// </summary>
        public static DateTimeOffset UtcNow
        {
            get
            {
                return new DateTimeOffset(DateTime.UtcNow);
            }
        }

        /// <summary>
        /// Gets the number of ticks that represents the date and time of the current DateTimeOffset object in Coordinated Universal Time (UTC).
        /// </summary>
        public long UtcTicks
        {
            get
            {
                return this.UtcDateTime.Ticks;
            }
        }

        /// <summary>
        /// Gets the year component of the date represented by the current DateTimeOffset object.
        /// </summary>
        public int Year
        {
            get
            {
                return this.ClockDateTime.Year;
            }
        }
    }


}