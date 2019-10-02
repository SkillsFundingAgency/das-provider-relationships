using AutoMapper;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationQuery.Dtos;
using SFA.DAS.ProviderRelationships.Domain.Models;

namespace SFA.DAS.ProviderRegistrations.Mappings
{
    public class InvitationMappings : Profile
    {
        public InvitationMappings()
        {
            CreateMap<Invitation, InvitationDto>()
                .ForMember(d => d.SentDate, opt => opt.MapFrom(s => s.CreatedDate));
        }
    }
}