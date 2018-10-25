using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Data
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Ref);
            builder.Property(u => u.Ref).ValueGeneratedNever();

            builder.Property(u => u.Email).IsRequired().HasColumnType("varchar(255)");
            builder.Property(u => u.FirstName).IsRequired().HasColumnType("nvarchar(50)");
            builder.Property(u => u.LastName).IsRequired().HasColumnType("nvarchar(50)");
        }
    }
}