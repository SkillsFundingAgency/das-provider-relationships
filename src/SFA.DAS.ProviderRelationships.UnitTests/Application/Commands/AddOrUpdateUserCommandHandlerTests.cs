using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class AddOrUpdateUserCommandHandlerTests : FluentTest<AddOrUpdateUserCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingAddOrUpdateUserCommandAndUserDoesNotExist_ThenShouldAddUser()
        {
            return RunAsync(f => f.Handle(), f => f.Db.Users.SingleOrDefault(u => u.Ref == f.AddOrUpdateUserCommand.Ref).Should().NotBeNull()
                .And.Match<User>(u => 
                    u.Ref == f.AddOrUpdateUserCommand.Ref.Value &&
                    u.Email == f.AddOrUpdateUserCommand.Email &&
                    u.FirstName == f.AddOrUpdateUserCommand.FirstName &&
                    u.LastName == f.AddOrUpdateUserCommand.LastName &&
                    u.Created >= f.Now &&
                    u.Updated == null));
        }
        
        [Test]
        public Task Handle_WhenHandlingAddOrUpdateUserCommandAndUserDoesExistAndPropertiesHaveChanged_ThenShouldUpdateUser()
        {
            return RunAsync(f => f.SetUser().SetCommandWithChangedProperties(), f => f.Handle(), f => f.Db.Users.SingleOrDefault(u => u.Ref == f.AddOrUpdateUserCommand.Ref).Should().NotBeNull()
                .And.Match<User>(u => 
                    u.Ref == f.AddOrUpdateUserCommand.Ref.Value &&
                    u.Email == f.AddOrUpdateUserCommand.Email &&
                    u.FirstName == f.AddOrUpdateUserCommand.FirstName &&
                    u.LastName == f.AddOrUpdateUserCommand.LastName &&
                    u.Created == f.Now &&
                    u.Updated >= f.Now));
        }
        
        [Test]
        public Task Handle_WhenHandlingAddOrUpdateUserCommandAndUserDoesExistAndPropertiesHaveNotChanged_ThenShouldUpdateUser()
        {
            return RunAsync(f => f.SetUser(), f => f.Handle(), f => f.Db.Users.SingleOrDefault(u => u.Ref == f.AddOrUpdateUserCommand.Ref).Should().NotBeNull()
                .And.Match<User>(u => 
                    u.Ref == f.AddOrUpdateUserCommand.Ref.Value &&
                    u.Email == f.AddOrUpdateUserCommand.Email &&
                    u.FirstName == f.AddOrUpdateUserCommand.FirstName &&
                    u.LastName == f.AddOrUpdateUserCommand.LastName &&
                    u.Created == f.Now &&
                    u.Updated == null));
        }
    }

    public class AddOrUpdateUserCommandHandlerTestsFixture
    {
        public AddOrUpdateUserCommand AddOrUpdateUserCommand { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IRequestHandler<AddOrUpdateUserCommand, Unit> Handler { get; set; }
        public DateTime Now { get; set; }
        public User User { get; set; }

        public AddOrUpdateUserCommandHandlerTestsFixture()
        {
            AddOrUpdateUserCommand = new AddOrUpdateUserCommand
            {
                Ref = Guid.NewGuid(),
                Email = "foo@bar.com",
                FirstName = "Foo",
                LastName = "Bar"
            };
            
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            Handler = new AddOrUpdateUserCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
            Now = DateTime.UtcNow;
        }

        public async Task Handle()
        {
            await Handler.Handle(AddOrUpdateUserCommand, CancellationToken.None);
            await Db.SaveChangesAsync();
        }

        public AddOrUpdateUserCommandHandlerTestsFixture SetUser()
        {
            User = new UserBuilder()
                .WithRef(AddOrUpdateUserCommand.Ref.Value)
                .WithEmail(AddOrUpdateUserCommand.Email)
                .WithFirstName(AddOrUpdateUserCommand.FirstName)
                .WithLastName(AddOrUpdateUserCommand.LastName)
                .WithCreated(Now)
                .Build();
            
            Db.Users.Add(User);
            Db.SaveChanges();
            
            return this;
        }

        public AddOrUpdateUserCommandHandlerTestsFixture SetCommandWithChangedProperties()
        {
            AddOrUpdateUserCommand = new AddOrUpdateUserCommand
            {
                Ref = User.Ref,
                Email = "_" + User.Email,
                FirstName = "_" + User.FirstName,
                LastName = "_" + User.LastName
            };
            
            return this;
        }
    }
}