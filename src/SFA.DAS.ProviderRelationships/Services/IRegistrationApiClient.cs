using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Services
{
    public interface IRegistrationApiClient
    {
        Task Unsubscribe(string CorrelationId);

        Task<InvitationDto> GetInvitations(string CorrelationId, CancellationToken cancellationToken);
    }
}