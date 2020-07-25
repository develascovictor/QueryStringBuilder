using System.Collections.Generic;
using System.Linq.Expressions;

namespace Expressions.Helpers
{
    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            var parameterRebinder = new ParameterRebinder(map);
            var expression = parameterRebinder.Visit(exp);

            return expression;
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            if (_map.TryGetValue(p, out var replacement))
            {
                p = replacement;
            }

            var expression = base.VisitParameter(p);
            return expression;
        }
    }
}