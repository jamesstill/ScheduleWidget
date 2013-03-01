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

                union.Add(new AnniversaryTE(_event.Anniversary.Month, _event.Anniversary.Day));
            }
            return union;
        }
    }
}
