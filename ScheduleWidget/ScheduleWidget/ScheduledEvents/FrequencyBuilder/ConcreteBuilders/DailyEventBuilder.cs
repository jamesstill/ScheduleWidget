using System;
using ScheduleWidget.Enums;
using ScheduleWidget.TemporalExpressions;

namespace ScheduleWidget.ScheduledEvents.FrequencyBuilder.ConcreteBuilders
{
    public class DailyEventBuilder : IEventFrequencyBuilder
    {
        public UnionTE Create()
        {
            var union = new UnionTE();
            foreach(DayOfWeekEnum day in Enum.GetValues(typeof(DayOfWeekEnum)))
            {
                union.Add(new DayOfWeekTE(day));
            }
            return union;
        }
    }
}