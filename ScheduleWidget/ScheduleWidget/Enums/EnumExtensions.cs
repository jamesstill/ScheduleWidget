using System;
using System.Collections.Generic;

namespace ScheduleWidget.Enums
{
    internal static class EnumExtensions
    {
        public const int MonthlyIntervalMaxBitField = 31;
        public const int FrequencyTypeMaxBitField = 7;
        public const int DayOfWeekMaxBitField = 127;

        public static IEnumerable<Enum> GetFlags(Enum input)
        {
            foreach (Enum value in Enum.GetValues(input.GetType()))
                if (input.HasFlag(value))
                    yield return value;
        }
    }
}
