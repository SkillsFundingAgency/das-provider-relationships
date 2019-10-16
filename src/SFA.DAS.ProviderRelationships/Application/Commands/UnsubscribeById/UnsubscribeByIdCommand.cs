using System;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands.UnsubscribeById
{
    public class UnsubscribeByIdCommand : IRequest
    {
        public UnsubscribeByIdCommand(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; }
    }
}