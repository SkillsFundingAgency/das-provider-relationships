using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.Http;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.ProviderRelationships.Services
{
    public class DasRecruitService : IDasRecruitService
    {
        private readonly ILog _log;
        private readonly HttpClient _httpClient;

        public DasRecruitService(ILog log, IRecruitApiHttpClientFactory recruitApiHttpClientFactory)
        {
            _log = log;
            _httpClient = recruitApiHttpClientFactory.CreateHttpClient();
        }

        public async Task<BlockedOrganisationStatus> GetProviderBlockedStatusAsync(long providerUkprn, CancellationToken cancellationToken = default)
        {
            var blockedProviderStatusUri = $"/api/providers/{providerUkprn}/status";

            try
            {
                var response = await _httpClient.GetAsync(blockedProviderStatusUri, cancellationToken);

                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new RestHttpClientException(response, content);

                var blockedOrgStatus = JsonConvert.DeserializeObject<BlockedOrganisationStatus>(content);
                return blockedOrgStatus;
            }
            catch (Exception ex)
            {
                _log.Warn($"Ignoring failed call to Recruit API: {ex}");
                throw;
            }
        }

        public async Task<VacanciesSummary> GetVacanciesAsync(
            string hashedAccountId,
            long? legalEntityId = null,
            string ukprn = null,
            int maxVacanciesToGet = int.MaxValue,
            CancellationToken cancellationToken = default)
        {
            _log.Info($"Getting VacanciesSummaries for Employer Account ID {hashedAccountId}");

            var vacanciesSummaryUri = $"/api/vacancies/?employerAccountId={hashedAccountId}&pageSize={maxVacanciesToGet}";

            if (legalEntityId.HasValue)
            {
                vacanciesSummaryUri = string.Concat(vacanciesSummaryUri, $"&legalEntityId={legalEntityId}");
            }

            if (string.IsNullOrEmpty(ukprn) == false)
            {
                vacanciesSummaryUri = string.Concat(vacanciesSummaryUri, $"&ukprn={ukprn}");
            }

            try
            {
                var response = await _httpClient.GetAsync(vacanciesSummaryUri, cancellationToken);

                if (response.StatusCode == HttpStatusCode.NotFound)
                    return null;

                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new RestHttpClientException(response, content);

                var vacanciesSummary = JsonConvert.DeserializeObject<VacanciesSummary>(content);
                return vacanciesSummary;
            }
            catch (Exception ex)
            {
                _log.Warn($"Ignoring failed call to Recruit API: {ex}");
                throw;
            }
        }
    }
}
