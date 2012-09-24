using System;
using System.Linq;

namespace ScheduleWidget.TemporalExpressions
{
    public class UnionTE : CollectionTE
    {
        /// <summary>
        /// Returns true if the date is included anywhere in the expression
        /// </summary>
        /// <param name="aDate"></param>
        /// <returns></returns>
        public override bool Includes(DateTime aDate)
        {
            return Expressions.Any(e => e.Includes(aDate));
        }
    }
}
