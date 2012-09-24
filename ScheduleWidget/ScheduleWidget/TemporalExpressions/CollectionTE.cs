using System.Collections.Generic;

namespace ScheduleWidget.TemporalExpressions
{
    public abstract class CollectionTE : TemporalExpression
    {
        protected CollectionTE()
        {
            Expressions = new List<TemporalExpression>();
        }

        public ICollection<TemporalExpression> Expressions { get; set; }

        /// <summary>
        /// Adds a temporal expression to the list
        /// </summary>
        /// <param name="expr">Temporal expression to add.</param>
        public void Add(TemporalExpression expr)
        {
            Expressions.Add(expr);
        }
    }
}
