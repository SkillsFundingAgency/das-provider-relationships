﻿using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders;

namespace SFA.DAS.ProviderRelationships.Web.ViewComponents;

public class AccountProvidersWithSingleOrganisationViewComponent: ViewComponent
{
    public IViewComponentResult Invoke(AccountProvidersViewModel model)
    {
        return View(model);
    }
}