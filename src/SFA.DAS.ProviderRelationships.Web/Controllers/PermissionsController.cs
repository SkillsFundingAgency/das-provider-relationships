using System.Web.Mvc;
using AutoMapper;
using MediatR;
using SFA.DAS.Authorization.EmployerRoles;
using SFA.DAS.Authorization.Mvc;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [DasAuthorize(EmployerRoles.Any)]
    [RoutePrefix("accounts/{hashedAccountId}/permissions")]
    public class PermissionsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PermissionsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
    }
}