using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Services
{
    public interface IDasRecruitService
    {
        Task<BlockedOrganisationStatus> GetProviderBlockedStatusAsync(long providerUkprn, CancellationToken cancellationToken);
    }
}