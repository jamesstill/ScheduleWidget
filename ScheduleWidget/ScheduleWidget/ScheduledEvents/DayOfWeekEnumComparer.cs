using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleWidget.Enums;

namespace ScheduleWidget.ScheduledEvents
{
	public class DayOfWeekEnumComparer : IComparer<DayOfWeekEnum>
	{
		public static int Rank(DayOfWeekEnum firstDayOfWeek, DayOfWeekEnum dayOfWeek)
		{
			return (int)dayOfWeek + (dayOfWeek < firstDayOfWeek ? EnumExtensions.DayOfWeekMaxBitField + 1 : 0);
		}

		public static int Compare(DayOfWeekEnum firstDayOfWeek, DayOfWeekEnum x, DayOfWeekEnum y)
		{
			return Rank(firstDayOfWeek, x).CompareTo(Rank(firstDayOfWeek, y));
		}

		DayOfWeekEnum firstDayOfWeek;
		public DayOfWeekEnumComparer(DayOfWeekEnum firstDayOfWeek)
		{
			this.firstDayOfWeek = firstDayOfWeek;
		}

		public int Compare(DayOfWeekEnum x, DayOfWeekEnum y)
		{
			return DayOfWeekEnumComparer.Compare(this.firstDayOfWeek, x, y);
		}
	}
}
