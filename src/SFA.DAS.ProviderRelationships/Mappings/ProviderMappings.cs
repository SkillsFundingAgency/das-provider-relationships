using AutoMapper;
using SFA.DAS.ProviderRelationships.Dtos;
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