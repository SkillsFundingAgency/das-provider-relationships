using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using SFA.DAS.Http;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.ProviderRelationships.Services
{
    public class DasRecruitService : IDasRecruitService
    {
        private readonly ILog _log;
        private readonly IRestHttpClient _httpClient;

        public DasRecruitService(ILog log, IRecruitApiHttpClientFactory recruitApiHttpClientFactory)
        {
            _log = log;
            _httpClient = recruitApiHttpClientFactory.CreateRestHttpClient();
        }

        public async Task<BlockedOrganisationStatus> GetProviderBlockedStatusAsync(long providerUkprn, CancellationToken cancellationToken = default)
        {
            var blockedProviderStatusUri = $"/api/providers/{providerUkprn}/status";

            try
            {
                var blockedOrgStatus = await _httpClient.Get<BlockedOrganisationStatus>(blockedProviderStatusUri, cancellationToken);
                _log.Info($"After getting organisation status for provider {providerUkprn}  and status is {blockedOrgStatus} ");
                return blockedOrgStatus;
            }
            catch (Exception ex)
            {
                _log.Warn($"Failed to call Provider Blocked Status endpoint of Recruit API: {ex.Message}");
                throw;
            }
        }

        public async Task<VacanciesSummary> GetVacanciesAsync(
            string hashedAccountId,
            long? legalEntityId = null,
            long? ukprn = null,
            int maxVacanciesToGet = int.MaxValue,
            CancellationToken cancellationToken = default)
        {
            _log.Info($"Getting Vacancies Summaries for Employer Account ID {hashedAccountId}");

            var vacanciesSummaryUri = $"/api/vacancies/?employerAccountId={hashedAccountId}&pageSize={maxVacanciesToGet}";

            if (legalEntityId.HasValue)
            {
                vacanciesSummaryUri = string.Concat(vacanciesSummaryUri, $"&legalEntityId={legalEntityId}");
            }

            if (ukprn.HasValue)
            {
                vacanciesSummaryUri = string.Concat(vacanciesSummaryUri, $"&ukprn={ukprn}");
            }

            try
            {
                var vacanciesSummary = await _httpClient.Get<VacanciesSummary>(vacanciesSummaryUri, cancellationToken);
                return vacanciesSummary;
            }
            catch (RestHttpClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return new VacanciesSummary(new List<VacancySummary>(), 0, 0, 0, 0);
                }

                _log.Warn($"Failed to call Vacancies Summary endpoint of Recruit API: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _log.Warn($"Failed to call Vacancies Summary endpoint of Recruit API: {ex.Message}");
                throw;
            }
        }
    }
}