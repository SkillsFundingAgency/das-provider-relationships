using AutoMapper;
using SFA.DAS.ProviderRelationships.Application.Queries.GetProviderToAdd.Dtos;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Mappings
{
    public class ProviderMappings : Profile
    {
        public ProviderMappings()
        {
            CreateMap<Provider, ProviderDto>();
        }
    }
}