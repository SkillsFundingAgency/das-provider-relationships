using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Data
{
    public class AccountProviderConfiguration : IEntityTypeConfiguration<AccountProvider>
    {
        public void Configure(EntityTypeBuilder<AccountProvider> builder)
        {
        }
    }
}