using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScheduleWidget.Enums;

namespace ScheduleWidget.TemporalExpressions
{
    /// <summary>
    /// Expression for day in Quarter. Implements support for temporal expressions of
    /// the form: "1st quarter, 1st month, 3rd Friday" or "Last quarter, 2nd month, Fridays and Saturdays".
    /// </summary>
    public class DayInQuarterTE : TemporalExpression
    {
        int _quarter;
        int _quarterInterval;
        int _monthInterval;
        int _dayOfWeek;

        public DayInQuarterTE(
            QuarterEnum quarter,
            QuarterlyIntervalEnum quarterInterval, 
            MonthlyIntervalEnum monthInterval, 
            DayOfWeekEnum dayOfWeek)
        {
            _quarter = GetQuarterValue(quarter);
            _dayOfWeek = TEHelpers.GetDayOfWeekValue(dayOfWeek);
            _monthInterval = TEHelpers.GetMonthlyIntervalValue(monthInterval);
            _quarterInterval = GetQuarterIntervalValue(quarterInterval);
        }

        public override bool Includes(DateTime aDate)
        {
            var quarterMonthArray = BuildMonthQuarterMatrix();
            int ourMonth = (int)quarterMonthArray.GetValue(_quarter, _quarterInterval) + 1; // since our months are zero-based
            if (aDate.Month == ourMonth)
            {
                // we have a match on Month! Now just need to check day ...
                return TEHelpers.DayMatches(aDate, _dayOfWeek) && TEHelpers.WeekMatches(aDate, _monthInterval);
            }
            return false;
        }


        internal Array BuildMonthQuarterMatrix()
        {
            // create a 4x3 matrix of Month / Quarter, compare our date's place on the matrix with our settings
            // http://www.c-sharpcorner.com/UploadFile/mahesh/WorkingWithArrays11232005064036AM/WorkingWithArrays.aspx
            Array quarterMonthArray = Array.CreateInstance(typeof(Int32), 4, 3);
            int monthCounter = 0;

            for (int q = 1; q <= 4; q++) // 4 quarters
            {
                for (int m = 1; m <= 3; m++) // 3 months / quarter
                {
                    quarterMonthArray.SetValue(monthCounter, q - 1, m - 1);
                    monthCounter++;
                }
            }
            return quarterMonthArray;
        }
        
        private int GetQuarterValue(QuarterEnum quarter)
        {
            // zero-indexed for our matrix
            int q = 0;
            switch (quarter)
            {
                case QuarterEnum.First:
                    q = 0;
                    break;
                case QuarterEnum.Second:
                    q = 1;
                    break;
                case QuarterEnum.Third:
                    q = 2;
                    break;
                default:
                    q = 3;
                    break;
            }
            return q;
        }

        // quarterInterval == the month of the quarter (1st, 2nd, last)
        private int GetQuarterIntervalValue(QuarterlyIntervalEnum quarterInterval)
        {
            // zero-indexed for our matrix
            int q = 0;
            switch (quarterInterval)
            {
                case QuarterlyIntervalEnum.First:
                    q = 0;
                    break;
                case QuarterlyIntervalEnum.Second:
                    q = 1;
                    break;
                    
                default:
                    q = 2;
                    break;
            }
            return q;
        }
    }
}
