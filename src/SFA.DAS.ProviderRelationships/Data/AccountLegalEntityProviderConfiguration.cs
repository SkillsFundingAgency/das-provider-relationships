using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Data
{
    public class AccountLegalEntityProviderConfiguration : IEntityTypeConfiguration<AccountLegalEntityProvider>
    {
        public void Configure(EntityTypeBuilder<AccountLegalEntityProvider> builder)
        {
            builder.HasKey(ap => new { ap.AccountLegalEntityId, ap.Ukprn });
            
            builder
                .HasOne(ap => ap.AccountLegalEntity)
                .WithMany(ap => ap.AccountLegalEntityProviders)
                .HasForeignKey(ap => ap.AccountLegalEntityId);
            
            builder
                .HasOne(ap => ap.Provider)
                .WithMany(ap => ap.AccountLegalEntityProviders)
                .HasForeignKey(ap => ap.Ukprn);
        }
    }
}