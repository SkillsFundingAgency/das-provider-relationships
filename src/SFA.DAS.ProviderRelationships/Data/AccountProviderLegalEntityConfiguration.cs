using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Data
{
    public class AccountProviderLegalEntityConfiguration : IEntityTypeConfiguration<AccountProviderLegalEntity>
    {
        public void Configure(EntityTypeBuilder<AccountProviderLegalEntity> builder)
        {
            builder.HasOne(p => p.AccountProvider).WithMany(ap => ap.AccountProviderLegalEntities).Metadata.DeleteBehavior = DeleteBehavior.Restrict;
            builder.HasOne(p => p.AccountLegalEntity).WithMany(ale => ale.AccountProviderLegalEntities).Metadata.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}