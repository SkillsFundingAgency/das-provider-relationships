﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MediatR;
using Microsoft.IdentityModel;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.EmployerUserRoles;
using SFA.DAS.Authorization.Mvc;
using SFA.DAS.ProviderRelationships.Application.Queries.FindProviderToAdd;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAllProviders;
using SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviders;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders;
using SFA.DAS.ProviderRelationships.Web.ViewModels.Providers;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [DasAuthorize(EmployerFeature.ProviderRelationships)]
    [RoutePrefix("providers")]
    public class ProvidersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public ProvidersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [DasAuthorize(EmployerUserRole.Owner)]
        [Route("find")]
        public ActionResult Find()
        {
            return View(new FindProvidersViewModel());
        }

        [DasAuthorize(EmployerUserRole.Owner)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("find")]
        public async Task<ActionResult> Find(FindProvidersViewModel model)
        {
            var ukprn = long.Parse(model.Ukprn);
            //var query = new FindProviderToAddQuery(model.AccountId.Value, ukprn);
            var query = new FindAllProvidersQuery();
            var result = await _mediator.Send(query);

            //if (result.ProviderNotFound)
            //{
            //    ModelState.AddModelError(nameof(model.Ukprn), ErrorMessages.InvalidUkprn);

            //    return RedirectToAction("Find");
            //}

            //if (result.ProviderAlreadyAdded)
            //{
            //    return RedirectToAction("AlreadyAdded", new AlreadyAddedAccountProviderRouteValues { AccountProviderId = result.AccountProviderId.Value });
            //}

            //return RedirectToAction("Add", new AddAccountProviderRouteValues { Ukprn = result.Ukprn });
            return null;
        }
    }
}