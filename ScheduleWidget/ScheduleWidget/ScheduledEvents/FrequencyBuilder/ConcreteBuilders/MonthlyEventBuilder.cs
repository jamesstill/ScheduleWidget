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
            if (aEvent.MonthInterval == 0)
                aEvent.MonthInterval = 1;

            _event = aEvent;
        }

        public UnionTE Create()
        {
            var union = new UnionTE();
            var firstDateTime = _event.FirstDateTime ?? DateTime.Now;

            if (_event.DayOfMonth == 0 && (int)_event.MonthlyIntervalOptions == 0)
                _event.MonthlyIntervalOptions = MonthlyIntervalEnum.EveryWeek;

            if (_event.DayOfMonth > 0)
            {
                var dayInMonth = new MonthTE(_event.MonthInterval, firstDateTime, _event.DayOfMonth);
                union.Add(dayInMonth);
            }
            else if (_event.MonthlyInterval == (int)MonthlyIntervalEnum.EveryWeek)
            {
                var daysOfWeek = EnumExtensions.GetFlags(_event.DaysOfWeekOptions);
                foreach (DayOfWeekEnum dayOfWeek in daysOfWeek)
                {
                    var dayInMonth = new MonthTE(_event.MonthInterval, firstDateTime, dayOfWeek);
                    union.Add(dayInMonth);
                }
            }
            else
            {
                var monthlyIntervals = EnumExtensions.GetFlags(_event.MonthlyIntervalOptions);
                var daysOfWeek = EnumExtensions.GetFlags(_event.DaysOfWeekOptions);
                foreach (MonthlyIntervalEnum monthlyInterval in monthlyIntervals)
                {
                    foreach (DayOfWeekEnum dayOfWeek in daysOfWeek)
                    {
                        var dayInMonth = new MonthTE(_event.MonthInterval, firstDateTime, dayOfWeek, monthlyInterval);
                        union.Add(dayInMonth);
                    }
                }
            }

            return union;
        }
    }
}
