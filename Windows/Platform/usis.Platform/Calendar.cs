//
//  @(#) Calendar.cs
//
//  Project:    usis.Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2008-2018 usis GmbH. All rights reserved.

using System;

namespace usis.Platform
{
    #region Calendar class

    //  --------------
    //  Calendar class
    //  --------------

    /// <summary>
    /// Provides methods for calendar calculations.
    /// </summary>

    public static class Calendar
    {
        //  -------------
        //  Easter method
        //  -------------

        /// <summary>
        /// Returns the date of easter sunday for the specified year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>
        /// The date of easter sunday for the specified year.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="year" /> is less than 1 or greater than 9999.</exception>

        //  Schritt Bedeutung                                               Formel
        //
        //  1.      die Säkularzahl                                         K(X) = X div 100
        //  2.      die säkulare Mondschaltung                              M(K) = 15 + (3K + 3) div 4 − (8K + 13) div 25
        //  3.      die säkulare Sonnenschaltung                            S(K) = 2 − (3K + 3) div 4
        //  4.      den Mondparameter                                       A(X) = X mod 19
        //  5.      den Keim für den ersten Vollmond im Frühling            D(A, M) = (19A + M) mod 30
        //  6.      die kalendarische Korrekturgröße                        R(D, A) = (D + A div 11) div 29
        //  7.      die Ostergrenze                                         OG(D, R) = 21 + D − R
        //  8.      den ersten Sonntag im März                              SZ(X, S) = 7 − (X + X div 4 + S) mod 7
        //  9.      die Entfernung des Ostersonntags von der Ostergrenze
        //          (Osterentfernung in Tagen)                              OE(OG, SZ) = 7 − (OG − SZ) mod 7
        //  10.     das Datum des Ostersonntags als Märzdatum
        //          (32. März = 1.April usw.)                               OS = OG + OE

        public static DateTime Easter(int year)
        {
            bool gregorian = year >= 1583;

            int k = year / 100;
            int m = gregorian ? 15 + (3 * k + 3) / 4 - (8 * k + 13) / 25 : 15;
            int s = gregorian ? 2 - (3 * k + 3) / 4 : 0;
            int a = year % 19;
            int d = (19 * a + m) % 30;
            int r = (d + a / 11) / 29;

            int og = 21 + d - r;
            int sz = 7 - (year + year / 4 + s) % 7;
            int oe = 7 - (og - sz) % 7;
            int os = og + oe;

            DateTime dt = gregorian ? new DateTime(year, 3, 1) : new DateTime(year, 3, 1, new System.Globalization.JulianCalendar());

            return dt.AddDays(os - 1);
        }

        //  -------------
        //  Advent method
        //  -------------

        /// <summary>
        /// Returns the date of the first Sunday of Advent for the specified year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>
        /// The date of the first Sunday of Advent for the specified year.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="year" /> is less than 1 or greater than 9999.</exception>

        public static DateTime Advent(int year)
        {
            var christmas = new DateTime(year, 12, 25);
            var dayOfWeek = christmas.DayOfWeek;
            var offset = -21 - (dayOfWeek == DayOfWeek.Sunday ? 7 : (int)christmas.DayOfWeek);

            return christmas.AddDays(offset);
        }
    }

    #endregion Calendar class

    #region HolidayType enumeration

    //  -----------------------
    //  HolidayType enumeration
    //  -----------------------

    /// <summary>
    /// Indicates the type of a holiday definition.
    /// </summary>

    public enum HolidayType
    {
        /// <summary>
        /// The holiday is defined by a fixed date in the Gregorian calendar.
        /// </summary>

        GregorianFixed,

        /// <summary>
        /// The holiday is defined relative to the date of Easter Sunday.
        /// </summary>

        EasterRelative,

        /// <summary>
        /// The holiday is defined relative to the date of the first Sunday of Advent.
        /// </summary>

        AdventRelative,

        /// <summary>
        /// The Holiday is defined with the position of a weekday in a month.
        /// </summary>

        WeekdayInMonth
    }

    #endregion HolidayType enumeration

    #region HolidayDefinition class

    //  -----------------------
    //  HolidayDefinition class
    //  -----------------------

    /// <summary>
    /// Provides a base class to define holidays.
    /// </summary>

    public abstract class HolidayDefinition
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="HolidayDefinition"/> class
        /// with the specified type and name.
        /// </summary>
        /// <param name="holidayType">The type of the holiday definition.</param>
        /// <param name="name">The name of the holiday.</param>

