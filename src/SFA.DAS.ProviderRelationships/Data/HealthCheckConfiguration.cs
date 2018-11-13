using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Data
{
    public class HealthCheckConfiguration : IEntityTypeConfiguration<HealthCheck>
    {
        public void Configure(EntityTypeBuilder<HealthCheck> builder)
        {
        }
    }
}