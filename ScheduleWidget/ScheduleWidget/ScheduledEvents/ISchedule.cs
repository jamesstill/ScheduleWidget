using System;
using System.Collections.Generic;

namespace ScheduleWidget.ScheduledEvents
{
    public interface ISchedule
    {
        bool IsOccurring(DateTime aDate);
        DateTime? PreviousOccurrence(DateTime aDate);
        DateTime? PreviousOccurrence(DateTime aDate, DateRange during);
        DateTime? NextOccurrence(DateTime aDate);
        DateTime? NextOccurrence(DateTime aDate, DateRange during);
        IEnumerable<DateTime> Occurrences(DateRange during);
    }
}
