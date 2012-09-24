using System;
using ScheduleWidget.Enums;
using ScheduleWidget.TemporalExpressions;

namespace ScheduleWidget.ScheduledEvents.FrequencyBuilder.ConcreteBuilders
{
    public class WeeklyEventBuilder : IEventFrequencyBuilder
    {
        private readonly Event _event;

        public WeeklyEventBuilder(Event aEvent)
        {
            _event = aEvent;
        }

        public UnionTE Create()
        {
            var union = new UnionTE();
            var daysOfWeek = EnumExtensions.GetFlags(_event.DaysOfWeekOptions);
            foreach (DayOfWeekEnum day in daysOfWeek)
            {
                var dayOfWeek = new DayOfWeekTE(day);
                union.Add(dayOfWeek);
            }
            return union;
        }
    }
}
