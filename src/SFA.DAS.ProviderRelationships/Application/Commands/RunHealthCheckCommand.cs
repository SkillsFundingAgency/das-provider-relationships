using System;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class RunHealthCheckCommand : IRequest
    {
        public Guid UserRef { get; }

        public RunHealthCheckCommand(Guid userRef)
        {
            UserRef = userRef;
        }
    }
}