using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Data
{
    public class AddedAccountProviderEventAuditConfiguration : IEntityTypeConfiguration<AddedAccountProviderEventAudit>
    {
        public void Configure(EntityTypeBuilder<AddedAccountProviderEventAudit> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.AccountProviderId).IsRequired().HasColumnType("bigint");
            builder.Property(e => e.AccountId).IsRequired().HasColumnType("bigint");
            builder.Property(e => e.ProviderUkprn).IsRequired().HasColumnType("bigint");
            builder.Property(e => e.UserRef).IsRequired();
            builder.Property(e => e.Added).IsRequired().HasColumnType("datetime2");
        }
    }
}