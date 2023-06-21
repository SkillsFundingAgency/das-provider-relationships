using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Data.Configuration
{
    public class HealthCheckConfiguration : IEntityTypeConfiguration<HealthCheck>
    {
        public void Configure(EntityTypeBuilder<HealthCheck> builder)
        {
            builder.HasOne(h => h.User).WithMany().Metadata.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}