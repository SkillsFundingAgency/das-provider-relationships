using AutoMapper;
using SFA.DAS.ProviderRelationships.Application.Queries.GetHealthCheck.Dtos;
using SFA.DAS.ProviderRelationships.Domain.Models;

namespace SFA.DAS.ProviderRelationships.Mappings
{
    public class HealthCheckMappings : Profile
    {
        public HealthCheckMappings()
        {
            CreateMap<HealthCheck, HealthCheckDto>();
        }
    }
}