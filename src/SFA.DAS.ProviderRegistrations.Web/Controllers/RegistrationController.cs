using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddInvitationCommand;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationQuery;
using SFA.DAS.ProviderRegistrations.Types;
using SFA.DAS.ProviderRegistrations.Web.ViewModels;

namespace SFA.DAS.ProviderRegistrations.Web.Controllers
{
    [Route("registration")]
    public class RegistrationController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public RegistrationController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("startAccountSetup")]
        public IActionResult StartAccountSetup()
        {
            return View();
        }

        [HttpGet]
        [Route("NewEmployeruser")]
        public IActionResult NewEmployerUser()
        { 
            return View();
        }

        [HttpPost]
        [Route("NewEmployeruser")]
        [ValidateAntiForgeryToken]
        public IActionResult NewEmployeruser(NewEmployerUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("NewEmployerUser", model);
            }

            return View("ReviewDetails", model);
        }

        [HttpPost]
        [Route("InviteEmployeruser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InviteEmployeruser(NewEmployerUserViewModel model, string command)
        {
            if (command == "Change")
            {
                return View("NewEmployerUser", model);
            }

            await _mediator.Send(new AddInvitationCommand(
                "UKPRN",
                "USER_REF",
                model.EmployerOrganisation,
                model.EmployerFirstName,
                model.EmployerLastName,
                model.EmployerEmailAddress,
                model.CopyEmailToProvider ? "PROVIDER_EMAIL" : null
            ));

            return View("InviteConfirmation");
        }

        [HttpPost]
        [Route("InviteConfirmation")]
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
        [Route("InvitedEmployers")]
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
