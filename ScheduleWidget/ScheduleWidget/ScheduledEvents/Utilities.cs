using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleWidget.ScheduledEvents
{
    static public class Utilities
    {

        /// <summary>
        /// DateTime.SafeAddDays,
        /// This is an extension method.
        /// Adds days to a date time object without the possibility of generating an out of range exception.
        /// This is useful when the DateTime object might be set to minimum or maximum value.
        /// </summary>
        /// <returns>The new date time object.</returns>
        static public DateTime SafeAddDays(this DateTime initialValue, double days)
        {
            try { return initialValue.AddDays(days); }
            catch (Exception) { return (days > 0) ? DateTime.MaxValue: DateTime.MinValue; }
        }
        /// <summary>
        /// DateTime.SafeAddYears,
        /// This is an extension method.
        /// Adds years to a date time object without the possibility of generating an out of range exception.
        /// This is useful when the DateTime object might be set to minimum or maximum value.
        /// </summary>
        /// <returns>The new date time object.</returns>
        static public DateTime SafeAddYears(this DateTime initialValue, int years)
        {
            try { return initialValue.AddYears(years); }
            catch (Exception) { return (years > 0) ? DateTime.MaxValue : DateTime.MinValue; ; }
        }
    }
}
