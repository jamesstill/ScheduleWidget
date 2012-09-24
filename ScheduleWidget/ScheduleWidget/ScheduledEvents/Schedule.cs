using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleWidget.ScheduledEvents.FrequencyBuilder;
using ScheduleWidget.TemporalExpressions;

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
            var during = new DateRange()
            {
                StartDateTime = aDate.AddMonths(-3),
                EndDateTime = aDate
            };
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
            var during = new DateRange()
            {
                StartDateTime = aDate,
                EndDateTime = aDate.AddMonths(3)
            };
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
    }
}
