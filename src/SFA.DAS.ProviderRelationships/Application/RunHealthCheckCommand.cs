using System;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Application
{
    public class RunHealthCheckCommand : IRequest
    {
        public Guid? UserRef { get; set; } = Guid.Empty;
    }
}