using AddressLocation.Domain.Models;
using AddressLocation.Infrastructure.Database.Enums;
using AddressLocation.Infrastructure.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AddressLocation.Infrastructure.Database
{

    public partial class AddressLocationContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AddressLocationContext> _logger;
        private readonly int _casinoId;

        public AddressLocationContext(ILogger<AddressLocationContext> logger, IConfiguration configuration, DbContextOptions<AddressLocationContext> options)
            : base(options)
        {
            _configuration = configuration;
            _logger = logger;
            _casinoId = int.Parse(_configuration["CFG:CASINO_ID"]);
        }

        public virtual DbSet<DbOutboxMessage> OutboxMessages { get; set; } = null!;
        public virtual DbSet<DbAddressLevel1> AddressLevel1 { get; set; } = null!;
        public virtual DbSet<DbAddressLevel2> AddressLevel2 { get; set; } = null!;
        public virtual DbSet<DbAddressLevel3> AddressLevel3 { get; set; } = null!;
        public virtual DbSet<DbAddressPath> AddressPath { get; set; } = null!;
        public virtual DbSet<DbPostalCode> PostalCodes { get; set; } = null!;
        public virtual DbSet<DbCountry> Countries { get; set; } = null!;
        public virtual DbSet<DbCity> Cities { get; set; } = null!;
        public virtual DbSet<DbAddressLevelLabel> AddressLevelDescription { get; set; } = null!;
        public virtual DbSet<DbAddressLevelStructure> AddressLevelStructure { get; set; } = null!;
        public virtual DbSet<DbLanguage> Languages { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = $"User Id={_configuration["DB:GLX:USERNAME"]};Password={_configuration["DB:GLX:PASSWORD"]};Data Source={_configuration["DB:GLX:DATASOURCE"]}";
                optionsBuilder
                    .UseOracle(connectionString, b => b.UseOracleSQLCompatibility("11"))
                    .EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasDbFunction(typeof(ContextDbFunctions).GetMethod(nameof(ContextDbFunctions.Translate)))
                .HasName("TRANSLATE");

            modelBuilder.Entity<DbOutboxMessage>(entity =>
            {
                entity.ToTable("OUTBOX_MESSAGES", "GALAXIS");

                entity.HasKey(b => b.Id);

                entity.Property(b => b.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd()
                    .UseHiLo("OUTBOX_MESSAGE_SEQ", "GALAXIS");

                entity.Property(b => b.CorrelationId)
                    .HasColumnName("CORRELATION_ID");

                entity.Property(b => b.SiteOriginId)
                    .HasColumnName("SITE_ORIGIN_ID");

                entity.Property(b => b.EventTimestampUtc)
                    .HasColumnType("DATE")
                    .HasColumnName("EVENT_TS_UTC");

                entity.Property(b => b.ExpirationTimestampUtc)
                    .HasColumnType("DATE")
                    .HasColumnName("EXPIRY_TS_UTC");

                entity.Property(b => b.Context)
                    .HasColumnName("CONTEXT");

                entity.Property(b => b.RoutingKey)
                    .HasColumnName("ROUTING_KEY");

                entity.Property(b => b.Exchange)
                    .HasColumnName("EXCHANGE");

                entity.Property(b => b.IsReliable)
                    .HasPrecision(1)
                    .HasColumnName("RELIABLE");

                entity.Property(b => b.ConsistentHash)
                    .HasColumnName("CONSISTENT_HASH");

                entity.Property(b => b.Data)
                    .HasColumnName("DATA");
            });

            modelBuilder.Entity<DbAddressLevel1>(entity =>
                {
                    entity.HasKey(e => e.Id);

                    entity.ToTable("ADRLEV1", "GALAXIS");

                    entity.Property(e => e.Id)
                        .HasMaxLength(21)
                        .IsUnicode(false)
                        .HasColumnName("IDADRLEV1");

                    entity.Property(e => e.Abbreviation)
                        .HasMaxLength(5)
                        .IsUnicode(false)
                        .HasColumnName("ABREVIATIO");

                    entity.Property(e => e.LastUpdatedTimestamp)
                        .HasColumnType("DATE")
                        .HasColumnName("DAT_HEU");

                    entity.Property(e => e.ShortLabel)
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnName("LIB_COURT");

                    entity.Property(e => e.LongLabel)
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnName("LIB_LONG");

                    entity.HasOne(e => e.AsParentAddressPath)
                        .WithOne(p => p.ParentAddressLevel1)
                        .HasForeignKey<DbAddressLevel1>(e => e.Id)
                        .HasPrincipalKey<DbAddressPath>(x => x.ParentId)
                        .IsRequired(false);

                    entity.HasOne(e => e.AsChildAddressPath)
                        .WithOne(p => p.ChildAddressLevel1)
                        .HasForeignKey<DbAddressLevel1>(e => e.Id)
                        .HasPrincipalKey<DbAddressPath>(x => x.ChildId)
                        .IsRequired(false);
                });

                modelBuilder.Entity<DbAddressLevel2>(entity =>
                {
                    entity.HasKey(e => e.Id);

                    entity.ToTable("ADRLEV2", "GALAXIS");

                    entity.Property(e => e.Id)
                        .HasMaxLength(21)
                        .IsUnicode(false)
                        .HasColumnName("IDADRLEV2");

                    entity.Property(e => e.Abbreviation)
                        .HasMaxLength(5)
                        .IsUnicode(false)
                        .HasColumnName("ABREVIATIO");

                    entity.Property(e => e.LastUpdatedTimestamp)
                        .HasColumnType("DATE")
                        .HasColumnName("DAT_HEU");

                    entity.Property(e => e.ShortLabel)
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnName("LIB_COURT");

                    entity.Property(e => e.LongLabel)
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnName("LIB_LONG");

                    entity.HasOne(e => e.AsParentAddressPath)
                        .WithOne(p => p.ParentAddressLevel2)
                        .HasForeignKey<DbAddressLevel2>(e => e.Id)
                        .HasPrincipalKey<DbAddressPath>(x => x.ParentId)
                        .IsRequired(false);

                    entity.HasOne(e => e.AsChildAddressPath)
                        .WithOne(p => p.ChildAddressLevel2)
                        .HasForeignKey<DbAddressLevel2>(e => e.Id)
                        .HasPrincipalKey<DbAddressPath>(x => x.ChildId)
                        .IsRequired(false);
                });

                modelBuilder.Entity<DbAddressLevel3>(entity =>
                {
                    entity.HasKey(e => e.Id);

                    entity.ToTable("ADRLEV3", "GALAXIS");

                    entity.Property(e => e.Id)
                        .HasMaxLength(21)
                        .IsUnicode(false)
                        .HasColumnName("IDADRLEV3");

                    entity.Property(e => e.Abbreviation)
                        .HasMaxLength(5)
                        .IsUnicode(false)
                        .HasColumnName("ABREVIATIO");

                    entity.Property(e => e.LastUpdatedTimestamp)
                        .HasColumnType("DATE")
                        .HasColumnName("DAT_HEU");

                    entity.Property(e => e.ShortLabel)
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnName("LIB_COURT");

                    entity.Property(e => e.LongLabel)
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnName("LIB_LONG");

                    entity.HasOne(e => e.AsParentAddressPath)
                        .WithOne(p => p.ParentAddressLevel3)
                        .HasForeignKey<DbAddressLevel3>(e => e.Id)
                        .HasPrincipalKey<DbAddressPath>(x => x.ParentId)
                        .IsRequired(false);

                    entity.HasOne(e => e.AsChildAddressPath)
                        .WithOne(p => p.ChildAddressLevel3)
                        .HasForeignKey<DbAddressLevel3>(e => e.Id)
                        .HasPrincipalKey<DbAddressPath>(x => x.ChildId)
                        .IsRequired(false);
                });

                modelBuilder.Entity<DbAddressPath>(entity =>
                {
                    entity.HasKey(e => new { e.ChildId });

                    entity.ToTable("ADRPTH", "GALAXIS");

                    entity.Property(e => e.ChildId)
                        .HasMaxLength(21)
                        .IsUnicode(false)
                        .HasColumnName("ID_SON");

                    entity.Property(e => e.ChildLevel)
                        .HasColumnType("NUMBER(38)")
                        .HasColumnName("TYPSON")
                        .HasConversion(
                        v => (decimal)v,
                        v => (decimal)(LevelDescriptionType)v);

                    entity.Property(e => e.ParentId)
                        .HasMaxLength(21)
                        .IsUnicode(false)
                        .HasColumnName("ID_FAT");

                    entity.Property(e => e.ParentLevel)
                        .HasColumnType("NUMBER(38)")
                        .HasColumnName("TYPFAT")
                        .HasConversion(
                        v => (decimal)v,
                        v => (decimal)(LevelDescriptionType)v);

                    entity.Property(e => e.LastUpdatedTimestamp)
                        .HasColumnType("DATE")
                        .HasColumnName("DAT_HEU");

                    entity.HasOne(x => x.Parent)
                        .WithMany(x => x.Childs)
                        .HasForeignKey(x => x.ParentId)
                        .IsRequired(false);

                    entity.HasMany(x => x.Childs)
                        .WithOne(x => x.Parent)
                        .HasForeignKey(x => x.ParentId)
                        .IsRequired(false);

                    entity.HasOne(e => e.ParentAddressLevel1)
                        .WithOne(p => p.AsParentAddressPath)
                        .HasForeignKey<DbAddressPath>(p => p.ParentId)
                        .IsRequired(false);

                    entity.HasOne(e => e.ChildAddressLevel1)
                        .WithOne(p => p.AsChildAddressPath)
                        .HasForeignKey<DbAddressPath>(p => p.ChildId)
                        .IsRequired(false);

                    entity.HasOne(e => e.ParentAddressLevel2)
                        .WithOne(p => p.AsParentAddressPath)
                        .HasForeignKey<DbAddressPath>(p => p.ParentId)
                        .IsRequired(false);

                    entity.HasOne(e => e.ChildAddressLevel2)
                        .WithOne(p => p.AsChildAddressPath)
                        .HasForeignKey<DbAddressPath>(p => p.ChildId)
                        .IsRequired(false);

                    entity.HasOne(e => e.ParentAddressLevel3)
                        .WithOne(p => p.AsParentAddressPath)
                        .HasForeignKey<DbAddressPath>(p => p.ParentId)
                        .IsRequired(false);

                    entity.HasOne(e => e.ChildAddressLevel3)
                        .WithOne(p => p.AsChildAddressPath)
                        .HasForeignKey<DbAddressPath>(p => p.ChildId)
                        .IsRequired(false);

                    entity.HasOne(e => e.ParentCountry)
                        .WithOne(p => p.AddressPath)
                        .HasForeignKey<DbAddressPath>(p => p.ParentId)
                        .IsRequired(false);

                    entity.HasOne(e => e.City)
                        .WithOne(p => p.AddressPath)
                        .HasForeignKey<DbAddressPath>(p => p.ChildId)
                        .IsRequired(false);
                });

                modelBuilder.Entity<DbPostalCode>(entity =>
                {
                    entity.HasKey(e => new { e.Code, e.CityId });

                    entity.ToTable("BCP", "GALAXIS");

                    entity.Property(e => e.Code)
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnName("COD_POST");

                    entity.Property(e => e.CityId)
                        .HasMaxLength(21)
                        .IsUnicode(false)
                        .HasColumnName("ID_VILLE");

                    entity.Property(e => e.LastUpdatedTimestamp)
                        .HasColumnType("DATE")
                        .HasColumnName("DAT_HEU");

                    entity.HasOne(d => d.City)
                        .WithMany(p => p.PostalCodes)
                        .HasForeignKey(d => d.CityId);

                });

                modelBuilder.Entity<DbCountry>(entity =>
                {
                    entity.HasKey(e => e.Id);

                    entity.ToTable("BPAYS", "GALAXIS");

                    entity.HasIndex(e => e.LongLabel, "IX_BPAYS0")
                        .IsUnique();

                    entity.HasIndex(e => new { e.Id, e.CountryNumber }, "IX_BPAYS1");

                    entity.Property(e => e.Id)
                        .HasMaxLength(3)
                        .IsUnicode(false)
                        .HasColumnName("COD_PAYS");

                    entity.Property(e => e.IsMailAllowed)
                        .HasMaxLength(1)
                        .IsUnicode(false)
                        .HasColumnName("BOO_COURR")
                        .HasDefaultValueSql("0")
                        .HasConversion(new BoolToStringConverter("0", "1"));


                    entity.Property(e => e.IsPostcodeOnTheRight)
                        .HasMaxLength(1)
                        .IsUnicode(false)
                        .HasColumnName("BOO_DRGA")
                        .HasDefaultValueSql("0")
                        .HasConversion(new BoolToStringConverter("0", "1"));

                    entity.Property(e => e.ISO2)
                        .HasMaxLength(2)
                        .IsUnicode(false)
                        .HasColumnName("COD_PAYS2");

                    entity.Property(e => e.LastUpdatedTimestamp)
                        .HasColumnType("DATE")
                        .HasColumnName("DAT_HEU");

                    entity.Property(e => e.ShortLabel)
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnName("LIB_COURT");

                    entity.Property(e => e.LangId)
                        .HasMaxLength(2)
                        .IsUnicode(false)
                        .HasColumnName("LIB_LANG");

                    entity.Property(e => e.LongLabel)
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnName("LIB_LONG");

                    entity.Property(e => e.SocialSecurityMask)
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnName("MSKSCS");

                    entity.Property(e => e.NationalityLabel)
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnName("NATIO");

                    entity.Property(e => e.CountryNumber)
                        .HasPrecision(3)
                        .HasColumnName("NUM_PAYS");

                    entity.HasOne(e => e.AddressLevelStructure)
                        .WithOne(e => e.Country)
                        .HasForeignKey<DbAddressLevelStructure>(a => a.CountryId)
                        .IsRequired(false);

                    entity.HasOne(x => x.AddressPath)
                        .WithOne(x => x.ParentCountry)
                        .HasForeignKey<DbCountry>(x => x.Id)
                        .HasPrincipalKey<DbAddressPath>(x => x.ParentId)
                        .IsRequired(false);
                });

                modelBuilder.Entity<DbCity>(entity =>
                {
                    entity.HasKey(e => e.Id);

                    entity.ToTable("BVILLES", "GALAXIS");

                    entity.Property(e => e.Id)
                        .HasMaxLength(21)
                        .IsUnicode(false)
                        .HasColumnName("ID_VILLE");

                    entity.Property(e => e.LastUpdatedTimestamp)
                        .HasColumnType("DATE")
                        .HasColumnName("DAT_HEU");

                    entity.Property(e => e.ShortLabel)
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnName("LIB_COURT");

                    entity.Property(e => e.LongLabel)
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnName("LIB_LONG");

                    entity.HasMany(d => d.PostalCodes)
                        .WithOne(c => c.City)
                        .HasForeignKey(d => d.CityId);

                    entity.HasOne(x => x.AddressPath)
                        .WithOne(x => x.City)
                        .HasForeignKey<DbCity>(x => x.Id)
                        .IsRequired(false);
                });

            modelBuilder.Entity<DbAddressLevelLabel>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("DESCLEV", "GALAXIS");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("LEVELX");

                entity.Property(e => e.LastUpdatedTimestamp)
                    .HasColumnType("DATE")
                    .HasColumnName("DAT_HEU");

                entity.Property(e => e.Label)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DSC");

                entity.HasMany(d => d.AddressLevelStructureAsAbbreviation1)
                    .WithOne(p => p.AddressLevel1Abbreviation)
                    .HasForeignKey(d => d.AddressLevel1AbbreviationId)
                    .IsRequired(false);

                entity.HasMany(d => d.AddressLevelStructureAsAbbreviation2)
                    .WithOne(p => p.AddressLevel2Abbreviation)
                    .HasForeignKey(d => d.AddressLevel2AbbreviationId)
                    .IsRequired(false);

                entity.HasMany(d => d.AddressLevelStructureAsAbbreviation3)
                    .WithOne(p => p.AddressLevel3Abbreviation)
                    .HasForeignKey(d => d.AddressLevel3AbbreviationId)
                    .IsRequired(false);

                entity.HasMany(d => d.AddressLevelStructureAsDescription1)
                    .WithOne(p => p.AddressLevel1Description)
                    .HasForeignKey(d => d.AddressLevel1DescriptionId)
                    .IsRequired(false);

                entity.HasMany(d => d.AddressLevelStructureAsDescription2)
                    .WithOne(p => p.AddressLevel2Description)
                    .HasForeignKey(d => d.AddressLevel2DescriptionId)
                    .IsRequired(false);

                entity.HasMany(d => d.AddressLevelStructureAsDescription3)
                    .WithOne(p => p.AddressLevel3Description)
                    .HasForeignKey(d => d.AddressLevel3DescriptionId)
                    .IsRequired(false);
            });

            modelBuilder.Entity<DbAddressLevelStructure>(entity =>
            {
                entity.HasKey(e => e.CountryId);

                entity.ToTable("LEVTYP", "GALAXIS");

                entity.Property(e => e.CountryId)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("COD_PAYS");

                entity.Property(e => e.LastUpdatedTimestamp)
                    .HasColumnType("DATE")
                    .HasColumnName("DAT_HEU");

                entity.Property(e => e.AddressLevel1AbbreviationId)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("LEVABBREV1");

                entity.Property(e => e.AddressLevel2AbbreviationId)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("LEVABBREV2");

                entity.Property(e => e.AddressLevel3AbbreviationId)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("LEVABBREV3");

                entity.Property(e => e.AddressLevel1DescriptionId)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("LEVDESCR1");

                entity.Property(e => e.AddressLevel2DescriptionId)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("LEVDESCR2");

                entity.Property(e => e.AddressLevel3DescriptionId)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("LEVDESCR3");

                entity.HasOne(d => d.Country)
                    .WithOne(p => p.AddressLevelStructure)
                    .HasForeignKey<DbAddressLevelStructure>(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LEVTYP4");

                entity.HasOne(d => d.AddressLevel1Abbreviation)
                    .WithMany(p => p.AddressLevelStructureAsAbbreviation1)
                    .HasForeignKey(d => d.AddressLevel1AbbreviationId)
                    .HasPrincipalKey(d => d.Id)
                    .HasConstraintName("FK_LEVTYP1")
                    .IsRequired(false);

                entity.HasOne(d => d.AddressLevel2Abbreviation)
                    .WithMany(p => p.AddressLevelStructureAsAbbreviation2)
                    .HasForeignKey(d => d.AddressLevel2AbbreviationId)
                    .HasPrincipalKey(d => d.Id)
                    .HasConstraintName("FK_LEVTYP2")
                    .IsRequired(false);

                entity.HasOne(d => d.AddressLevel3Abbreviation)
                    .WithMany(p => p.AddressLevelStructureAsAbbreviation3)
                    .HasForeignKey(d => d.AddressLevel3AbbreviationId)
                    .HasPrincipalKey(d => d.Id)
                    .HasConstraintName("FK_LEVTYP3")
                    .IsRequired(false);

                entity.HasOne(d => d.AddressLevel1Description)
                    .WithMany(p => p.AddressLevelStructureAsDescription1)
                    .HasForeignKey(d => d.AddressLevel1DescriptionId)
                    .HasPrincipalKey(d => d.Id)
                    .IsRequired(false);

                entity.HasOne(d => d.AddressLevel2Description)
                    .WithMany(p => p.AddressLevelStructureAsDescription2)
                    .HasForeignKey(d => d.AddressLevel2DescriptionId)
                    .HasPrincipalKey(d => d.Id)
                    .IsRequired(false);

                entity.HasOne(d => d.AddressLevel3Description)
                    .WithMany(p => p.AddressLevelStructureAsDescription3)
                    .HasForeignKey(d => d.AddressLevel3DescriptionId)
                    .HasPrincipalKey(d => d.Id)
                    .IsRequired(false);
            });

            modelBuilder.Entity<DbLanguage>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("BLANGUE", "GALAXIS");

                entity.Property(e => e.Id)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COD_LANGUE");

                entity.Property(e => e.ShortLabel)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("LIB_COURT");

                entity.Property(e => e.LongLabel)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("LIB_LONG");

                entity.Property(e => e.LastUpdatedTimestamp)
                    .HasColumnType("DATE")
                    .HasColumnName("DAT_HEU");
            });

            modelBuilder.HasSequence("LIVE_MESSAGE_TEXT_SEQUENCE", "GALAXIS");

            modelBuilder.HasSequence<long>("OUTBOX_MESSAGE_SEQ", "GALAXIS").IncrementsBy(1);

            modelBuilder.HasSequence("SEQ_ID", "GALAXIS").IncrementsBy(10000);

            OnModelCreatingPartial(modelBuilder);
        }

        // GetGalaxisId
        public string GetGalaxisId()
        {
            object? nextVal;
            var connection = Database.GetDbConnection();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT {_configuration["DB:GLX:SCHEMA"]}.SEQ_GALAXIS_UID.NEXTVAL FROM DUAL";
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();
                nextVal = command.ExecuteScalar();
            }
            return string.Format("{0}{1}", _casinoId.ToString("0000"), Convert.ToInt64(nextVal).ToString("00000000000000000"));
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
