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
            //Assigning default value to day interval if the value is 0.
            if (aEvent.DayInterval == 0)
                aEvent.DayInterval = 1;

            _event = aEvent;
        }

        public UnionTE Create()
        {
            var union = new UnionTE();

            if (_event.DayInterval > 1)
            {
                var firstDateTime = _event.FirstDateTime ?? DateTime.Now;

                foreach (DayOfWeekEnum day in Enum.GetValues(typeof(DayOfWeekEnum)))
                {
                    union.Add(new DayIntervalTE(_event.DayInterval, firstDateTime));
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