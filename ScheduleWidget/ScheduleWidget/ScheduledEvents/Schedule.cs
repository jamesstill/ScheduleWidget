using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleWidget.ScheduledEvents.FrequencyBuilder;
using ScheduleWidget.TemporalExpressions;
using ScheduleWidget.Enums;

namespace ScheduleWidget.ScheduledEvents
{
    /// <summary>
    /// A schedule is a collection of one or more recurring events. It contains functionality to
    /// work out what event dates do or not not fall on the schedule's days (occurrences). This 
    /// schedule engine implements Martin Fowler's white paper "Recurring Events for Calendars" 
    /// (http://martinfowler.com/apsupp/recurring.pdf).
    /// </summary>
    public class Schedule : ISchedule
    {
        private readonly Event _event;
        public TemporalExpression TemporalExpression { get; private set; }

        public Schedule(Event aEvent)
        {
            _event = aEvent;
            TemporalExpression = Create();
        }

        public Schedule(Event aEvent, IEnumerable<DateTime> excludedDates)
        {
            _event = aEvent;
            TemporalExpression = Create(excludedDates);
        }

        public Schedule(Event aEvent, UnionTE excludedDates)
        {
            _event = aEvent;
            TemporalExpression = Create(excludedDates);
        }

        /// <summary>
        /// Return the schedule's event
        /// </summary>
        public Event Event
        {
            get { return _event; }
        }

        /// <summary>
        /// Return true if the date occurs in the schedule.
        /// </summary>
        /// <param name="aDate"></param>
        /// <returns></returns>
        public bool IsOccurring(DateTime aDate)
        {
            if (!_event.DateIsWithinLimits(aDate))
                return false;
            return TemporalExpression.Includes(aDate);
        }

        /// <summary>
        /// PreviousOccurrence(DateTime),
        /// Return the previous occurrence in the schedule for the given date.
        /// Returns null if nothing is found.
        /// And start date is required in order to determine the search limits.
        /// Throws an exception if the event has no start date, and no search range is provided.
        /// Notes:
        /// This is not inclusive of the supplied date. Only earlier dates can be returned.
        /// This returned value will stay inside the event StartDateTime and EndDateTime.
        /// This function takes into account any excluded dates that were provided when the 
        /// schedule was created.
        /// This function will use the Event.StartDateTime to set a default search limit.
        /// Improved performance can generally be obtained by using the previous occurrence function
        /// with an explicit date range.
        /// </summary>
        /// <param name="aDate"></param>
        /// <returns></returns>
        public DateTime? PreviousOccurrence(DateTime aDate)
        {
            if (_event.StartDateTime == null || _event.StartDateTime == DateTime.MinValue)
                throw new Exception("PreviousOccurrence(), The event must have a StartDateTime, or " +
                "a DateRange must be included, to use this function.");
            if (aDate < _event.StartDateTime) { return null; }
            return PreviousOccurrence(aDate, new DateRange((DateTime)_event.StartDateTime, aDate));
        }

        /// <summary>
        /// PreviousOccurrence(DateTime, DateRange),
        /// Return the previous occurrence in the schedule for the given date, from within the
        /// specified date range. Returns null if nothing is found.
        /// See PreviousOccurrence(DateTime) for additional details.
        /// </summary>
        public DateTime? PreviousOccurrence(DateTime aDate, DateRange during)
        {
            // Make sure that our search begins no later than the end of the during range.
            DateTime duringEnding = during.EndDateTime.SafeAddDays(1);
            if (aDate > duringEnding) { aDate = duringEnding; }
            // Make sure that our search begins no later than the end of the event limits range.
            DateRange eventLimits = _event.GetEventLimitsAsDateRange();
            DateTime eventEnding = eventLimits.EndDateTime.SafeAddDays(1);
            if (aDate > eventEnding) { aDate = eventEnding; }
            // Find the loop limit date for this search.
            // This should be the latest available start date from all the applicable limiting ranges.
            DateTime earliestSearchDate = during.StartDateTime;
            if (eventLimits.StartDateTime > earliestSearchDate)
                earliestSearchDate = eventLimits.StartDateTime;
            // Perform the search.
            DateTime? occurrence = null;
            while (aDate >= earliestSearchDate)
            {
                // Get a working range for this search.
                var workingRange = DateRangeForPreviousOrNextOccurrence(aDate, true, earliestSearchDate);
                // Find the previous occurrence.
                var dates = Occurrences(workingRange).OrderByDescending(o => o.Date);
                occurrence = dates.SkipWhile(o => o >= aDate.Date).FirstOrDefault();
                occurrence = (occurrence == default(DateTime)) ? null : occurrence;
                // Break the loop if an occurrence was found.
                if (occurrence != null) { break; }
                // Increment the search date to search an earlier DateRange.
                aDate = workingRange.StartDateTime.AddDays(-1);
            }

            // Make sure that our result is no earlier than the start of the event limits range.
            if (occurrence != null && occurrence < eventLimits.StartDateTime) { occurrence = null; }
            // Make sure that our result is no earlier than the start of the during range.
            if (occurrence != null && occurrence < during.StartDateTime)
                occurrence = null;
            return occurrence;
        }

