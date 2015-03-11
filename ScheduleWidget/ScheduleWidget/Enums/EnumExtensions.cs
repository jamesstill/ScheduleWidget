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

		public static DayOfWeek GetDayOfWeek(DayOfWeekEnum dayOfWeek)
		{
			switch (dayOfWeek)
			{
				case DayOfWeekEnum.Sun:
					return DayOfWeek.Sunday;
				case DayOfWeekEnum.Mon:
					return DayOfWeek.Monday;
				case DayOfWeekEnum.Tue:
					return DayOfWeek.Tuesday;
				case DayOfWeekEnum.Wed:
					return DayOfWeek.Wednesday;
				case DayOfWeekEnum.Thu:
					return DayOfWeek.Thursday;
				case DayOfWeekEnum.Fri:
					return DayOfWeek.Friday;
				//case DayOfWeekEnum.Sat:
				//	return DayOfWeek.Saturday;
			}
			return DayOfWeek.Sunday;
		}
	}
}
