using AutoMapper;
using SFA.DAS.ProviderRelationships.Dtos;
using SFA.DAS.ProviderRelationships.Models;

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