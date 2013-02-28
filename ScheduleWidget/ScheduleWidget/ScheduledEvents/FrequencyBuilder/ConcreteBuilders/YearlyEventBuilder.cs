using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public UnionTE Create()
        {
            throw new NotImplementedException();
        }
    }
}
