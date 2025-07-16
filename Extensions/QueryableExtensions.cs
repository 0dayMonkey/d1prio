using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace ApiTools.Models
{
    public static partial class QueryableExtensions
    {
        public static IQueryable<T> FilterBy<T>(this IQueryable<T> source, IEnumerable<FilterModel>? filters)
        {
            if (filters?.Any() != true)
                return source;

            var predicate = filters.Select(filter => filter.CreateClauses())
                .Where(clause => !string.IsNullOrEmpty(clause.condition))
                .Aggregate(
                    (orOperator: true, condition: string.Empty),
                    (clause, next) => (
                        next.orOperator,
                        $"{(clause.condition == string.Empty ? string.Empty : $"{clause.condition} {(next.orOperator ? "OR" : "AND")} ")}{next.condition}"
                    ),
                    clause => clause.condition == string.Empty ? string.Empty : $"({clause.condition})"
                 );

            if (string.IsNullOrEmpty(predicate))
                return source;

            return source.Where(predicate);

        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, IEnumerable<SortModel>? sorts)
        {
            Expression expression = source.Expression;
            int count = 0;

            if (sorts == null)
                return source;

            foreach (SortModel item in sorts)
            {
                ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
                Expression selector;

                Expression body = parameter;
                foreach (string member in item.Active.Split('.'))
                {
                    body = Expression.PropertyOrField(body, member);
                }
                selector = body;

                string method = item.Direction == SortDirection.desc ?
                    (count == 0 ? "OrderByDescending" : "ThenByDescending") :
                    (count == 0 ? "OrderBy" : "ThenBy");
                expression = Expression.Call(typeof(Queryable), method,
                    new Type[] { source.ElementType, selector.Type },
                    expression, Expression.Quote(Expression.Lambda(selector, parameter)));
                count++;
            }

            return count > 0 ? source.Provider.CreateQuery<T>(expression) : source;
        }
    }
}
