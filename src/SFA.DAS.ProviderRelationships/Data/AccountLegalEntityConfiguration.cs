using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Data
{
    public class AccountLegalEntityConfiguration : IEntityTypeConfiguration<AccountLegalEntity>
    {
        public void Configure(EntityTypeBuilder<AccountLegalEntity> builder)
        {
            builder.Property(a => a.Id).ValueGeneratedNever();
            builder.Property(a => a.PublicHashedId).IsRequired().HasColumnType("char(6)");
            builder.Property(a => a.Name).IsRequired().HasColumnType("nvarchar(100)");
        }
    }
}