        /// <summary>
        /// NextOccurrence(DateTime),
        /// Return the next occurrence in the schedule for the given date.
        /// Returns null if nothing is found.
        /// And ending date is required in order to determine the search limits.
        /// Throws an exception if the event has no end date, and no search range is provided.
        /// Notes:
        /// This is not inclusive of the supplied date. Only later dates can be returned.
        /// This returned value will stay inside the event StartDateTime and EndDateTime.
        /// This function takes into account any excluded dates that were provided when the 
        /// schedule was created.
        /// This function will use the Event.EndDateTime to set a default search limit.
        /// Improved performance can generally be obtained by using the next occurrence function
        /// with an explicit date range.
        /// </summary>
        /// <param name="aDate"></param>
        /// <returns></returns>
        public DateTime? NextOccurrence(DateTime aDate)
        {
            if (_event.EndDateTime == null || _event.EndDateTime == DateTime.MaxValue)
                throw new Exception("NextOccurrence(), The event must have an EndDateTime, or " +
                "a DateRange must be included, to use this function.");
            if (aDate > _event.EndDateTime) { return null; }
            return NextOccurrence(aDate, new DateRange(aDate, (DateTime)_event.EndDateTime));
        }

        /// <summary>
        /// NextOccurrence(DateTime, DateRange),
        /// Return the next occurrence in the schedule for the given date, from within the
        /// specified date range. Returns null if nothing is found.
        /// See NextOccurrence(DateTime) for additional details.
        /// </summary>
        public DateTime? NextOccurrence(DateTime aDate, DateRange during)
        {
            // Make sure that our search begins no earlier than the beginning of the during range.
            DateTime startOfDuring = during.StartDateTime.SafeAddDays(-1);
            if (aDate < startOfDuring) { aDate = startOfDuring; }
            // Make sure that our search begins no earlier than the start of the event limits range.
            DateRange eventLimits = _event.GetEventLimitsAsDateRange();
            DateTime startOfEvent = eventLimits.StartDateTime.SafeAddDays(-1);
            if (aDate < startOfEvent) { aDate = startOfEvent; }
            // Find the loop limit date for this search.
            // This should be the earliest available ending date from all the applicable limiting ranges.
            DateTime latestSearchDate = during.EndDateTime;
            if (eventLimits.EndDateTime < latestSearchDate)
                latestSearchDate = eventLimits.EndDateTime;
            // Perform the search.
            DateTime? occurrence = null;
            while (aDate <= latestSearchDate)
            {
                // Get a working range for this search.
                var workingRange = DateRangeForPreviousOrNextOccurrence(aDate, false, latestSearchDate);
                // Find the next occurrence.
                var dates = Occurrences(workingRange);
                occurrence = dates.SkipWhile(o => o.Date <= aDate.Date).FirstOrDefault();
                occurrence = (occurrence == default(DateTime)) ? null : occurrence;
                // Break the loop if an occurrence was found.
                if (occurrence != null) { break; }
                // Increment the search date to search the next DateRange.
                aDate = workingRange.EndDateTime.AddDays(1);
            }
            // Make sure that our result is no later than the end of the event limits range.
            if (occurrence != null && occurrence > eventLimits.EndDateTime) { occurrence = null; }
            // Make sure that our result is no later than the end of the during range.
            if (occurrence != null && occurrence > during.EndDateTime) { occurrence = null; }
            return occurrence;
        }

        /// <summary>
        /// Return all occurrences within the given date range.
        /// </summary>
        /// <param name="during">DateRange</param>
        /// <returns></returns>
        public IEnumerable<DateTime> Occurrences(DateRange during)
        {
            return EachDay(during.StartDateTime, during.EndDateTime).Where(IsOccurring);
        }

        /// <summary>
        /// Create and return a base schedule with no exclusions.
        /// </summary>
        /// <returns></returns>
        private TemporalExpression Create()
        {
            var union = new UnionTE();
            return Create(union);
        }
        /// <summary>
        /// Create and return a base schedule including exclusions if applicable.
        /// </summary>
        /// <param name="excludedDates"></param>
        /// <returns></returns>
        private TemporalExpression Create(IEnumerable<DateTime> excludedDates)
        {
            var union = new UnionTE();
            if (excludedDates != null)
            {
                foreach (var date in excludedDates)
                {
                    union.Add(new DateTE(date));
                }
            }
            return Create(union);
        }

