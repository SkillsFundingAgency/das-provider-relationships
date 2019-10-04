using System;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands.UpsertUser
{
    public class UpsertUserCommand : IRequest
    {
        public string UserRef { get; }

        public Guid CorrelationId { get; }

        public DateTime Created { get; }

        public UpsertUserCommand(string userRef, DateTime created, Guid correlationId)
        {
            UserRef = userRef;
            Created = created;
            CorrelationId = correlationId;
        }
    }
}