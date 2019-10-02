using System.Collections.Generic;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationQuery.Dtos;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationQuery
{
    public class GetInvitationQueryResult
    {
        public GetInvitationQueryResult(List<InvitationDto> invitations)
        {
            Invitations = invitations;
        }

        public List<InvitationDto> Invitations { get; }
    }
}