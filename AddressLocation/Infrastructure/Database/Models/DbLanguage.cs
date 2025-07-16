namespace AddressLocation.Infrastructure.Database.Models
{
    public partial class DbLanguage
    {
        public DbLanguage()
        {
        }

        public string Id { get; set; } = null!;
        public string LongLabel { get; set; } = null!;
        public string ShortLabel { get; set; } = null!;
        public DateTime? LastUpdatedTimestamp { get; set; }
    }
}
