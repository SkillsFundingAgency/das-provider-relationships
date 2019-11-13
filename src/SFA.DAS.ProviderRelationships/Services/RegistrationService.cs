using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Http;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly ILog _log;
        private readonly IRestHttpClient _httpClient;

        public RegistrationService(ILog log, IRegistrationApiHttpClientFactory registrationApiHttpClientFactory)
        {
            _log = log;
            _httpClient = registrationApiHttpClientFactory.CreateRestHttpClient();
        }

        public async Task<InvitationDto> GetInvitationById(Guid correlationId, CancellationToken cancellationToken = default)
        {
            var invitationQueryUri = $"/api/invitations/{correlationId}";

            try
            {
                var invitation = await _httpClient.Get<InvitationDto>(invitationQueryUri, cancellationToken);
                return invitation;
            }
            catch (Exception ex)
            {
                _log.Warn($"Failed to call invitations endpoint of Registration API: {ex.Message}");
                throw;
            }
        }
    }
}
