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
    [Serializable]
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
        /// If this event occurs only after a certain date,
        /// or repeats every x weeks, set the date.
        /// </summary>
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// If this event occurs only until a specific ending date, set the date.
        /// Note that if you set both the EndDateTime, and a NumberOfOccurrences,
        /// The actual end date of the event will be determined by the more restrictive of 
        /// the two values.
        /// </summary>
        public DateTime? EndDateTime { get; set; }

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
        /// value of FrequencyTypeEnum (0, 1, 2, 4, 8, 16, 32, 64 or 128 only).
        /// </summary>
        public int Frequency { get; set; }

        /// <summary>
        /// If the frequency is daily, weekly, monthly, or yearly, 
        /// then set the interval of the event as an int.
        /// E.g., every second week == 2, every fourth day == 4.
        /// </summary>
        public int RepeatInterval { get; set; }

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
        /// The days of the week that the event occurs as a value
        /// of the DayOfWeekEnum flag attribute value. E.g., every
        /// day of the week is 127.
        /// </summary>
        public int DaysOfWeek { get; set; }

        /// <summary>
        /// This holds the number of occurrences that was last set with the 
        /// SetEndDateTimeForMaximumNumberOfOccurrences() function, for informational purposes only.
        /// If nothing has been set, this will contain null.
        /// </summary>
        public int? NumberOfOccurrencesThatWasLastSet { get; private set; }

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
        /// A particular day of a month used for 'Monthly' frequency type.
        /// E.g., 1st day of every month, 10th day of every 2 months, etc.
        /// </summary>
        public int DayOfMonth { get; set; }


        /// <summary>
        /// Returns the start and end date for this event as a date range instance.
        /// If the event does not have a start date or end date, the date range values will be
        /// DateTime.MinValue and DateTime.MaxValue, respectively.
        /// </summary>
        public DateRange GetEventLimitsAsDateRange()
        {
            DateRange range = new DateRange();
            range.StartDateTime = (StartDateTime == null) ? DateTime.MinValue : (DateTime)StartDateTime;
            range.EndDateTime = (EndDateTime == null) ? DateTime.MaxValue : (DateTime)EndDateTime;
            return range;
        }

        /// <summary>
        /// Returns true if a date falls within this event's limits, otherwise false.
        /// </summary>
        internal bool DateIsWithinLimits(DateTime aDate)
        {
            DateRange limits = GetEventLimitsAsDateRange();
            return (aDate >= limits.StartDateTime && aDate <= limits.EndDateTime);
        }

        /// <summary>
        /// SetEndDateTimeForMaximumNumberOfOccurrences,
        /// This will use the currently defined event schedule, to choose and set an
        /// EndDateTime that will limit the event to a fixed maximum number of occurrences.
        /// Calling this function will override any previously set EndDateTime.
        /// 
        /// All other desired event parameters should be set before this function is called.
        /// Previously set variables should include at minimum, a StartDateTime, and a Frequency.
        /// 
        /// The reason this sets a "maximum" number of occurrences, is that the number of 
        /// actual occurrences can be reduced by excluding occurrence dates from a Schedule 
        /// instance. Changing the exclusions will not change the EndDateTime.
        /// 
        /// The supplied maximumNumberOfOccurrences value is recorded for informational
        /// purposes only. Only the EndDateTime is used by the Event and Schedule calculations.
        /// 
        /// Setting this to null will clear the NumberOfOccurrencesThatWasLastSet variable, but will
        /// not change the EndDateTime value.
        /// </summary>
        public void SetEndDateForNumberOfOccurrences(int? numberOfOccurrences)
        {
            // If the supplied parameter is null, clear the last set number of occurrences and return.
            if (numberOfOccurrences == null)
            {
                NumberOfOccurrencesThatWasLastSet = null;
                return;
            }
            // Validate the input parameters.
            if (numberOfOccurrences < 1)
                throw new Exception("SetEndDateTimeForMaximumNumberOfOccurrences(), " +
                    "numberOfOccurrences cannot be less than one.");
            // Calculate and set the appropriate end date.
            Schedule schedule = new Schedule(this);
            EndDateTime = schedule.zInternalGetEndDateBasedOnNumberOfOccurrences((int)numberOfOccurrences);
            // Store the last set number of occurrences, for future reference by the user.
            NumberOfOccurrencesThatWasLastSet = numberOfOccurrences;
        }
    }
}
