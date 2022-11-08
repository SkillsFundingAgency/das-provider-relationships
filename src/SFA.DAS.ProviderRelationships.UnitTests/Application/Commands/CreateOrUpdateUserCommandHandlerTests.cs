using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Services;
using SFA.DAS.ProviderRelationships.Services.OuterApi;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class CreateOrUpdateUserCommandHandlerTests : FluentTest<CreateOrUpdateUserCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingCreateOrUpdateUserCommandAndUserDoesNotExist_ThenShouldCreateUser()
        {
            return RunAsync(
                f => f.Handle(), 
                f => f.Db.Users.SingleOrDefault(u => u.Ref == f.CreateOrUpdateUserCommand.Ref).Should().NotBeNull()
                .And.Match<User>(u => 
                    u.Ref == f.CreateOrUpdateUserCommand.Ref &&
                    u.Email == f.CreateOrUpdateUserCommand.Email &&
                    u.FirstName == f.CreateOrUpdateUserCommand.FirstName &&
                    u.LastName == f.CreateOrUpdateUserCommand.LastName &&
                    u.Created >= f.Now &&
                    u.Updated == null));
        }
        
        [Test]
        public Task Handle_WhenHandlingCreateOrUpdateUserCommandAndUserDoesExistAndPropertiesHaveChanged_ThenShouldUpdateUser()
        {
            return RunAsync(
                f => f.SetUser().SetCommandWithChangedProperties(), 
                f => f.Handle(), 
                f => f.Db.Users.SingleOrDefault(u => u.Ref == f.CreateOrUpdateUserCommand.Ref).Should().NotBeNull()
                .And.Match<User>(u => 
                    u.Ref == f.CreateOrUpdateUserCommand.Ref &&
                    u.Email == f.CreateOrUpdateUserCommand.Email &&
                    u.FirstName == f.CreateOrUpdateUserCommand.FirstName &&
                    u.LastName == f.CreateOrUpdateUserCommand.LastName &&
                    u.Created == f.Now &&
                    u.Updated >= f.Now));
        }
        
        [Test]
        public Task Handle_WhenHandlingCreateOrUpdateUserCommandAndUserDoesExistAndPropertiesHaveNotChanged_ThenShouldNotUpdateUser()
        {
            return RunAsync(
                f => f.SetUser(), 
                f => f.Handle(), 
                f => f.Db.Users.SingleOrDefault(u => u.Ref == f.CreateOrUpdateUserCommand.Ref).Should().NotBeNull()
                .And.Match<User>(u => 
                    u.Ref == f.CreateOrUpdateUserCommand.Ref &&
                    u.Email == f.CreateOrUpdateUserCommand.Email &&
                    u.FirstName == f.CreateOrUpdateUserCommand.FirstName &&
                    u.LastName == f.CreateOrUpdateUserCommand.LastName &&
                    u.Created == f.Now &&
                    u.Updated == null));
        }
        
        // use govuk signin
        
        [Test]
        public Task Handle_WhenHandlingCreateOrUpdateUserCommandAndUseGovUkSignInTrue_ThenShouldCallOuterApi()
        {
            return RunAsync(
                f => f.SetUseGovUkSignIn(true),
                f => f.Handle(),
                f =>
                {
                    var expectedUrl = new GetEmployerAccountRequest(f.CreateOrUpdateUserCommand.Ref.ToString(), f.CreateOrUpdateUserCommand.Email);
                    f.MockOuterApiClient.Verify(client => 
                        client.Get<GetUserAccountsResponse>(It.Is<IGetApiRequest>(request => 
                            request.GetUrl.Contains(expectedUrl.GetUrl))));
                });
        }
    }

    public class CreateOrUpdateUserCommandHandlerTestsFixture
    {
        public ProviderRelationshipsDbContext Db { get; set; }
        public CreateOrUpdateUserCommand CreateOrUpdateUserCommand { get; set; }
        public IRequestHandler<CreateOrUpdateUserCommand, Unit> Handler { get; set; }
        public DateTime Now { get; set; }
        public User User { get; set; }
        public ProviderRelationshipsConfiguration Configuration { get; set; }
        public Mock<IOuterApiClient> MockOuterApiClient { get; set; }

        public CreateOrUpdateUserCommandHandlerTestsFixture()
        {
            Configuration = new ProviderRelationshipsConfiguration();
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            CreateOrUpdateUserCommand = new CreateOrUpdateUserCommand(Guid.NewGuid(), "foo@bar.com", "Foo", "Bar");
            MockOuterApiClient = new Mock<IOuterApiClient>();
            Handler = new CreateOrUpdateUserCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db), MockOuterApiClient.Object,
                Configuration);
            Now = DateTime.UtcNow;
        }

        public async Task Handle()
        {
            await Handler.Handle(CreateOrUpdateUserCommand, CancellationToken.None);
            await Db.SaveChangesAsync();
        }

        public CreateOrUpdateUserCommandHandlerTestsFixture SetUser()
        {
            User = EntityActivator.CreateInstance<User>()
                .Set(u => u.Ref, CreateOrUpdateUserCommand.Ref)
                .Set(u => u.Email, CreateOrUpdateUserCommand.Email)
                .Set(u => u.FirstName, CreateOrUpdateUserCommand.FirstName)
                .Set(u => u.LastName, CreateOrUpdateUserCommand.LastName)
                .Set(u => u.Created, Now);
            
            Db.Users.Add(User);
            Db.SaveChanges();
            
            return this;
        }

        public CreateOrUpdateUserCommandHandlerTestsFixture SetUseGovUkSignIn(bool useGovUkSignIn)
        {
            Configuration.UseGovUkSignIn = useGovUkSignIn;
            return this;
        }

        public CreateOrUpdateUserCommandHandlerTestsFixture SetCommandWithChangedProperties()
        {
            CreateOrUpdateUserCommand = new CreateOrUpdateUserCommand(User.Ref, "_" + User.Email, "_" + User.FirstName, "_" + User.LastName);
            
            return this;
        }
    }
}