using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Data.Configuration
{
    public class AccountProviderConfiguration : IEntityTypeConfiguration<AccountProvider>
    {
        public void Configure(EntityTypeBuilder<AccountProvider> builder)
        {
            builder.HasOne(ap => ap.Account).WithMany(a => a.AccountProviders).Metadata.DeleteBehavior = DeleteBehavior.Restrict;
            builder.HasOne(ap => ap.Provider).WithMany(p => p.AccountProviders).Metadata.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}