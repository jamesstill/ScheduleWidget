using System;

namespace ScheduleWidget.Enums
{
    /// <summary>
    /// The week(s) in which a monthly event recurs, e.g. every 3rd week, or the last week, or the first and second weeks
    /// </summary>
    [Flags]
    public enum MonthlyIntervalEnum
    {
        First = 1,
        Second = 2,
        Third = 4,
        Fourth = 8,
        Last = 16,
        EveryWeek = First | Second | Third | Fourth | Last
    }
}
