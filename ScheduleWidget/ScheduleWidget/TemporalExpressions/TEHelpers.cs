using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScheduleWidget.Enums;

namespace ScheduleWidget.TemporalExpressions
{
    internal static class TEHelpers
    {
        internal static int GetWeekInMonth(int dayNumber)
        {
            var value = ((dayNumber - 1) / 7) + 1;
            return value;
        }

        internal static int GetDaysLeftInMonth(DateTime aDate)
        {
            var actualMaximum = DateTime.DaysInMonth(aDate.Year, aDate.Month);
            return actualMaximum - aDate.Day;
        }

        internal static int GetDayOfWeek(DateTime aDate)
        {
            return (int)aDate.DayOfWeek;
        }

        internal static int GetDayOfWeekValue(DayOfWeekEnum dayOfWeekOption)
        {
            switch (dayOfWeekOption)
            {
                case DayOfWeekEnum.Sun:
                    return 0;

                case DayOfWeekEnum.Mon:
                    return 1;

                case DayOfWeekEnum.Tue:
                    return 2;

                case DayOfWeekEnum.Wed:
                    return 3;

                case DayOfWeekEnum.Thu:
                    return 4;

                case DayOfWeekEnum.Fri:
                    return 5;

                case DayOfWeekEnum.Sat:
                    return 6;

                default:
                    return 0;
            }
        }

        internal static int GetMonthlyIntervalValue(MonthlyIntervalEnum monthlyIntervalOption)
        {
            switch (monthlyIntervalOption)
            {
                case MonthlyIntervalEnum.First:
                    return 1;

                case MonthlyIntervalEnum.Second:
                    return 2;

                case MonthlyIntervalEnum.Third:
                    return 3;

                case MonthlyIntervalEnum.Fourth:
                    return 4;

                case MonthlyIntervalEnum.Last:
                    return -1;

                default:
                    return 0;
            }
        }

        internal static bool DayMatches(DateTime aDate, int dayOfWeek)
        {
            return GetDayOfWeek(aDate) == dayOfWeek;
        }

        internal static bool WeekMatches(DateTime aDate, int monthlyInterval)
        {
            if (monthlyInterval > 0)
            {
                return WeekFromStartMatches(aDate, monthlyInterval);
            }

            return WeekFromEndMatches(aDate, monthlyInterval);
        }

        private static bool WeekFromStartMatches(DateTime aDate, int monthlyInterval)
        {
            var week = GetWeekInMonth(aDate.Day);
            return (week == monthlyInterval);
        }

        private static bool WeekFromEndMatches(DateTime aDate, int monthlyInterval)
        {
            var daysFromMonthEnd = GetDaysLeftInMonth(aDate) + 1;
            return GetWeekInMonth(daysFromMonthEnd) == Math.Abs(monthlyInterval);
        }

    }
}
