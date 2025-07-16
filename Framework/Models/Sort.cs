using System.ComponentModel;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Models
{
    public class Sort<T>
    {
        private static readonly MethodInfo OrderByMethod =
            typeof(Queryable).GetMethods().Single(method =>
            method.Name == "OrderBy" && method.GetParameters().Length == 2);

        private static readonly MethodInfo OrderByDescendingMethod =
            typeof(Queryable).GetMethods().Single(method =>
            method.Name == "OrderByDescending" && method.GetParameters().Length == 2);

        private static readonly MethodInfo ThenByMethod =
            typeof(Queryable).GetMethods().Single(method =>
            method.Name == "ThenBy" && method.GetParameters().Length == 2);

        private static readonly MethodInfo ThenByDescendingMethod =
            typeof(Queryable).GetMethods().Single(method =>
            method.Name == "ThenByDescending" && method.GetParameters().Length == 2);

        public Expression<Func<T, object>> Expression { get; set; }
        public ListSortDirection Direction { get; set; }
        public int Index { get; private set; }

        public MethodInfo Method
        {
            get
            {
                return Direction == ListSortDirection.Descending
                    ? Index == 0 ? OrderByDescendingMethod : ThenByDescendingMethod
                    : Index == 0 ? OrderByMethod : ThenByMethod;
            }
        }

        public Sort() { }

        public Sort(string fieldName, ListSortDirection direction, int index, string param)
        {
            Index = index;
            Direction = direction;
            Expression = (Expression<Func<T, object>>)DynamicExpressionParser.ParseLambda(
                new[] { System.Linq.Expressions.Expression.Parameter(typeof(T), param) },
                typeof(object),
                $"{param}.{fieldName}");
        }

        public Sort(string fieldName, ListSortDirection direction, int index) : this(fieldName, direction, index, "x") { }

    }
}
