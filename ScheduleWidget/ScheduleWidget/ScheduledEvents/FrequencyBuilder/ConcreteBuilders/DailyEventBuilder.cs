using System;
using ScheduleWidget.Enums;
using ScheduleWidget.TemporalExpressions;

namespace ScheduleWidget.ScheduledEvents.FrequencyBuilder.ConcreteBuilders
{
    public class DailyEventBuilder : IEventFrequencyBuilder
    {
        private readonly Event _event;

        public DailyEventBuilder(Event aEvent)
        {
            _event = aEvent;
        }

        public UnionTE Create()
        {
            var union = new UnionTE();
            var dayIntervals = _event.DayInterval;

            if (dayIntervals > 1 && _event.FirstDateTime != null)
            {
                foreach (DayOfWeekEnum day in Enum.GetValues(typeof(DayOfWeekEnum)))
                {
                    var dayOfWeek = new DayIntervalTE(dayIntervals, (DateTime)_event.FirstDateTime);

                    union.Add(dayOfWeek);
                }
            }
            else
            {
                foreach (DayOfWeekEnum day in Enum.GetValues(typeof(DayOfWeekEnum)))
                {
                    union.Add(new DayOfWeekTE(day));
                }
            }

            return union;
        }
    }
}