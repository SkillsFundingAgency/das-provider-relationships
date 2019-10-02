using System;
using AutoFixture;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddInvitationCommand;
using SFA.DAS.ProviderRegistrations.Web.Controllers;
using SFA.DAS.ProviderRegistrations.Web.ViewModels;

namespace SFA.DAS.ProviderRegistrations.Web.UnitTests.Controllers.RegistrationControllerUnitTests
{
    [TestFixture]
    public class WhenIPostNewEmployerUserViewModel
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
            await _fixture.InviteEmployeruser();
            _fixture.VerifyInvitationAdded();
        }

        private class RegistrationControllerTestFixture
        {
            private readonly RegistrationController _controller;
            private readonly Mock<IMediator> _mediator;
            private readonly Mock<IMapper> _mapper;
            private readonly NewEmployerUserViewModel _model;

            public RegistrationControllerTestFixture()
            {
                var autoFixture = new Fixture();

                _model = new NewEmployerUserViewModel {
                    EmployerOrganisation = autoFixture.Create<string>(),
                    EmployerFirstName = autoFixture.Create<string>(),
                    EmployerLastName = autoFixture.Create<string>(),
                    EmployerEmailAddress = autoFixture.Create<string>(),    
                    CopyEmailToProvider = autoFixture.Create<bool>()
                };

                _mapper = new Mock<IMapper>();
                _mediator = new Mock<IMediator>();
                _mediator.Setup(x => x.Send(It.IsAny<AddInvitationCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Guid.NewGuid().ToString());

                _controller = new RegistrationController(_mediator.Object, _mapper.Object);
            }

            public async Task InviteEmployeruser()
            {
                await _controller.InviteEmployeruser(_model, null);
            }

            public void VerifyInvitationAdded()
            {
                _mediator.Verify(x => x.Send(It.IsAny<AddInvitationCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            }
        }
    }
}
