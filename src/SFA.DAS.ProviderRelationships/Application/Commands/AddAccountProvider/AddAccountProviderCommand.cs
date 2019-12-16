using System;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands.AddAccountProvider
{
    public class AddAccountProviderCommand : IRequest<long>
    {
        public long AccountId { get; }
        public long Ukprn { get; }
        public Guid UserRef { get; }
        public Guid? CorrelationId { get; }

        public AddAccountProviderCommand(long accountId, long ukprn, Guid userRef, Guid? correlationId = null)
        {
            AccountId = accountId;
            Ukprn = ukprn;
            UserRef = userRef;
            CorrelationId = correlationId;
        }
    }
}