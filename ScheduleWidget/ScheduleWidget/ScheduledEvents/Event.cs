using System;
using ScheduleWidget.Enums;

namespace ScheduleWidget.ScheduledEvents
{
    /// <summary>
    /// A generic event that is scheduled. An event is a concrete item such as "Street Cleaning"
    /// or "Summer Term Algebra 1" that has a literal start and end date. The event knows its own
    /// frequency (daily, weekly, or monthly), the days of the week on which it occurs, and if 
    /// monthly its monthly interval. Given an event and an optional set of excluded dates (such
    /// as holidays) a schedule can be created.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// The unique ID of the event.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The title of the event (e.g., "Street Cleaning")
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// If this is a one-time only event then set the date
        /// </summary>
        public DateTime? OneTimeOnlyEventDate { get; set; }

        /// <summary>
        /// If this is a event that repeats every x weeks set the date
        /// </summary>
        public DateTime? FirstDateTime { get; set; }

        /// <summary>
        /// If this event has a yearly frequency then the anniversary
        /// describes the fixed year after year month and day of recurrence.
        /// </summary>
        public Anniversary Anniversary { get; set; }

        /// <summary>
        /// For events that occur only part of the year (optional)
        /// </summary>
        public RangeInYear RangeInYear { get; set; }

        /// <summary>
        /// The one-time, daily, weekly, or monthly frequency of the event as a
        /// value of FrequencyTypeEnum (0, 1, 2, 4, 8 or 16 only).
        /// </summary>
        public int Frequency { get; set; }

        /// <summary>
        /// If an event is quarterly, which quarter(s) does it fall in? 
        /// From QuarterEnum: First, Second, Third, Fourth 
        /// E.g., Second and Fourth quarters == 10 (2 + 8)
        /// </summary>
        public int QuarterInterval { get; set; }

        /// <summary>
        /// If the frequency is quarterly then the interval of the
        /// event as a flag attribute value of QuarterlyIntervalEnum.
        /// E.g., the first and last months of the quarter == 5
        /// </summary>
        public int QuarterlyInterval { get; set; }

        /// <summary>
        /// If the frequency is monthly then the interval of the
        /// event as a flag attribute value of MonthlyIntervalEnum.
        /// E.g., the first and third weeks of the month == 5
        /// </summary>
        public int MonthlyInterval { get; set; }

        /// <summary>
        /// If the frequency is weekly then the interval of the
        /// event as an int.
        /// E.g., every second week == 2
        /// </summary>
        public int WeeklyInterval { get; set; }

        /// <summary>
        /// The days of the week that the event occurs as a value
        /// of the DayOfWeekEnum flag attribute value. E.g., every
        /// day of the week is 127.
        /// </summary>
        public int DaysOfWeek { get; set; }

        /// <summary>
        /// The frequency expressed as enumeration.
        /// </summary>
        public FrequencyTypeEnum FrequencyTypeOptions
        {
            get
            {
                return (FrequencyTypeEnum)Frequency;
            }
            set
            {
                Frequency = (int)value;
            }
        }

        /// <summary>
        /// The actual Quarter expressed as an enumeration
        /// </summary>
        public QuarterEnum QuarterlyOptions
        {
            get { return (QuarterEnum)QuarterInterval; }
            set { QuarterInterval = (int)value; }

        }

        /// <summary>
        /// The quarterly interval (i.e. month) expressed as enumeration
        /// </summary>
        public QuarterlyIntervalEnum QuarterlyIntervalOptions
        {
            get
            {
                return (QuarterlyIntervalEnum)QuarterlyInterval;
            }
            set
            {
                QuarterlyInterval = (int)value;
            }
        }

        /// <summary>
        /// The monthly interval (i.e. week) expressed as enumeration
        /// </summary>
        public MonthlyIntervalEnum MonthlyIntervalOptions
        {
            get
            {
                return (MonthlyIntervalEnum)MonthlyInterval;
            }
            set
            {
                MonthlyInterval = (int)value;
            }
        }

        /// <summary>
        /// The monthly interval expressed as enumeration
        /// The weekly interval (i.e. 2 == every 2 weeks) expressed as enumeration
        /// </summary>
        public int WeeklyIntervalOptions
        {
            get
            {
                return WeeklyInterval;
            }
            set
            {
                WeeklyInterval = value;
            }
        }

        /// <summary>
        /// The days of the week expressed as enumeration.
        /// </summary>
        public DayOfWeekEnum DaysOfWeekOptions
        {
            get
            {
                return (DayOfWeekEnum)DaysOfWeek;
            }
            set
            {
                DaysOfWeek = (int)value;
            }
        }

        /// <summary>
        /// The days interval used for 'Daily' frequency type.
        /// E.g., every day, every 2 days, every 3 days, .... , every n days.
        /// </summary>
        public int DayInterval { get; set; }

        /// <summary>
        /// The months interval used for 'Monthly' frequency type.
        /// E.g., every month, every 2 months, every 3 months, .... , every n months.
        /// </summary>
        public int MonthInterval { get; set; }

        /// <summary>
        /// A particular day of a month used for 'Monthly' frequency type.
        /// E.g., 1st day of every month, 10th day of every 2 months, etc.
        /// </summary>
        public int DayOfMonth { get; set; }

        /// <summary>
        /// The years interval used for 'Yearly' frequency type.
        /// E.g., every year, every 2 years, every 3 years, .... , every n years.
        /// </summary>
        public int YearInterval { get; set; }
    }
}
