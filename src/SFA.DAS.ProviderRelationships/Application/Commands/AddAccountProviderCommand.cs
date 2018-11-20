using System;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class AddAccountProviderCommand : IRequest<long>
    {
        public long AccountId { get;  }
        public Guid UserRef { get;  }
        public long Ukprn { get;  }

        public AddAccountProviderCommand(long accountId, Guid userRef, long ukprn)
        {
            AccountId = accountId;
            UserRef = userRef;
            Ukprn = ukprn;
        }
    }
}