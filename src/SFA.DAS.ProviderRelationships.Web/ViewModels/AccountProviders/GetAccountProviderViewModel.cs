using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders
{
    public class GetAccountProviderViewModel
    {
        public AccountProviderDto AccountProvider { get; set; }
        public List<AccountLegalEntityBasicDto> AccountLegalEntities { get; set; }
    }
}