using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRelationships.Domain.Models;

namespace SFA.DAS.ProviderRelationships.Domain.Data
{
    public class DeletedPermissionsEventAuditConfiguration : IEntityTypeConfiguration<DeletedPermissionsEventAudit>
    {
        public void Configure(EntityTypeBuilder<DeletedPermissionsEventAudit> builder)
        {
        }
    }
}