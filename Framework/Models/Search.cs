namespace Framework.Models
{
    public class Search<T>
    {
        public ulong? First { get; set; }

        public ulong? Rows { get; set; }

        public Filter<T>? Filter { get; set; }

        public Sort<T>[]? Sorts { get; set; }
    }
}
