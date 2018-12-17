using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Data
{
    public class AddedAccountProviderEventAuditConfiguration : IEntityTypeConfiguration<AddedAccountProviderEventAudit>
    {
        public void Configure(EntityTypeBuilder<AddedAccountProviderEventAudit> builder)
        {
        }
    }
}