        protected HolidayDefinition(HolidayType holidayType, string name) { HolidayType = holidayType; Name = name; }

        #endregion construction

        #region properties

        //  --------------------
        //  HolidayType property
        //  --------------------

        /// <summary>
        /// Gets the type of the holiday definition.
        /// </summary>
        /// <value>
        /// The type of the holiday definition.
        /// </value>

        public HolidayType HolidayType { get; }

        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets the name of the Holiday.
        /// </summary>
        /// <value>
        /// The name of the Holiday.
        /// </value>

        public string Name { get; }

        #endregion properties

        #region methods

        //  -----------------
        //  DateInYear method
        //  -----------------

        /// <summary>
        /// Calculates the date of the Holiday in the specified year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>
        /// The date of the Holiday in the specified year
        /// or <c>null</c> if the Holiday does not take place this year.
        /// </returns>

        public abstract DateTime? DateInYear(int year);

        #endregion methods
    }

    #endregion HolidayDefinition class

    #region GregorianFixedHoliday class

    //  ---------------------------
    //  GregorianFixedHoliday class
    //  ---------------------------

    /// <summary>
    /// Represents the definition of a holiday that has a fixed date in the Gregorian calendar.
    /// </summary>
    public class GregorianFixedHoliday : HolidayDefinition
    {
        #region properties

        //  ------------
        //  Day property
        //  ------------

        /// <summary>
        /// Gets the day in month of the Holiday.
        /// </summary>
        /// <value>
        /// The day in month of the Holiday.
        /// </value>

        public int Day { get; }

        //  --------------
        //  Month property
        //  --------------

        /// <summary>
        /// Gets the month of the Holiday.
        /// </summary>
        /// <value>
        /// The month of the Holiday.
        /// </value>

        public int Month { get; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="GregorianFixedHoliday"/> class.
        /// </summary>
        /// <param name="name">The name of the Holiday.</param>
        /// <param name="month">The month of the Holiday.</param>
        /// <param name="day">The day of the Holiday.</param>

        public GregorianFixedHoliday(string name, int month, int day) : base(HolidayType.GregorianFixed, name)
        {
            Month = month; Day = day;
        }

        #endregion construction

        #region methods

        //  -----------------
        //  DateInYear method
        //  -----------------

        /// <summary>
        /// Calculates the date of the Holiday in the specified year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>
        /// The date of the Holiday in the specified year
        /// or <c>null</c> if the Holiday does not take place this year.
        /// </returns>

        public override DateTime? DateInYear(int year)
        {
            return new DateTime(year, Month, Day);
        }

        #endregion methods
    }

    #endregion GregorianFixedHoliday class

    #region EasterRelativeHoliday class

    //  ---------------------------
    //  EasterRelativeHoliday class
    //  ---------------------------

    /// <summary>
    /// Represents the definition of a holiday relative to Easter Sunday.
    /// </summary>

    public class EasterRelativeHoliday : HolidayDefinition
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="EasterRelativeHoliday"/> class
        /// with the specified name and offset.
        /// </summary>
        /// <param name="name">The name of the Holiday.</param>
        /// <param name="offset">The offset of the Holiday relative to Easter Sunday.</param>

        public EasterRelativeHoliday(string name, int offset) : base(HolidayType.EasterRelative, name) { Offset = offset; }

        #endregion construction

        #region properties

        //  ---------------
        //  Offset property
        //  ---------------

        /// <summary>
        /// Gets the offset of the Holiday relative to Easter Sunday.
        /// </summary>
        /// <value>
        /// The offset of the Holiday relative to Easter Sunday.
        /// </value>

        public int Offset { get; }

        #endregion properties

        #region methods

        //  -----------------
        //  DateInYear method
        //  -----------------

        /// <summary>
        /// Calculates the date of the Holiday in the specified year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>
        /// The date of the Holiday in the specified year
        /// or <c>null</c> if the Holiday does not take place this year.
        /// </returns>

        public override DateTime? DateInYear(int year)
        {
            return Calendar.Easter(year).AddDays(Offset);
        }

        #endregion methods
    }

    #endregion EasterRelativeHoliday class

    #region AdventRelativeHoliday class

    //  ---------------------------
    //  AdventRelativeHoliday class
    //  ---------------------------

    /// <summary>
    /// Represents the definition of a holiday relative to the first Sunday of Advent.
    /// </summary>

    public class AdventRelativeHoliday : HolidayDefinition
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="AdventRelativeHoliday"/> class.
        /// with the specified name and offset.
        /// </summary>
        /// <param name="name">The name of the Holiday.</param>
        /// <param name="offset">The offset of the Holiday relative to the first Sunday of Advent.</param>

        public AdventRelativeHoliday(string name, int offset) : base(HolidayType.AdventRelative, name) { Offset = offset; }

        #endregion construction

        #region properties

        //  ---------------
        //  Offset property
        //  ---------------

        /// <summary>
        /// Gets the offset of the Holiday relative to the first Sunday of Advent.
        /// </summary>
        /// <value>
        /// The offset of the Holiday relative to the first Sunday of Advent.
        /// </value>

        public int Offset { get; }

        #endregion properties

        #region methods

        //  -----------------
        //  DateInYear method
        //  -----------------

        /// <summary>
        /// Calculates the date of the Holiday in the specified year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>
        /// The date of the Holiday in the specified year
        /// or <c>null</c> if the Holiday does not take place this year.
        /// </returns>

        public override DateTime? DateInYear(int year)
        {
            return Calendar.Advent(year).AddDays(Offset);
        }

        #endregion methods
    }

    #endregion AdventRelativeHoliday class

    #region WeekdayInMonthHoliday class

    //  ---------------------------
    //  WeekdayInMonthHoliday class
    //  ---------------------------

    /// <summary>
    /// Represents the definition of a Holiday with the position of a weekday in a month.
    /// </summary>

    public class WeekdayInMonthHoliday : HolidayDefinition
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="WeekdayInMonthHoliday"/> class.
        /// </summary>
        /// <param name="name">The name of the Holiday.</param>
        /// <param name="month">The month of the Holiday.</param>
        /// <param name="dayOfWeek">The weekday of the Holiday.</param>
        /// <param name="ordinal">The ordinal number of weekday units.</param>
        /// <param name="offset">The offset of the Holiday relative to the ordinal weekday date.</param>

        public WeekdayInMonthHoliday(string name, int month, DayOfWeek dayOfWeek, int ordinal, int offset) : base(HolidayType.WeekdayInMonth, name)
        {
            Month = month; DayOfWeek = dayOfWeek; Ordinal = ordinal; Offset = offset;
        }

        #endregion construction

        #region properties

        //  ----------------
        //  Ordinal property
        //  ----------------

        /// <summary>
        /// Gets the ordinal number of weekday units.
        /// </summary>
        /// <value>
        /// The ordinal number of weekday units.
        /// </value>
        /// <remarks>
        /// Weekday ordinal units represent the position of the weekday within the next larger calendar unit, such as the month.
        /// For example, 2 is the weekday ordinal unit for the second Friday of the month.
        /// </remarks>

        public int Ordinal { get; }

        //  ------------------
        //  DayOfWeek property
        //  ------------------

        /// <summary>
        /// Gets the weekday of the Holiday.
        /// </summary>
        /// <value>
        /// The weekday of the Holiday.
        /// </value>

        public DayOfWeek DayOfWeek { get; }

        //  --------------
        //  Month property
        //  --------------

        /// <summary>
        /// Gets the month of the Holiday.
        /// </summary>
        /// <value>
        /// The month of the Holiday.
        /// </value>

        public int Month { get; }

        //  ---------------
        //  Offset property
        //  ---------------

        /// <summary>
        /// Gets the offset of the Holiday relative to the ordinal weekday date.
        /// </summary>
        /// <value>
        /// The offset of the Holiday relative to the ordinal weekday date.
        /// </value>

        public int Offset { get; }

        #endregion properties

        #region methods

        //  -----------------
        //  DateInYear method
        //  -----------------

        /// <summary>
        /// Calculates the date of the Holiday in the specified year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>
        /// The date of the Holiday in the specified year
        /// or <c>null</c> if the Holiday does not take place this year.
        /// </returns>

        public override DateTime? DateInYear(int year)
        {
            var date = new DateTime(year, Month, 1);
            var delta = (int)DayOfWeek - (int)date.DayOfWeek;

            return date.AddDays(delta >= 0 ? delta : delta + 7).AddDays(7 * (Ordinal - 1));
        }

        #endregion methods
    }

    #endregion WeekdayInMonthHoliday class
}

// eof "Calendar.cs"
