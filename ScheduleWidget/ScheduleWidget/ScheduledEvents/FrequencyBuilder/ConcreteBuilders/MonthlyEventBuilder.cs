using System;
using ScheduleWidget.Enums;
using ScheduleWidget.TemporalExpressions;

namespace ScheduleWidget.ScheduledEvents.FrequencyBuilder.ConcreteBuilders
{
    public class MonthlyEventBuilder : IEventFrequencyBuilder
    {
        private readonly Event _event;

        public MonthlyEventBuilder(Event aEvent)
        {
            _event = aEvent;
        }

        public UnionTE Create()
        {
            var union = new UnionTE();
            var monthlyIntervals = EnumExtensions.GetFlags(_event.MonthlyIntervalOptions);
            var daysOfWeek = EnumExtensions.GetFlags(_event.DaysOfWeekOptions);

            foreach (MonthlyIntervalEnum monthlyInterval in monthlyIntervals)
            {
                foreach (DayOfWeekEnum dayOfWeek in daysOfWeek)
                {
                    var dayInMonth = new DayInMonthTE(dayOfWeek, monthlyInterval);
                    union.Add(dayInMonth);
                }
            }
            return union;
        }
    }
}
