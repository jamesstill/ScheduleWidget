using ScheduleWidget.TemporalExpressions;

namespace ScheduleWidget.ScheduledEvents.FrequencyBuilder
{
    public interface IEventFrequencyBuilder
    {
        UnionTE Create();
    }
}
