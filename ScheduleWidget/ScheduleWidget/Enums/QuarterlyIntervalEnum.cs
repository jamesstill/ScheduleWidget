using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleWidget.Enums
{
    /// <summary>
    /// The month(s) in which a quarterly event recurs - First / Second / Last month of the quarter
    /// </summary>
    [Flags]
    public enum QuarterlyIntervalEnum
    {
        First = 1,
        Second = 2,
        Last = 4
    }
}
