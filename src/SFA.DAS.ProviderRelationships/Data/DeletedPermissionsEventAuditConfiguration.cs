using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Data
{
    public class DeletedPermissionsEventAuditConfiguration : IEntityTypeConfiguration<DeletedPermissionsEventAudit>
    {
        public void Configure(EntityTypeBuilder<DeletedPermissionsEventAudit> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Ukprn).IsRequired().HasColumnType("bigint");
            builder.Property(e => e.Deleted).IsRequired().HasColumnType("datetime2");
            builder.Property(e => e.TimeLogged).IsRequired().HasColumnType("datetime2");
        }
    }
}