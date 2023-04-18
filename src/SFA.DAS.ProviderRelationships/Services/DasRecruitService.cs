using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Http;

namespace SFA.DAS.ProviderRelationships.Services;

public class DasRecruitService : IDasRecruitService
{
    private readonly ILogger<DasRecruitService> _log;
    private readonly IRestHttpClient _httpClient;

    public DasRecruitService(ILogger<DasRecruitService> log, IRecruitApiHttpClientFactory recruitApiHttpClientFactory)
    {
        _log = log;
        _httpClient = recruitApiHttpClientFactory.CreateRestHttpClient();
    }

    public async Task<BlockedOrganisationStatus> GetProviderBlockedStatusAsync(long providerUkprn, CancellationToken cancellationToken)
    {
        var blockedProviderStatusUri = $"/api/providers/{providerUkprn}/status";
            
        try
        {
            var blockedOrgStatus = await _httpClient.Get<BlockedOrganisationStatus>(blockedProviderStatusUri, cancellationToken, cancellationToken);
            
            _log.LogInformation("After getting organisation status for provider {ProviderUkprn}  and status is {BlockedOrgStatus} ", providerUkprn, blockedOrgStatus);
            
            return blockedOrgStatus;
        }
        catch (Exception ex)
        {
            _log.LogWarning("Failed to call Provider Blocked Status endpoint of Recruit API: {Message}", ex.Message);
            throw;
        }
    }
}