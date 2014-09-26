using System;
using ScheduleWidget.Enums;
using ScheduleWidget.TemporalExpressions;

namespace ScheduleWidget.ScheduledEvents.FrequencyBuilder.ConcreteBuilders
{
    public class YearlyEventBuilder : IEventFrequencyBuilder
    {
        private readonly Event _event;

        public YearlyEventBuilder(Event aEvent)
        {
            //Assigning default value to year interval if the value is 0.
            if (aEvent.YearInterval == 0)
                aEvent.YearInterval = 1;

            _event = aEvent;
        }

        /// <summary>
        /// On yearly frequency build an anniversary temporal expression
        /// </summary>
        /// <returns></returns>
        public UnionTE Create()
        {
            var union = new UnionTE();
            if (_event.FrequencyTypeOptions == FrequencyTypeEnum.Yearly)
            {
                if (_event.Anniversary == null)
                {
                    throw new ApplicationException("Events with a yearly frequency requires an anniversary.");
                }

                union.Add(new YearTE(_event.YearInterval, _event.FirstDateTime.HasValue ? _event.FirstDateTime.Value.Year : DateTime.Now.Year, _event.Anniversary.Month, _event.Anniversary.Day));
            }
            return union;
        }
    }
}
