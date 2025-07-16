using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace ApiTools.Models
{
    public static class PagingExtension
    {
        public static PagedResult<T> PagedBy<T>(this IQueryable<T> query, ulong? pageIndex, ulong? ulPageSize) where T : class
        {
            int page = pageIndex == null ? 1 : (int)(pageIndex + 1);
            int rowCount = query.Count();
            int pageSize = (int)(ulPageSize == null || ulPageSize < 1 ? 25 : ulPageSize);

            return new()
            {
                CurrentPage = page,
                PageCount = (int)Math.Ceiling((double)rowCount / pageSize),
                PageSize = pageSize,
                Queryable = query.Skip((page - 1) * pageSize).Take(pageSize),
                RowCount = rowCount,
            };
        }

        public static PagedResult<T> SearchBy<T>(this IQueryable<T> query, SearchModel search) where T : class
            => query
                .Where(search.GetPredicateString())
                .OrderBy(search.Sorts)
                .PagedBy(search.First / search.Rows, search.Rows);
    }
}
