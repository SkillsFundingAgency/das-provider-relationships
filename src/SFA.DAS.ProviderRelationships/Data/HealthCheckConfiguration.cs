using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Data
{
    public class HealthCheckConfiguration : IEntityTypeConfiguration<HealthCheck>
    {
        public void Configure(EntityTypeBuilder<HealthCheck> builder)
        {
            //todo: compare complains about ... HealthCheck->Property 'UserRef', nullability. Expected = NULL, found = NOT NULL
            //builder.HasOne<User>().WithMany().IsRequired();
        }
    }
}