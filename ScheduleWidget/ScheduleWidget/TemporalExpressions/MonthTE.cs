using System;
using ScheduleWidget.Enums;

namespace ScheduleWidget.TemporalExpressions
{
    /// <summary>
    /// Expression for 'Monthly' frequency type.
    /// </summary>
    public class MonthTE : TemporalExpression
    {
        private enum eMonthIntervalType
        {
            None = 0,
            EachWeekSelectedWeekDays = 1,
            SelectedDate = 2,
            SelectedWeekSelectedWeekDays = 3
        }

        private readonly int _monthIntervals;
        private readonly DateTime _firstDateTime;
        private readonly int _date;
        private readonly eMonthIntervalType _monthIntervalType;

        internal protected readonly int _dayOfWeek;
        internal protected readonly int _monthlyInterval;

        private MonthTE() { }

        /// <summary>
        /// Every n month(s), every week, selected week day(s)
        /// </summary>
        /// <param name="monthIntervals">Month interval. E.g., Every 1 month, every 2 months, .... , every n months.</param>
        /// <param name="firstDateTime">Used when monthIntervals > 1 to check a month that comes under expected month interval.</param>
        /// <param name="dayOfWeek">Day(s) of the week</param>
        public MonthTE(int monthIntervals, DateTime firstDateTime, DayOfWeekEnum dayOfWeek)
        {
            _monthIntervals = monthIntervals;
            _firstDateTime = firstDateTime.Date;
            _dayOfWeek = TEHelpers.GetDayOfWeekValue(dayOfWeek);
            _monthIntervalType = eMonthIntervalType.EachWeekSelectedWeekDays;
        }

        /// <summary>
        /// Every n month(s), one selected date
        /// </summary>
        /// <param name="monthIntervals">Month interval. E.g., Every 1 month, every 2 months, .... , every n months.</param>
        /// <param name="firstDateTime">Used when monthIntervals > 1 to check a month that comes under expected month interval.</param>
        /// <param name="date">Day of the month</param>
        public MonthTE(int monthIntervals, DateTime firstDateTime, int date)
        {
            _monthIntervals = monthIntervals;
            _firstDateTime = firstDateTime.Date;
            _date = date;
            _monthIntervalType = eMonthIntervalType.SelectedDate;
        }

        /// <summary>
        /// Every n month(s), selected week(s), selected week day(s)
        /// </summary>
        /// <param name="monthIntervals">Month interval. E.g., Every 1 month, every 2 months, .... , every n months.</param>
        /// <param name="firstDateTime">Used when monthIntervals > 1 to check a month that comes under expected month interval.</param>
        /// <param name="dayOfWeek">Day(s) of the week</param>
        /// <param name="monthlyInterval">Week numbers. E.g., first week, second week, every week, second and third week, etc.</param>
        public MonthTE(int monthIntervals, DateTime firstDateTime, DayOfWeekEnum dayOfWeek, MonthlyIntervalEnum monthlyInterval)
        {
            _monthIntervals = monthIntervals;
            _firstDateTime = firstDateTime.Date;
            _dayOfWeek = TEHelpers.GetDayOfWeekValue(dayOfWeek);
            _monthlyInterval = TEHelpers.GetMonthlyIntervalValue(monthlyInterval);
            _monthIntervalType = eMonthIntervalType.SelectedWeekSelectedWeekDays;
        }

        public override int GetHashCode()
        {
            return _monthlyInterval ^ _dayOfWeek ^ _date;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var d = obj as DayInMonthTE;
            if ((object)d == null)
                return false;

            return (_monthlyInterval == d._monthlyInterval) && (_dayOfWeek == d._dayOfWeek) && (_date == d._date);
        }

        public override bool Includes(DateTime aDate)
        {
            if (_monthIntervalType == eMonthIntervalType.EachWeekSelectedWeekDays)
            {
                return IntervalMatches(aDate) && TEHelpers.DayOfWeekMatches(aDate, _dayOfWeek);
            }
            else if (_monthIntervalType == eMonthIntervalType.SelectedDate)
            {
                return IntervalMatches(aDate) && TEHelpers.DayMatches(aDate, _date);
            }
            else
            {
                return IntervalMatches(aDate) && TEHelpers.DayOfWeekMatches(aDate, _dayOfWeek) && TEHelpers.WeekMatches(aDate, _monthlyInterval);
            }
        }

        protected bool IntervalMatches(DateTime aDate)
        {
            return _monthIntervals > 1
                   ? ((int)Math.Abs(aDate.Month - _firstDateTime.Month)) % _monthIntervals == 0
                   : true;
        }
    }
}
