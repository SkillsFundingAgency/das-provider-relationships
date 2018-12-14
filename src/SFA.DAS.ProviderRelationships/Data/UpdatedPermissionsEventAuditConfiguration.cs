using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Data
{
    public class UpdatedPermissionsEventAuditConfiguration : IEntityTypeConfiguration<UpdatedPermissionsEventAudit>
    {
        public void Configure(EntityTypeBuilder<UpdatedPermissionsEventAudit> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Logged).IsRequired().HasColumnType("datetime2");
            builder.Property(e => e.AccountId).IsRequired().HasColumnType("bigint");
            builder.Property(e => e.UserRef).IsRequired();
            builder.Property(e => e.Updated).IsRequired().HasColumnType("datetime2");
            builder.Property(e => e.AccountLegalEntityId).IsRequired().HasColumnType("bigint");
            builder.Property(e => e.AccountProviderId).IsRequired().HasColumnType("bigint");
            builder.Property(e => e.AccountProviderLegalEntityId).IsRequired().HasColumnType("bigint");
            builder.Property(e => e.Ukprn).IsRequired().HasColumnType("bigint");
            builder.Property(e => e.GrantedOperations).HasColumnType("nvarchar(max)");
        }
    }
}