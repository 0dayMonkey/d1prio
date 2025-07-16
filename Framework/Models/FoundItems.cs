namespace Framework.Models
{
    public class FoundItems<T>
    {
        public ulong First { get; set; }
        public ulong TotalItems { get; set; }
        public IQueryable<T> Items { get; set; }
    }
}
