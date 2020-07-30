using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.ProviderRelationships.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork.DependencyResolution.StructureMap;
using StructureMap;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Authentication
{
    [TestFixture]
    [Parallelizable]
    public class PostAuthenticationHandlerTests : FluentTest<PostAuthenticationHandlerTestsFixture>
    {
        [Test]
        public void Handle_WhenHandlingPostAuthenticationIdentity_ThenShouldSendCreateOrUpdateUserCommand()
        {
            Run(f => f.Handle(), f => f.Mediator.Verify(m => m.Send(It.Is<CreateOrUpdateUserCommand>(c => 
                c.Ref == f.Ref && c.Email == f.Email && c.FirstName == f.FirstName && c.LastName == f.LastName), CancellationToken.None), Times.Once));
        }
    }

    public class PostAuthenticationHandlerTestsFixture
    {
        public Guid Ref { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public ClaimsIdentity ClaimsIdentity { get; set; }
        public IPostAuthenticationHandler Handler { get; set; }
        public Mock<IUnitOfWorkScope> UnitOfWorkScope { get; set; }
        public Mock<IContainer> Container { get; set; }
        public Mock<ILog> Log { get; set; }
        
        public PostAuthenticationHandlerTestsFixture()
        {
            Ref = Guid.NewGuid();
            Email = "foo@bar.com";
            FirstName = "Foo";
            LastName = "Bar";
            Mediator = new Mock<IMediator>();
            
            ClaimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(DasClaimTypes.Id, Ref.ToString()),
                new Claim(DasClaimTypes.Email, Email),
                new Claim(DasClaimTypes.GivenName, FirstName),
                new Claim(DasClaimTypes.FamilyName, LastName),
            });

            UnitOfWorkScope = new Mock<IUnitOfWorkScope>();
            Container = new Mock<IContainer>();
            Log = new Mock<ILog>();
            
            UnitOfWorkScope.Setup(s => s.RunAsync(It.IsAny<Func<IContainer, Task>>())).Returns(Task.CompletedTask).Callback<Func<IContainer, Task>>(o => o(Container.Object));
            Container.Setup(c => c.GetInstance<IMediator>()).Returns(Mediator.Object);
            
            Handler = new PostAuthenticationHandler(UnitOfWorkScope.Object, Log.Object);
        }

        public void Handle()
        {
            Handler.Handle(ClaimsIdentity);
        }
    }
}