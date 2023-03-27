using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Services;

public interface IRegistrationApiClient
{
    Task Unsubscribe(string correlationId);

    Task<string> GetInvitations(string correlationId, CancellationToken cancellationToken);
}