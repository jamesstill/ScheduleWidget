using System;

namespace ScheduleWidget.Enums
{
    [Flags]
    public enum MonthlyIntervalEnum
    {
        First = 1,
        Second = 2,
        Third = 4,
        Fourth = 8,
        Last = 16
    }
}
