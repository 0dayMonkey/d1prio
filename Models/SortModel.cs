namespace ApiTools.Models
{
    public class SortModel
    {
        public string Active { get; set; }
        public SortDirection Direction { get; set; }

        public SortModel(string active, SortDirection direction = SortDirection.desc)
        {
            Active = active;
            Direction = direction;
        }
    }
}
