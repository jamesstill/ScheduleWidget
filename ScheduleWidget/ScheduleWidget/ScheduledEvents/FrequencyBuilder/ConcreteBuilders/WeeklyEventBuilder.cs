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
            var weeklyIntervals = _event.WeeklyIntervalOptions;
            if (weeklyIntervals > 0 && _event.FirstDateTime != null)
            {
                foreach (DayOfWeekEnum day in daysOfWeek)
                {
                    var dayOfWeek = new DayInWeekTE(day, (DateTime)_event.FirstDateTime, weeklyIntervals);
                    union.Add(dayOfWeek);
                }
            }
            else
            {
                foreach (DayOfWeekEnum day in daysOfWeek)
                {
                    var dayOfWeek = new DayOfWeekTE(day);
                    union.Add(dayOfWeek);
                }
            }

            return union;
        }
    }
}
