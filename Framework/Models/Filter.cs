using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Framework.Models
{
    public class Filter<T>
    {
        public Expression<Func<T, bool>> Expression { get; set; }

        public Filter() { }

        public Filter(string filter, string param)
        {
            Expression = (Expression<Func<T, bool>>)DynamicExpressionParser.ParseLambda(new[] { System.Linq.Expressions.Expression.Parameter(typeof(T), param) }, typeof(bool), filter);
        }

        public Filter(string filter) : this(filter, "x") { }

    }
}
