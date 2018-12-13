using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Data
{
    public class DeletedPermissionsEventAuditConfiguration : IEntityTypeConfiguration<DeletedPermissionsEventAudit>
    {
        public void Configure(EntityTypeBuilder<DeletedPermissionsEventAudit> builder)
        {
            //builder.HasKey(e => e.Id);
            builder.Property(e => e.Ukprn).IsRequired();
            builder.Property(e => e.Deleted).IsRequired();
            builder.Property(e => e.TimeLogged).IsRequired();
        }
    }
}