using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Services
{
    public interface IRegistrationApiClient
    {
        Task Unsubscribe(string CorrelationId);

        Task<string> GetInvitations(string CorrelationId, CancellationToken cancellationToken);
    }
}