using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Data
{
    public class AccountLegalEntityConfiguration : IEntityTypeConfiguration<AccountLegalEntity>
    {
        public void Configure(EntityTypeBuilder<AccountLegalEntity> builder)
        {
            builder.Property(ale => ale.Id).ValueGeneratedNever();
            builder.Property(ale => ale.PublicHashedId).IsRequired().HasColumnType("char(6)");
            builder.Property(ale => ale.Name).IsRequired().HasColumnType("nvarchar(100)");
            builder.HasOne(ale => ale.Account).WithMany(a => a.AccountLegalEntities).Metadata.DeleteBehavior = DeleteBehavior.Restrict;
            builder.HasQueryFilter(ale => ale.Deleted == null);
        }
    }
}