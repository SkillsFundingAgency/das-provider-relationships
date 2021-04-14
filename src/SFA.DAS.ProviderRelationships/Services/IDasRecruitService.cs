using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Services
{
    public interface IDasRecruitService
    {
        Task<VacanciesSummary> GetVacanciesAsync(string hashedAccountId, long? legalEntityId = null, long? ukprn = null, int maxVacanciesToGet = int.MaxValue, CancellationToken cancellationToken = default);
        Task<BlockedOrganisationStatus> GetProviderBlockedStatusAsync(long providerUkprn, CancellationToken cancellationToken);
    }
}