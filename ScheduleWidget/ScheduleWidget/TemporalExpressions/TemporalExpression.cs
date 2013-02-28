using System;
using ScheduleWidget.Enums;

namespace ScheduleWidget.TemporalExpressions
{
    public abstract class TemporalExpression
    {
        public abstract bool Includes(DateTime aDate);
    }
}
