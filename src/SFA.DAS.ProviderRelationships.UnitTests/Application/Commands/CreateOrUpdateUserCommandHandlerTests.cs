using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;
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
            return TestAsync(f => f.Handle(), f => f.Db.Users.SingleOrDefault(u => u.Ref == f.CreateOrUpdateUserCommand.Ref).Should().NotBeNull()
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
            return TestAsync(f => f.SetUser().SetCommandWithChangedProperties(), f => f.Handle(), f => f.Db.Users.SingleOrDefault(u => u.Ref == f.CreateOrUpdateUserCommand.Ref).Should().NotBeNull()
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
            return TestAsync(f => f.SetUser(), f => f.Handle(), f => f.Db.Users.SingleOrDefault(u => u.Ref == f.CreateOrUpdateUserCommand.Ref).Should().NotBeNull()
                .And.Match<User>(u => 
                    u.Ref == f.CreateOrUpdateUserCommand.Ref &&
                    u.Email == f.CreateOrUpdateUserCommand.Email &&
                    u.FirstName == f.CreateOrUpdateUserCommand.FirstName &&
                    u.LastName == f.CreateOrUpdateUserCommand.LastName &&
                    u.Created == f.Now &&
                    u.Updated == null));
        }
    }

    public class CreateOrUpdateUserCommandHandlerTestsFixture
    {
        public ProviderRelationshipsDbContext Db { get; set; }
        public CreateOrUpdateUserCommand CreateOrUpdateUserCommand { get; set; }
        public IRequestHandler<CreateOrUpdateUserCommand, Unit> Handler { get; set; }
        public DateTime Now { get; set; }
        public User User { get; set; }

        public CreateOrUpdateUserCommandHandlerTestsFixture()
        {
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            CreateOrUpdateUserCommand = new CreateOrUpdateUserCommand(Guid.NewGuid(), "foo@bar.com", "Foo", "Bar");
            Handler = new CreateOrUpdateUserCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
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

        public CreateOrUpdateUserCommandHandlerTestsFixture SetCommandWithChangedProperties()
        {
            CreateOrUpdateUserCommand = new CreateOrUpdateUserCommand(User.Ref, "_" + User.Email, "_" + User.FirstName, "_" + User.LastName);
            
            return this;
        }
    }
}