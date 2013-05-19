using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleWidget.Enums
{
    /// <summary>
    /// The actual Quarter (not interval!) the event occurs in - First, second, Third or Fourth.
    /// </summary>
    [Flags]
    public enum QuarterEnum
    {
        //None = 0,
        First = 1,
        Second = 2,
        Third = 4,
        Fourth = 8,
    }
}
