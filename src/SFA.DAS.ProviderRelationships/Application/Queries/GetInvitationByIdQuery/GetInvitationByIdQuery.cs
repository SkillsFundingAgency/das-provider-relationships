using System;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetInvitationByIdQuery
{
    public class GetInvitationByIdQuery : IRequest<GetInvitationByIdQueryResult>
    {
        public Guid CorrelationId { get; set; }
    }
}