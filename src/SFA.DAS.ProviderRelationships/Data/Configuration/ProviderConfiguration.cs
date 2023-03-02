using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Data.Configuration
{
    public class ProviderConfiguration : IEntityTypeConfiguration<Provider>
    {
        public void Configure(EntityTypeBuilder<Provider> builder)
        {
            builder.HasKey(p => p.Ukprn);
            builder.Property(p => p.Ukprn).ValueGeneratedNever();
            builder.Property(p => p.Name).IsRequired().HasColumnType("nvarchar(100)");
        }
    }
}