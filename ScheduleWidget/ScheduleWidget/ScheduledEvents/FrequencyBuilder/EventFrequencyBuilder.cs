using System.Diagnostics;
using ScheduleWidget.Enums;
using ScheduleWidget.ScheduledEvents.FrequencyBuilder.ConcreteBuilders;

namespace ScheduleWidget.ScheduledEvents.FrequencyBuilder
{
    public class EventFrequencyBuilder
    {
        public static IEventFrequencyBuilder Create(Event aEvent)
        {
            IEventFrequencyBuilder builder;

            switch(aEvent.FrequencyTypeOptions)
            {
                case FrequencyTypeEnum.None:
                    builder = new OneTimeEventBuilder(aEvent);
                    break;

                case FrequencyTypeEnum.Daily:
                    // if the event is using FirstDateTime then return a builder for that
                    // otherwise return the simpler builder for backward compatibility
                    if (aEvent.FirstDateTime.HasValue)
                        builder = new DailyEventWithFirstDateTimeBuilder(aEvent);
                    else
                        builder = new DailyEventBuilder();
                    break;

                case FrequencyTypeEnum.Weekly:
                case FrequencyTypeEnum.EveryWeekDay:
                case FrequencyTypeEnum.EveryMonWedFri:
                case FrequencyTypeEnum.EveryTuTh:
                    builder = new WeeklyEventBuilder(aEvent);
                    break;

                case FrequencyTypeEnum.Monthly:
                    builder = new MonthlyEventBuilder(aEvent);
                    break;

                case FrequencyTypeEnum.Quarterly:
                    builder = new QuarterlyEventBuilder(aEvent);
                    break;

                case FrequencyTypeEnum.Yearly:
                    builder = new YearlyEventBuilder(aEvent);
                    break;

                default:
                    Trace.TraceError("Unknown frequency type '{0}'", aEvent.FrequencyTypeOptions);
                    builder = null;
                    break;
            }

            return builder;
        }
    }
}
