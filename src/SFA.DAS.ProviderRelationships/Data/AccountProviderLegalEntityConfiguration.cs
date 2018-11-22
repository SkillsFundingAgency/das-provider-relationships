using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Data
{
    public class AccountProviderLegalEntityConfiguration : IEntityTypeConfiguration<AccountProviderLegalEntity>
    {
        public void Configure(EntityTypeBuilder<AccountProviderLegalEntity> builder)
        {
            builder.HasOne(aple => aple.AccountProvider).WithMany(ap => ap.AccountProviderLegalEntities).Metadata.DeleteBehavior = DeleteBehavior.Restrict;
            builder.HasOne(aple => aple.AccountLegalEntity).WithMany(ale => ale.AccountProviderLegalEntities).Metadata.DeleteBehavior = DeleteBehavior.Restrict;
            builder.Property(aple => aple.Updated).IsConcurrencyToken();
        }
    }
}