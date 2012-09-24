using System;
using System.Collections.Generic;

namespace ScheduleWidget.ScheduledEvents
{
    public interface ISchedule
    {
        bool IsOccurring(DateTime aDate);
        DateTime? PreviousOccurrence(DateTime aDate);
        DateTime? NextOccurrence(DateTime aDate);
        IEnumerable<DateTime> Occurrences(DateRange during);
    }
}
