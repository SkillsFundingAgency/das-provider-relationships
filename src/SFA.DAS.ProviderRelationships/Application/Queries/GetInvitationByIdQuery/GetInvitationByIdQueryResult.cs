using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetInvitationByIdQuery
{
    public class GetInvitationByIdQueryResult
    {
        public GetInvitationByIdQueryResult(InvitationDto invitation)
        {
            Invitation = invitation;
        }

        public InvitationDto Invitation { get; }
    }
}