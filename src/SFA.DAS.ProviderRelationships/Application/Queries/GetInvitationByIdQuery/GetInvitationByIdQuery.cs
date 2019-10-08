using System;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetInvitationByIdQuery
{
    public class GetInvitationByIdQuery : IRequest<GetInvitationByIdQueryResult>
    {
        public GetInvitationByIdQuery(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; }
    }
}