        /// <summary>
        /// Create and return a base schedule including exclusions if applicable.
        /// </summary>
        /// <param name="excludedDates">Holidays or any excluded dates</param>
        /// <returns>Complete schedule as an expression</returns>
        private TemporalExpression Create(TemporalExpression excludedDates)
        {
            var intersectionTE = new IntersectionTE();

            // get a builder that knows how to create a UnionTE for the event frequency
            var builder = EventFrequencyBuilder.Create(_event);
            var union = builder.Create();
            intersectionTE.Add(union);

            if (_event.RangeInYear != null)
            {
                var rangeEachYear = GetRangeForYear(_event);
                intersectionTE.Add(rangeEachYear);
            }

            return new DifferenceTE(intersectionTE, excludedDates);
        }

        private static RangeEachYearTE GetRangeForYear(Event aEvent)
        {
            if (aEvent.RangeInYear == null)
            {
                return null;
            }

            var startMonth = aEvent.RangeInYear.StartMonth;
            var endMonth = aEvent.RangeInYear.EndMonth;

            if (!aEvent.RangeInYear.StartDayOfMonth.HasValue)
            {
                return new RangeEachYearTE(startMonth, endMonth);
            }

            if (!aEvent.RangeInYear.EndDayOfMonth.HasValue)
            {
                return new RangeEachYearTE(startMonth, endMonth);
            }

            var startDay = aEvent.RangeInYear.StartDayOfMonth.Value;
            var endDay = aEvent.RangeInYear.EndDayOfMonth.Value;
            return new RangeEachYearTE(startMonth, endMonth, startDay, endDay);
        }

        /// <summary>
        /// Return each calendar day in the date range in ascending order
        /// </summary>
        /// <param name="from"></param>
        /// <param name="through"></param>
        /// <returns></returns>
        private static IEnumerable<DateTime> EachDay(DateTime from, DateTime through)
        {
            for (var day = from.Date; day.Date <= through.Date; day = day.AddDays(1))
                yield return day;
        }
        /// <summary>
        /// DateRangeForPreviousOrNextOccurrence
        /// This is an effective way to find date range especially when the interval is greater than one for any date
        /// frequencies (every x days, every x weeks, every x months, every x quarters or every x years).
        /// NOTE: Quarterly is still not completely done as it is not supporting the interval 
        /// (every n quarter(s)) feature right now.
        /// 
        /// Return a date range to find either previous or next occurrence
        /// for a given date by evaluating some properties of the event
        /// </summary>
        internal DateRange DateRangeForPreviousOrNextOccurrence(
            DateTime aDate, bool previousOccurrence, DateTime? optionalLimitingDate)
        {
            if (_event.FrequencyTypeOptions == FrequencyTypeEnum.None)
                return new DateRange { StartDateTime = aDate, EndDateTime = aDate };

            int interval;
            DateRange dateRange = null;

            switch (_event.FrequencyTypeOptions)
            {
                case FrequencyTypeEnum.Daily:
                    interval = _event.RepeatInterval + 1;
                    dateRange = previousOccurrence
                                ? new DateRange { StartDateTime = aDate.AddDays(-interval), EndDateTime = aDate }
                                : new DateRange { StartDateTime = aDate, EndDateTime = aDate.AddDays(interval) };
                    break;
                case FrequencyTypeEnum.Weekly:
                case FrequencyTypeEnum.EveryWeekDay:
                case FrequencyTypeEnum.EveryMonWedFri:
                case FrequencyTypeEnum.EveryTuTh:
                    interval = (_event.RepeatInterval + 1) * 7;
                    dateRange = previousOccurrence
                                ? new DateRange { StartDateTime = aDate.AddDays(-interval), EndDateTime = aDate }
                                : new DateRange { StartDateTime = aDate, EndDateTime = aDate.AddDays(interval) };
                    break;
                case FrequencyTypeEnum.Monthly:
                    interval = _event.RepeatInterval + 1;
                    dateRange = previousOccurrence
                                ? new DateRange { StartDateTime = aDate.AddMonths(-interval), EndDateTime = aDate }
                                : new DateRange { StartDateTime = aDate, EndDateTime = aDate.AddMonths(interval) };
                    break;
                case FrequencyTypeEnum.Quarterly:
                    //Assign a default value as there is no interval option available for this frequency type now.
                    interval = 12;
                    dateRange = previousOccurrence
                                ? new DateRange { StartDateTime = aDate.AddMonths(-interval), EndDateTime = aDate }
                                : new DateRange { StartDateTime = aDate, EndDateTime = aDate.AddMonths(interval) };
                    break;
                case FrequencyTypeEnum.Yearly:
                    interval = _event.RepeatInterval + 1;
                    dateRange = previousOccurrence
                                ? new DateRange { StartDateTime = aDate.AddYears(-interval), EndDateTime = aDate }
                                : new DateRange { StartDateTime = aDate, EndDateTime = aDate.AddYears(interval) };
                    break;
            }
            if (optionalLimitingDate != null)
            {
                if (previousOccurrence)
                    dateRange.StartDateTime = (optionalLimitingDate > dateRange.StartDateTime) ? 
                        (DateTime)optionalLimitingDate : dateRange.StartDateTime;
                else
                    dateRange.EndDateTime = (optionalLimitingDate < dateRange.EndDateTime) ?
                        (DateTime)optionalLimitingDate : dateRange.EndDateTime;
            }
            if (dateRange.StartDateTime > dateRange.EndDateTime)
                throw new Exception("Start and end date are inconsistent. This should not happen.");
            return dateRange;
        }


