using System;

namespace ScheduleWidget.TemporalExpressions
{
    public class DifferenceTE : TemporalExpression
    {
        private readonly TemporalExpression _inclusiveExpression;
        private readonly TemporalExpression _exclusiveExpression;

        public DifferenceTE(TemporalExpression inclusiveExpression, TemporalExpression exclusiveExpression)
        {
            _inclusiveExpression = inclusiveExpression;
            _exclusiveExpression = exclusiveExpression;
        }

        /// <summary>
        /// Returns true if the inclusive expression is true and the exclusive expression is false
        /// </summary>
        /// <param name="aDate"></param>
        /// <returns></returns>
        public override bool Includes(DateTime aDate)
        {
            return (_inclusiveExpression.Includes(aDate) && !_exclusiveExpression.Includes(aDate));
        }
    }
}