using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRelationships.Domain.Models;

namespace SFA.DAS.ProviderRelationships.Domain.Data
{
    public class AddedAccountProviderEventAuditConfiguration : IEntityTypeConfiguration<AddedAccountProviderEventAudit>
    {
        public void Configure(EntityTypeBuilder<AddedAccountProviderEventAudit> builder)
        {
        }
    }
}