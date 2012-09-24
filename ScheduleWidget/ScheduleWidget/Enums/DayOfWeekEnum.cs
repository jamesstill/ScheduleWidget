using System;

namespace ScheduleWidget.Enums
{
    [Flags]
    public enum DayOfWeekEnum
    {
        Sun = 1,
        Mon = 2,
        Tue = 4,
        Wed = 8,
        Thu = 16,
        Fri = 32,
        Sat = 64
    }
}
