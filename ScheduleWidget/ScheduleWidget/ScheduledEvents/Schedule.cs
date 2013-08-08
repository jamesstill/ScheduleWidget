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
            return TemporalExpression.Includes(aDate);
        }

        /// <summary>
        /// Return the previous occurrence in the schedule for the given date.
        /// </summary>
        /// <param name="aDate"></param>
        /// <returns></returns>
        public DateTime? PreviousOccurrence(DateTime aDate)
        {
            var during = DateRange(aDate, true);
            var dates = Occurrences(during).OrderByDescending(o => o.Date);
            return dates.SkipWhile(o => o >= aDate.Date).FirstOrDefault();
        }

        /// <summary>
        /// Return the next occurrence in the schedule for the given date.
        /// </summary>
        /// <param name="aDate"></param>
        /// <returns></returns>
        public DateTime? NextOccurrence(DateTime aDate)
        {
            var during = DateRange(aDate, false);
            var dates = Occurrences(during);
            return dates.SkipWhile(o => o.Date <= aDate.Date).FirstOrDefault();
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

        //An effective way to find date range especially when the interval is greater than one for any date frequencies 
        //(every x days, every x weeks, every x months, every x quarters or every x years).
        //NOTE: Quarterly is still not completely done as it is not supporting the interval (every n quarter(s)) feature right now.
        /// <summary>
        /// Return a date range to find either previous or next occurrence
        /// for a given date by evaluating some properties of the event
        /// </summary>
        /// <param name="aDate"></param>
        /// <param name="previousOccurrence"></param>
        /// <returns></returns>
        private DateRange DateRange(DateTime aDate, bool previousOccurrence)
        {
            if (_event.FrequencyTypeOptions == FrequencyTypeEnum.None)
                return new DateRange { StartDateTime = aDate, EndDateTime = aDate };

            int interval;
            DateRange dateRange = null;

            switch (_event.FrequencyTypeOptions)
            {
                case FrequencyTypeEnum.Daily:
                    interval = _event.DayInterval + 1;
                    dateRange = previousOccurrence
                                ? new DateRange { StartDateTime = aDate.AddDays(-interval), EndDateTime = aDate }
                                : new DateRange { StartDateTime = aDate, EndDateTime = aDate.AddDays(interval) };
                    break;
                case FrequencyTypeEnum.Weekly:
                case FrequencyTypeEnum.EveryWeekDay:
                case FrequencyTypeEnum.EveryMonWedFri:
                case FrequencyTypeEnum.EveryTuTh:
                    interval = (_event.WeeklyInterval + 1) * 7;
                    dateRange = previousOccurrence
                                ? new DateRange { StartDateTime = aDate.AddDays(-interval), EndDateTime = aDate }
                                : new DateRange { StartDateTime = aDate, EndDateTime = aDate.AddDays(interval) };
                    break;
                case FrequencyTypeEnum.Monthly:
                    interval = _event.MonthInterval + 1;
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
                    interval = _event.YearInterval + 1;
                    dateRange = previousOccurrence
                                ? new DateRange { StartDateTime = aDate.AddYears(-interval), EndDateTime = aDate }
                                : new DateRange { StartDateTime = aDate, EndDateTime = aDate.AddYears(interval) };
                    break;
            }

            return dateRange;
        }
        /// <summary>
        /// Returns the date of last occurrence of the event.
        /// This method uses NumberOfOccurrences property to decide the last date.
        /// </summary>
        /// <returns>The date of last occurrence of the event.</returns>
        public DateTime? GetLastOccurrenceDate()
        {
            DateTime? firstDateTime = _event.FirstDateTime;
            if (!firstDateTime.HasValue)
            {
                return null;
            }
            DateTime startDateTime = firstDateTime.Value;
            FrequencyTypeEnum frequencyType = _event.FrequencyTypeOptions;
            if (frequencyType == FrequencyTypeEnum.None)
            {
                return firstDateTime;
            }

            if (!_event.NumberOfOccurrences.HasValue)
            {
                return firstDateTime;
            }

            int interval;
            int occurences = _event.NumberOfOccurrences.Value;
            DateRange dateRange = null;

            switch (frequencyType)
            {
                case FrequencyTypeEnum.Daily:
                    interval = _event.DayInterval + 1;
                    dateRange = new DateRange { StartDateTime = startDateTime, EndDateTime = startDateTime.AddDays(interval * occurences) };
                    break;
                case FrequencyTypeEnum.Weekly:
                case FrequencyTypeEnum.EveryWeekDay:
                case FrequencyTypeEnum.EveryMonWedFri:
                case FrequencyTypeEnum.EveryTuTh:
                    interval = (_event.WeeklyInterval + 1) * 7;
                    dateRange = new DateRange { StartDateTime = startDateTime, EndDateTime = startDateTime.AddDays(interval * occurences) };
                    break;
                case FrequencyTypeEnum.Monthly:
                    interval = _event.MonthInterval + 1;
                    dateRange = new DateRange { StartDateTime = startDateTime, EndDateTime = startDateTime.AddMonths(interval * occurences) };
                    break;
                case FrequencyTypeEnum.Quarterly:
                    //Assign a default value as there is no interval option available for this frequency type now.
                    interval = 12;
                    dateRange = new DateRange { StartDateTime = startDateTime, EndDateTime = startDateTime.AddMonths(interval * occurences) };
                    break;
                case FrequencyTypeEnum.Yearly:
                    interval = _event.YearInterval + 1;
                    dateRange = new DateRange { StartDateTime = startDateTime, EndDateTime = startDateTime.AddYears(interval * occurences) };
                    break;
            }

            IEnumerable<DateTime> items = Occurrences(dateRange);
            DateTime enddate = startDateTime;
            if (items != null)
            {
                enddate = items.ElementAtOrDefault(occurences-1);
            }
            return enddate;
        }

    }
}
