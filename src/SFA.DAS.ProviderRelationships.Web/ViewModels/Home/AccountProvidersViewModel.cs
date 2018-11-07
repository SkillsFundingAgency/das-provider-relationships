using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Application.Queries;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.Home
{
    public class AccountProvidersViewModel
    {
        public List<GetAddedProvidersQueryReply.AccountProvider> AccountProviders { get; set; }
    }
}