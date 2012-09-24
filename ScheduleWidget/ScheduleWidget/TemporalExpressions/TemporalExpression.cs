using System;

namespace ScheduleWidget.TemporalExpressions
{
    public abstract class TemporalExpression
    {
        public abstract bool Includes(DateTime aDate);

        public static int GetWeekInMonth(int dayNumber)
        {
            var value = ((dayNumber - 1) / 7) + 1;
            return value;
        }

        public static int GetDaysLeftInMonth(DateTime aDate)
        {
            var actualMaximum = DateTime.DaysInMonth(aDate.Year, aDate.Month);
            return actualMaximum - aDate.Day;
        }

        public static int GetDayOfWeek(DateTime aDate)
        {
            return (int)aDate.DayOfWeek;
        }
    }
}