        /// <summary>
        /// GetLastOccurrenceDate,
        /// Returns the date of last existing occurrence of the event.
        /// The last occurrence can happen on or before the Event.EndDateTime.
        /// 
        /// Dates that have been excluded in the Schedule constructor will not be included.
        /// If nothing is found, this will return null.
        /// </summary>
        /// <returns>The date of last occurrence of the event.</returns>
        public DateTime? GetLastOccurrenceDate()
        {
            if (_event.StartDateTime == null || _event.EndDateTime == null) { return null; }
            DateTime startDateTime = (DateTime)_event.StartDateTime;
            DateTime endDateTime = (DateTime)_event.EndDateTime;
            if (_event.FrequencyTypeOptions == FrequencyTypeEnum.None) { return startDateTime; }
            DateRange dateRange = new DateRange { StartDateTime = startDateTime, EndDateTime = endDateTime };
            IEnumerable<DateTime> items = Occurrences(dateRange);
            if (items != null)
            {
                DateTime foundDate = items.LastOrDefault();
                if (foundDate == default(DateTime)) { return null; }
                return foundDate;
            }
            return null;
        }

        /// <summary>
        /// zInternalGetEndDateBasedOnNumberOfOccurrences,
        /// Returns and end date for a schedule that will limit the schedule to a fixed maximum
        /// number of occurrences.
        /// This calculation assumes that this schedule was created without any excluded dates.
        /// This assumption should be true, since this function is only used internally.
        /// </summary>
        internal DateTime? zInternalGetEndDateBasedOnNumberOfOccurrences(int numberOfOccurrences)
        {
            int occurences = numberOfOccurrences;
            if (_event.StartDateTime == null)
                throw new Exception("GetEndDateBasedOnNumberOfOccurrences(), " +
                    "A StartDateTime must be set before using this function.");
            DateTime startDateTime = (DateTime)_event.StartDateTime;
            FrequencyTypeEnum frequencyType = _event.FrequencyTypeOptions;
            if (frequencyType == FrequencyTypeEnum.None)
                return startDateTime;
            int interval;
            DateRange dateRange = null;

            switch (frequencyType)
            {
                case FrequencyTypeEnum.Daily:
                    interval = _event.RepeatInterval + 1;
                    dateRange = new DateRange { StartDateTime = startDateTime, EndDateTime = startDateTime.AddDays(interval * occurences) };
                    break;
                case FrequencyTypeEnum.Weekly:
                case FrequencyTypeEnum.EveryWeekDay:
                case FrequencyTypeEnum.EveryMonWedFri:
                case FrequencyTypeEnum.EveryTuTh:
                    interval = (_event.RepeatInterval + 1) * 7;
                    dateRange = new DateRange { StartDateTime = startDateTime, EndDateTime = startDateTime.AddDays(interval * occurences) };
                    break;
                case FrequencyTypeEnum.Monthly:
                    interval = _event.RepeatInterval + 1;
                    dateRange = new DateRange { StartDateTime = startDateTime, EndDateTime = startDateTime.AddMonths(interval * occurences) };
                    break;
                case FrequencyTypeEnum.Quarterly:
                    //Assign a default value as there is no interval option available for this frequency type now.
                    interval = 12;
                    dateRange = new DateRange { StartDateTime = startDateTime, EndDateTime = startDateTime.AddMonths(interval * occurences) };
                    break;
                case FrequencyTypeEnum.Yearly:
                    interval = _event.RepeatInterval + 1;
                    dateRange = new DateRange { StartDateTime = startDateTime, EndDateTime = startDateTime.AddYears(interval * occurences) };
                    break;
            }
            // Find and return the date of the last occurrence.
            IEnumerable<DateTime> items = Occurrences(dateRange);
            if (items == null) return null;
            DateTime lastDate = items.ElementAtOrDefault(occurences - 1);
            if (lastDate == default(DateTime)) { return null; }
            return lastDate;
        }
    }
}
