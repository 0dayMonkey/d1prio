using AddressLocation.Domain.Models;

namespace AddressLocation.Infrastructure.Database.Models
{
    public partial class DbAddressPath
    {
        public string ChildId { get; set; } = null!;

        public decimal ChildLevel { get; set; }
       
        public string ParentId { get; set; } = null!;

        public decimal ParentLevel { get; set; }

        public DateTime? LastUpdatedTimestamp { get; set; }

        public virtual DbAddressPath? Parent {  get; set; }

        public virtual ICollection<DbAddressPath>? Childs { get; } = new List<DbAddressPath>();

        public virtual DbCity? City { get; set; }
        public virtual DbCountry? ParentCountry { get; set; }
        public virtual DbAddressLevel1? ParentAddressLevel1 { get; set; }
        public virtual DbAddressLevel2? ParentAddressLevel2 { get; set; }
        public virtual DbAddressLevel3? ParentAddressLevel3 { get; set; }
        public virtual DbAddressLevel1? ChildAddressLevel1 { get; set; }
        public virtual DbAddressLevel2? ChildAddressLevel2 { get; set; }
        public virtual DbAddressLevel3? ChildAddressLevel3 { get; set; }

        public void ClearDbLink() {
            City = null;
            ParentCountry = null;
            ParentAddressLevel1 = null;
            ParentAddressLevel2 = null;
            ParentAddressLevel3 = null;
            ChildAddressLevel1 = null;
            ChildAddressLevel2 = null;
            ChildAddressLevel3 = null;
        }
    }
}
