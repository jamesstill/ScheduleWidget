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
            if (aEvent.RepeatInterval == 0)
                aEvent.RepeatInterval = 1;

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

                if (_event.Anniversary.Day < 1 || _event.Anniversary.Day > 31 ||
                    _event.Anniversary.Month < 1 || _event.Anniversary.Month > 12)
                    throw new ApplicationException("The anniversary month and day are invalid.");


                if (_event.RepeatInterval > 1 && _event.StartDateTime == null)
                    throw new ApplicationException("Events with a yearly repeat interval greater than one, require a start date.");

                // Find the proper start year for the year temporal event, based on the event start date.
                int year = DateTime.Now.Year;
                if (_event.StartDateTime != null)
                {
                    DateTime startDate = (DateTime)_event.StartDateTime;
                    if (startDate.Month == _event.Anniversary.Month &&
                        startDate.Day > _event.Anniversary.Day)
                        startDate = startDate.AddMonths(1);
                    year = (startDate.Month > _event.Anniversary.Month) ?
                        startDate.Year + 1 : startDate.Year;
                }

                union.Add(new YearTE(_event.RepeatInterval, year, _event.Anniversary.Month, _event.Anniversary.Day));
            }
            return union;
        }
    }
}
