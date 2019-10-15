﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddInvitationCommand;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationQuery;
using SFA.DAS.ProviderRegistrations.Types;
using SFA.DAS.ProviderRegistrations.Web.Authentication;
using SFA.DAS.ProviderRegistrations.Web.ViewModels;

namespace SFA.DAS.ProviderRegistrations.Web.Controllers
{
    [Route("{providerId}/[controller]/[action]")]
    public class RegistrationController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public RegistrationController(IMediator mediator, IMapper mapper, IAuthenticationService authenticationService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public IActionResult StartAccountSetup()
        {
            return View();
        }

        [HttpGet]
        public IActionResult NewEmployerUser()
        { 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewEmployerUser(NewEmployerUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("NewEmployerUser", model);
            }

            if (model.ProviderEmailAddress == null) model.ProviderEmailAddress = _authenticationService.UserEmail;
            return View("ReviewDetails", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InviteEmployeruser(NewEmployerUserViewModel model, string command)
        {
            if (command == "Change")
            {
                return View("NewEmployerUser", model);
            }

            if (!ModelState.IsValid)
            {
                return View("ReviewDetails", model);
            }

            await _mediator.Send(new AddInvitationCommand(
                _authenticationService.Ukprn,
                _authenticationService.UserId,
                model.EmployerOrganisation,
                model.EmployerFirstName,
                model.EmployerLastName,
                model.EmployerEmailAddress,
                model.CopyEmailToProvider ? model.ProviderEmailAddress : null
            ));

            return View("InviteConfirmation");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InviteConfirmation(string action)
        {
            switch (action)
            {
                case "Invite": return View();
                case "Homepage": return View();
                default:
                {
                    ViewBag.InValid = true;
                    return View();
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> InvitedEmployers(string sortColumn, string sortDirection)
        {
            sortColumn = Enum.GetNames(typeof(InvitationSortColumn)).SingleOrDefault(e => e == sortColumn);

            if (string.IsNullOrWhiteSpace(sortColumn)) sortColumn = Enum.GetNames(typeof(InvitationSortColumn)).First();
            if (string.IsNullOrWhiteSpace(sortDirection) || (sortDirection != "Asc" && sortDirection != "Desc")) sortDirection = "Asc";

            var results = await _mediator.Send(new GetInvitationQuery("UKPRN", null, sortColumn, sortDirection));

            var model = _mapper.Map<InvitationsViewModel>(results);
            model.SortColumn = sortColumn;
            model.SortDirection = sortDirection;

            return View(model);
        }
    }
}
