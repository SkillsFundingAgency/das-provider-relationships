using MediatR;
using SFA.DAS.ProviderRelationships.Types;

namespace SFA.DAS.ProviderRelationships.Api.Client.Application
{
    public class GetHasRelationshipWithPermissionQuery : IRequest<bool>
    {
        public long Ukprn { get; set; }
        public PermissionEnumDto Permission { get; set; }

    }
}