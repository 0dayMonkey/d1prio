using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AddressLocation.Infrastructure.Database.Models
{
    public partial class DbAddressLevelLabel
    {
        public DbAddressLevelLabel()
        {
            AddressLevelStructureAsAbbreviation1 = new HashSet<DbAddressLevelStructure>();
            AddressLevelStructureAsAbbreviation2 = new HashSet<DbAddressLevelStructure>();
            AddressLevelStructureAsAbbreviation3 = new HashSet<DbAddressLevelStructure>();

            AddressLevelStructureAsDescription1 = new HashSet<DbAddressLevelStructure>();
            AddressLevelStructureAsDescription2 = new HashSet<DbAddressLevelStructure>();
            AddressLevelStructureAsDescription3 = new HashSet<DbAddressLevelStructure>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal? Id { get; set; }
        public string Label { get; set; }
        public DateTime? LastUpdatedTimestamp { get; set; }

        public virtual ICollection<DbAddressLevelStructure>? AddressLevelStructureAsAbbreviation1 { get; set; }
        public virtual ICollection<DbAddressLevelStructure>? AddressLevelStructureAsAbbreviation2 { get; set; }
        public virtual ICollection<DbAddressLevelStructure>? AddressLevelStructureAsAbbreviation3 { get; set; }

        public virtual ICollection<DbAddressLevelStructure>? AddressLevelStructureAsDescription1 { get; set; }
        public virtual ICollection<DbAddressLevelStructure>? AddressLevelStructureAsDescription2 { get; set; }
        public virtual ICollection<DbAddressLevelStructure>? AddressLevelStructureAsDescription3 { get; set; }
    }
}
