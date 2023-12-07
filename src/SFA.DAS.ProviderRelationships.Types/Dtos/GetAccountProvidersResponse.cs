using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Types.Dtos;

public class GetAccountProvidersResponse
{
    public long AccountId { get; set; }
    public List<AccountProviderDto> AccountProviders { get; set; }
}