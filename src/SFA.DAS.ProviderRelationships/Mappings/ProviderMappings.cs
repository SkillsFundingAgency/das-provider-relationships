using AutoMapper;
using SFA.DAS.Apprenticeships.Api.Types.Providers;
using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Mappings
{
    public class ProviderMappings : Profile
    {
        public ProviderMappings()
        {
            CreateMap<Provider, ProviderDto>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.ProviderName));
        }
    }
}