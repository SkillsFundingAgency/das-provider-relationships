using AutoMapper;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationQuery;
using SFA.DAS.ProviderRegistrations.Web.ViewModels;

namespace SFA.DAS.ProviderRegistrations.Web.Mappings
{
    public class InvitationMappings : Profile
    {
        public InvitationMappings()
        {
            CreateMap<GetInvitationQueryResult, InvitationsViewModel>();
        }
    }
}
