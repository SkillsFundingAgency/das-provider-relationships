using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Data
{
    public class CreatedAccountEventAuditConfiguration : IEntityTypeConfiguration<CreatedAccountEventAudit>
    {
        public void Configure(EntityTypeBuilder<CreatedAccountEventAudit> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.TimeLogged).IsRequired().HasColumnType("datetime2");
            builder.Property(e => e.AccountId).IsRequired().HasColumnType("bigint");
            builder.Property(e => e.Name).IsRequired().HasColumnType("nvarchar(100)");
            builder.Property(e => e.PublicHashedId).IsRequired().HasColumnType("char(6)");
            builder.Property(e => e.UserName).IsRequired().HasColumnType("nvarchar(100)");
            builder.Property(e => e.UserRef).IsRequired();
            builder.Property(e => e.HashedId).IsRequired().HasColumnType("char(6)");
        }
    }
}