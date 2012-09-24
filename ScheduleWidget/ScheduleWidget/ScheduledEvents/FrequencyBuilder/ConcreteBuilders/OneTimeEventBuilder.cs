using System;
using ScheduleWidget.Enums;
using ScheduleWidget.TemporalExpressions;

namespace ScheduleWidget.ScheduledEvents.FrequencyBuilder.ConcreteBuilders
{
    public class OneTimeEventBuilder : IEventFrequencyBuilder
    {
        private readonly Event _event;

        public OneTimeEventBuilder(Event aEvent)
        {
            _event = aEvent;
        }

        /// <summary>
        /// No frequency means a one-time only event
        /// </summary>
        /// <returns></returns>
        public UnionTE Create()
        {
            var union = new UnionTE();
            if (_event.FrequencyTypeOptions == FrequencyTypeEnum.None)
            {
                if (_event.OneTimeOnlyEventDate.HasValue)
                {
                    union.Add(new DateTE(_event.OneTimeOnlyEventDate.Value));
                }
            }
            return union;
        }
    }
}
