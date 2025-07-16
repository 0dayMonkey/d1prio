using System.Collections.Generic;

namespace ApiTools.Models
{
    public class SearchResult<T>
    {
        /// <summary>
        /// First item 0 based index
        /// </summary>
        public ulong? First { get; set; }

        /// <summary>
        /// Number of retieved rows
        /// </summary>
        public ulong? Rows { get; set; }

        /// <summary>
        /// Total number of items
        /// </summary>
        public ulong TotalItems { get; set; }

        /// <summary>
        /// Items
        /// </summary>
        public List<T>? Items { get; set; }
    }
}
