using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Data
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasOne(p => p.AccountProvider).WithMany(ap => ap.Permissions).Metadata.DeleteBehavior = DeleteBehavior.Restrict;
            builder.HasOne(p => p.AccountLegalEntity).WithMany(ale => ale.Permissions).Metadata.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}