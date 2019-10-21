using System;
using System.Collections.Generic;
using AutoFixture;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddInvitationCommand;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationQuery;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationQuery.Dtos;
using SFA.DAS.ProviderRegistrations.Web.Authentication;
using SFA.DAS.ProviderRegistrations.Web.Controllers;
using SFA.DAS.ProviderRegistrations.Web.Mappings;
using SFA.DAS.ProviderRegistrations.Web.ViewModels;

namespace SFA.DAS.ProviderRegistrations.Web.UnitTests.Controllers.RegistrationControllerUnitTests
{
    [TestFixture]
    public class WhenIViewInvitedEmployers
    {
        private RegistrationControllerTestFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new RegistrationControllerTestFixture();
        }

        [Test]
        public async Task ThenAnInvitationIsAdded()
        {
            await _fixture.ViewInvitedEmployer();
            _fixture.VerifyInvitationQuery();
        }

        private class RegistrationControllerTestFixture
        {
            private readonly RegistrationController _controller;
            private readonly Mock<IMediator> _mediator;
            private readonly IMapper _mapper;
            private readonly Mock<IAuthenticationService> _authenticationService;

            public RegistrationControllerTestFixture()
            {
                _authenticationService = new Mock<IAuthenticationService>();
                _mapper = new MapperConfiguration(c => c.AddProfiles(typeof(InvitationMappings))).CreateMapper();

                _mediator = new Mock<IMediator>();
                _mediator.Setup(x => x.Send(It.IsAny<GetInvitationQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new GetInvitationQueryResult(new List<InvitationDto>()));

                _controller = new RegistrationController(_mediator.Object, _mapper, _authenticationService.Object);
            }

            public async Task ViewInvitedEmployer()
            {
                await _controller.InvitedEmployers(null, null);
            }

            public void VerifyInvitationQuery()
            {
                _mediator.Verify(x => x.Send(It.IsAny<GetInvitationQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            }
        }
    }
}
