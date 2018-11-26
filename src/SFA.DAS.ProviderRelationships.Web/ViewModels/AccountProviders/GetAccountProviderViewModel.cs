using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderRelationships.Dtos;
using SFA.DAS.ProviderRelationships.Extensions;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders
{
    public class GetAccountProviderViewModel
    {
        public AccountProviderDto AccountProvider { get; set; }
        public List<AccountLegalEntityBasicDto> AccountLegalEntities { get; set; }

        public HashSet<Operation> GetOperationsByAccountLegalEntityId(long accountLegalEntityId)
        {
            return AccountProvider.AccountProviderLegalEntities
                .Where(aple => aple.AccountLegalEntityId == accountLegalEntityId)
                .SelectMany(aple => aple.Permissions)
                .Select(p => p.Operation)
                .OrderBy(o => o)
                .ToHashSet();
        }
    }
}