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

    }
}