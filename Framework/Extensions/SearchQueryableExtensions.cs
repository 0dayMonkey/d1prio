using Framework.Models;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace Framework.Extensions
{
    public static class SearchQueryableExtensions
    {
        public static IQueryable<T> SortBy<T>(this IQueryable<T> source, Sort<T> sort)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            MethodInfo genericMethod = sort.Method.MakeGenericMethod(typeof(T), typeof(object));
            return (IQueryable<T>)genericMethod.Invoke(null, new object[] { source, sort.Expression });
        }

        public static IQueryable<T> SortBy<T>(this IQueryable<T> source, IEnumerable<Sort<T>>? sorts)
        {
            if (source == null) 
                throw new ArgumentNullException("source");
            if (sorts == null)
                return source;

            var res = source;
            foreach (var sort in sorts.OrderBy(x => x.Index))
            {
                res = res.SortBy(sort);
            }
            return res;
        }

        private static FoundItems<T> SearchPart<T>(this IQueryable<T> source, ulong? first, ulong? rows) where T : class
        {
            if (source == null)
                throw new ArgumentNullException("source");

            int localFirst = (int)(first ?? 0);
            int localRows = (int)(rows ?? 25);
            int totalCount = source.Count();
            var result = source.Skip(localFirst).Take(localRows);

            return new FoundItems<T>
            {
                First = (ulong)localFirst,
                TotalItems = (ulong)totalCount,
                Items = result,
            };
        }

        public static FoundItems<T> SearchBy<T>(this IQueryable<T> query, Search<T> search) where T : class
            => query
                .Where(search.Filter == null ? (x => true) : search.Filter.Expression)
                .SortBy(search.Sorts)
                .SearchPart(search.First, search.Rows);
    }
}
