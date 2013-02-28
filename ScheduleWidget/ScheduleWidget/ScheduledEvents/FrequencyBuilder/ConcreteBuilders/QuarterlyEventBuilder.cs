using System;
using ScheduleWidget.Enums;
using ScheduleWidget.TemporalExpressions;

namespace ScheduleWidget.ScheduledEvents.FrequencyBuilder.ConcreteBuilders
{
    /// <summary>
    /// Allows for recurring Quarterly events. Events set to Freq = Quarterly will also have a MonthlyInterval and DailyInterval set, 
    /// so we can specify e.g. event takes place on the 1st and last quarter, last week, Wednesday, Thursday and Fridays
    /// </summary>
    public class QuarterlyEventBuilder : IEventFrequencyBuilder
    {
        private readonly Event _event;

        public QuarterlyEventBuilder(Event aEvent)
        {
            _event = aEvent;
        }

        public UnionTE Create()
        {
            var union = new UnionTE();

            var quarters = EnumExtensions.GetFlags(_event.QuarterlyOptions);
            var quarterlyIntervals = EnumExtensions.GetFlags(_event.QuarterlyIntervalOptions);
            var monthlyIntervals = EnumExtensions.GetFlags(_event.MonthlyIntervalOptions);
            var daysOfWeek = EnumExtensions.GetFlags(_event.DaysOfWeekOptions);

            foreach (QuarterEnum quarter in quarters)
            {
                foreach (QuarterlyIntervalEnum qInt in quarterlyIntervals)
                {
                    foreach (MonthlyIntervalEnum mInt in monthlyIntervals)
                    {
                        foreach (DayOfWeekEnum day in daysOfWeek)
                        {
                            var dayInQuarter = new DayInQuarterTE(quarter, qInt, mInt, day);
                            union.Add(dayInQuarter);
                        }
                    }
                }
            }
            return union;
        }
    }
}
