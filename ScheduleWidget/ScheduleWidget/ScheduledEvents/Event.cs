using System;
using ScheduleWidget.Enums;
using System.Collections.Generic;

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
        public int? NumberOfOccurrencesThatWasLastSet { get; set; }

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
        /// The dates included in this range will never be null.
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
        /// SetEndDateWithNumberOfOccurrences,
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
        public void SetEndDateWithNumberOfOccurrences(int? numberOfOccurrences)
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

        /// <summary>
        /// SetEndDateTimeWithDate,
        /// This is a convenience function, for setting or clearing the end date while remembering to
        /// clear any pre-existing NumberOfOccurrencesThatWasLastSet value.
        /// Setting this function to null, will cause the event to repeat forever.
        /// </summary>
        public void SetEndDateTimeWithDate(DateTime? endDate)
        {
            NumberOfOccurrencesThatWasLastSet = null;
            EndDateTime = endDate;
        }

        /// <summary>
        /// CanProduceOccurrences,
        /// This will return true if this event is capable of generating occurrences, otherwise false.
        /// </summary>
        public bool CanProduceOccurrences()
        {
            if (HasBrokenZeroOccurrenceConfiguration())
                return false;
            int manyYears = 210;
            int largeRepeatInterval = 100;
            DateRange eventLimits = GetEventLimitsAsDateRange();
            if (RepeatInterval < largeRepeatInterval &&
                (eventLimits.EndDateTime.Year - eventLimits.StartDateTime.Year) > manyYears)
                return true;
            Schedule schedule = new Schedule(this);
            DateTime? occurrence = schedule.NextOccurrence(eventLimits.StartDateTime);
            if (occurrence != null) { return true; }
            return false;
        }

        /// <summary>
        /// RoughlyEstimateMinimumEventOccurrences,
        /// This will return a very rough, and conservative, guess of the minimum number of occurrences
        /// that should be generated by a particular event. 
        /// </summary>
        static public int RoughlyEstimateMinimumEventOccurrences(Event eventInstance)
        {
            if (eventInstance.HasBrokenZeroOccurrenceConfiguration()) { return 0; }
            int repeatInterval = eventInstance.RepeatInterval;
            DateRange eventLimits = eventInstance.GetEventLimitsAsDateRange();
            TimeSpan timeSpan = eventLimits.EndDateTime - eventLimits.StartDateTime;
            int spanDays = timeSpan.Days;
            int spanWeeks = timeSpan.Days / 7;
            int result = 0;
            switch (eventInstance.FrequencyTypeOptions)
            {
                case FrequencyTypeEnum.None:
                    result = 1;
                    break;
                case FrequencyTypeEnum.Daily:
                    result = (spanDays / repeatInterval) - 1;
                    break;
                case FrequencyTypeEnum.Weekly:
                    int includedDays = 0;
                    includedDays += eventInstance.DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Mon) ? 1 : 0;
                    includedDays += eventInstance.DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Tue) ? 1 : 0;
                    includedDays += eventInstance.DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Wed) ? 1 : 0;
                    includedDays += eventInstance.DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Thu) ? 1 : 0;
                    includedDays += eventInstance.DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Fri) ? 1 : 0;
                    includedDays += eventInstance.DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Sat) ? 1 : 0;
                    includedDays += eventInstance.DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Sun) ? 1 : 0;
                    result = ((spanWeeks / repeatInterval) * includedDays) - 1;
                    break;
                case FrequencyTypeEnum.EveryWeekDay:
                    result = ((spanWeeks / repeatInterval) * 5) - 1;
                    break;
                case FrequencyTypeEnum.EveryMonWedFri:
                    result = ((spanWeeks / repeatInterval) * 3) - 1;
                    break;
                case FrequencyTypeEnum.EveryTuTh:
                    result = ((spanWeeks / repeatInterval) * 2) - 1;
                    break;
                case FrequencyTypeEnum.Monthly:
                    int spanMonths = spanDays / 31;
                    result = (spanMonths / repeatInterval) - 1;
                    break;
                case FrequencyTypeEnum.Quarterly:
                    int spanQuarters = spanDays / 92;
                    result = (spanQuarters / repeatInterval) - 1;
                    break;
                case FrequencyTypeEnum.Yearly:
                    int spanYears = spanDays / 365;
                    result = (spanYears / repeatInterval) - 1;
                    break;
            }
            return (result > 0) ? result : 0;
        }

        /// <summary>
        /// HasBrokenZeroOccurrenceConfiguration,
        /// This will return true if this event has certain invalid, broken configurations that will
        /// generate zero occurrences.
        /// </summary>
        private bool HasBrokenZeroOccurrenceConfiguration()
        {
            if (FrequencyTypeOptions == FrequencyTypeEnum.Weekly && DaysOfWeekOptions == 0)
                return true;
            if (FrequencyTypeOptions == FrequencyTypeEnum.Yearly && Anniversary == null)
                return true;
            return false;
        }

        /// <summary>
        /// AreEventSchedulesEqual,
        /// This will return true if the two supplied event instances have equivalent scheduling information.
        /// For example, if both schedules occur every 3 months on day 7, then this will return true.
        /// This only compares the scheduling details.
        /// It specifically does not compare the following fields for equality:
        /// ID, Title, NumberOfOccurrencesThatWasLastSet.
        /// </summary>
        static public bool AreEventSchedulesEqual(Event first, Event second)
        {
            // Return false if either event is null.
            if (first == null || second == null)
                return false;
            // Compare simple fields in alphabetical order.
            if (first.DayOfMonth != second.DayOfMonth) return false;
            if (first.DaysOfWeek != second.DaysOfWeek) return false;
            if (first.EndDateTime != second.EndDateTime) return false;
            if (first.Frequency != second.Frequency) return false;
            if (first.MonthlyInterval != second.MonthlyInterval) return false;
            if (first.OneTimeOnlyEventDate != second.OneTimeOnlyEventDate) return false;
            if (first.QuarterInterval != second.QuarterInterval) return false;
            if (first.QuarterlyInterval != second.QuarterlyInterval) return false;
            if (first.RepeatInterval != second.RepeatInterval) return false;
            if (first.StartDateTime != second.StartDateTime) return false;
            // Compare subclasses.
            if (first.Anniversary == null && second.Anniversary != null) return false;
            if (first.Anniversary != null)
            {
                if (second.Anniversary == null) return false;
                if (first.Anniversary.Day != second.Anniversary.Day) return false;
                if (first.Anniversary.Month != second.Anniversary.Month) return false;
            }
            if (first.RangeInYear == null && second.RangeInYear != null) return false;
            if (first.RangeInYear != null)
            {
                if (second.RangeInYear == null) return false;
                if (first.RangeInYear.EndDayOfMonth != second.RangeInYear.EndDayOfMonth) return false;
                if (first.RangeInYear.EndMonth != second.RangeInYear.EndMonth) return false;
                if (first.RangeInYear.StartDayOfMonth != second.RangeInYear.StartDayOfMonth) return false;
                if (first.RangeInYear.StartMonth != second.RangeInYear.StartMonth) return false;
            }
            // If we got this far, it means all the compared fields were equal.
            return true;
        }
    }
}
