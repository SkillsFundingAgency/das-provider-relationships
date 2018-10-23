using MediatR;
using SFA.DAS.ProviderRelationships.Document.Model;

namespace SFA.DAS.ProviderRelationships.Api.Client.Application
{
    public class GetAnyProviderRelationshipWithThisPermissonQuery : IRequest<bool>
    {
        public string Ukprn { get; set; }
        public PermissionEnum Permission { get; set; }

    }
}