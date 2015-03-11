using System;
using System.Linq;
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
            var firstDayOfWeek = EnumExtensions.GetDayOfWeek(_event.FirstDayOfWeek);
            var weeklyIntervals = _event.RepeatInterval;
            if (weeklyIntervals > 0 && _event.StartDateTime != null)
            {
                foreach (DayOfWeekEnum day in daysOfWeek.Cast<DayOfWeekEnum>().OrderBy(e => e, new DayOfWeekEnumComparer(_event.FirstDayOfWeek)))
                {
                    var dayOfWeek = new DayInWeekTE(day, firstDayOfWeek, (DateTime)_event.StartDateTime, weeklyIntervals);
                    union.Add(dayOfWeek);
                }
            }
            else
            {
                foreach (DayOfWeekEnum day in daysOfWeek.Cast<DayOfWeekEnum>().OrderBy(e => e, new DayOfWeekEnumComparer(_event.FirstDayOfWeek)))
                {
                    var dayOfWeek = new DayOfWeekTE(day);
                    union.Add(dayOfWeek);
                }
            }

            return union;
        }
    }
}
