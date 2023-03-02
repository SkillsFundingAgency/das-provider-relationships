using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Data.Configuration
{
    public class DeletedPermissionsEventAuditConfiguration : IEntityTypeConfiguration<DeletedPermissionsEventAudit>
    {
        public void Configure(EntityTypeBuilder<DeletedPermissionsEventAudit> builder)
        {
        }
    }
}