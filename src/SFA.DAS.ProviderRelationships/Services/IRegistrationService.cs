using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Services
{
    public interface IRegistrationService
    {
        Task<InvitationDto> GetInvitationById(Guid correlationId, CancellationToken cancellationToken = default);
    }
}