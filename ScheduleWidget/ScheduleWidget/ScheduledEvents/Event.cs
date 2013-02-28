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
        /// For events that occur only part of the year (optional)
        /// </summary>
        public RangeInYear RangeInYear { get; set; }

        /// <summary>
        /// The one-time, daily, weekly, or monthly frequency of the event as a
        /// value of FrequencyTypeEnum (0, 1, 2, or 4 only).
        /// </summary>
        public int Frequency { get; set; }

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
        /// The monthly interval expressed as enumeration
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
    }
}
