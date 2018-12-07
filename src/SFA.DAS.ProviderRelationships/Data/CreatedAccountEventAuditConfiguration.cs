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
            builder.Property(e => e.TimeLogged).IsRequired().HasColumnType("datetime");
            builder.Property(e => e.AccountId).IsRequired().HasColumnType("int");
            builder.Property(e => e.Name).IsRequired().HasColumnType("nvarchar(255)");
            builder.Property(e => e.PublicHashedId).IsRequired().HasColumnType("nvarchar(255)");
            builder.Property(e => e.UserName).IsRequired().HasColumnType("nvarchar(255)");
            builder.Property(e => e.UserRef).IsRequired();
        }
    }

    public class UpdatedPermissionsEventAuditConfiguration : IEntityTypeConfiguration<UpdatedPermissionsEventAudit>
    {
        public void Configure(EntityTypeBuilder<UpdatedPermissionsEventAudit> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.TimeLogged).IsRequired().HasColumnType("datetime");
            builder.Property(e => e.AccountId).IsRequired().HasColumnType("bigint");
            builder.Property(e => e.UserRef).IsRequired();
            builder.Property(e => e.Updated).IsRequired().HasColumnType("datetime");
            builder.Property(e => e.AccountLegalEntityId).IsRequired().HasColumnType("bigint");
            builder.Property(e => e.AccountProviderId).IsRequired().HasColumnType("bigint");
            builder.Property(e => e.AccountProviderLegalEntityId).IsRequired().HasColumnType("bigint");
            builder.Property(e => e.Ukprn).IsRequired().HasColumnType("bigint");
            builder.Property(e => e.GrantedOperations).HasColumnType("nvarchar(255)");
        }
    }

    public class DeletedPermissionsEventAuditConfiguration : IEntityTypeConfiguration<DeletedPermissionsEventAudit>
    {
        public void Configure(EntityTypeBuilder<DeletedPermissionsEventAudit> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Ukprn).IsRequired().HasColumnType("bigint");
            builder.Property(e => e.Deleted).IsRequired().HasColumnType("datetime");
            builder.Property(e => e.TimeLogged).IsRequired().HasColumnType("datetime");
        }
    }
}