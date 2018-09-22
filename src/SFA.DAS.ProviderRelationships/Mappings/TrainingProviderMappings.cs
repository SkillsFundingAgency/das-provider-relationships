using AutoMapper;
using SFA.DAS.Apprenticeships.Api.Types.Providers;
using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Mappings
{
    public class TrainingProviderMappings : Profile
    {
        public TrainingProviderMappings()
        {
            CreateMap<Provider, TrainingProviderDto>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.ProviderName));
        }
    }
}