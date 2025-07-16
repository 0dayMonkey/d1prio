using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace ApiTools.Models
{
    public class SearchModel
    {
        /// <summary>
        /// First item 0 based index
        /// </summary>
        public ulong? First { get; set; }
        
        /// <summary>
        /// Number of rows to retrieve
        /// </summary>
        public ulong? Rows { get; set; }
        
        /// <summary>
        /// Sorts
        /// </summary>
        public List<SortModel>? Sorts { get; set; }

        /// <summary>
        /// Filters
        /// </summary>
        public List<FilterModel>? Filters { get; set; }

        public SearchModel(ulong? first, ulong? rows, List<SortModel>? sorts, List<FilterModel>? filters)
        {
            First = first;
            Rows = rows;
            Sorts = sorts;
            Filters = filters;
        }

        public string GetPredicateString(string? param = null)
        {            
            return FilterModel.GetPredicateString(Filters, param);
        }
    }
}
