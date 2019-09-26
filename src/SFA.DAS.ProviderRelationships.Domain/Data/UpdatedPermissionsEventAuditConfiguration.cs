using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRelationships.Domain.Models;

namespace SFA.DAS.ProviderRelationships.Domain.Data
{
    public class UpdatedPermissionsEventAuditConfiguration : IEntityTypeConfiguration<UpdatedPermissionsEventAudit>
    {
        public void Configure(EntityTypeBuilder<UpdatedPermissionsEventAudit> builder)
        {
            builder.Property(a => a.GrantedOperations).IsRequired();
        }
    }
}