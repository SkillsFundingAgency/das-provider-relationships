using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands;

[TestFixture]
[Parallelizable]
public class CreateOrUpdateUserCommandHandlerTests 
{
    [Test]
    public async Task Handle_WhenHandlingCreateOrUpdateUserCommandAndUserDoesNotExist_ThenShouldCreateUser()
    {
        var fixture = new CreateOrUpdateUserCommandHandlerTestsFixture();

        await fixture.Handle();

        fixture.Db.Users.SingleOrDefault(u => u.Ref == fixture.CreateOrUpdateUserCommand.Ref)
            .Should().NotBeNull()
            .And.Match<User>(user =>
                user.Ref == fixture.CreateOrUpdateUserCommand.Ref &&
                user.Email == fixture.CreateOrUpdateUserCommand.Email &&
                user.FirstName == fixture.CreateOrUpdateUserCommand.FirstName &&
                user.LastName == fixture.CreateOrUpdateUserCommand.LastName &&
                user.Created >= fixture.Now &&
                user.Updated == null
                );
    }

    [Test]
    public async Task Handle_WhenHandlingCreateOrUpdateUserCommandAndUserDoesExistAndPropertiesHaveChanged_ThenShouldUpdateUser()
    {
        var fixture = new CreateOrUpdateUserCommandHandlerTestsFixture();
        
        fixture.SetUser().SetCommandWithChangedProperties();
        
        await fixture.Handle();
        
        fixture.Db.Users.SingleOrDefault(u => u.Ref == fixture.CreateOrUpdateUserCommand.Ref)
            .Should().NotBeNull()
            .And.Match<User>(user =>
                user.Ref == fixture.CreateOrUpdateUserCommand.Ref &&
                user.Email == fixture.CreateOrUpdateUserCommand.Email &&
                user.FirstName == fixture.CreateOrUpdateUserCommand.FirstName &&
                user.LastName == fixture.CreateOrUpdateUserCommand.LastName &&
                user.Created == fixture.Now &&
                user.Updated >= fixture.Now
                );
    }

    [Test]
    public async  Task Handle_WhenHandlingCreateOrUpdateUserCommandAndUserDoesExistAndPropertiesHaveNotChanged_ThenShouldNotUpdateUser()
    {
        var fixture = new CreateOrUpdateUserCommandHandlerTestsFixture();
        
        fixture.SetUser();
        
        await fixture.Handle();
        
        fixture.Db.Users.SingleOrDefault(u => u.Ref == fixture.CreateOrUpdateUserCommand.Ref)
            .Should().NotBeNull()
            .And.Match<User>(user =>
                user.Ref == fixture.CreateOrUpdateUserCommand.Ref &&
                user.Email == fixture.CreateOrUpdateUserCommand.Email &&
                user.FirstName == fixture.CreateOrUpdateUserCommand.FirstName &&
                user.LastName == fixture.CreateOrUpdateUserCommand.LastName &&
                user.Created == fixture.Now &&
                user.Updated == null
                );
    }
}

public class CreateOrUpdateUserCommandHandlerTestsFixture
{
    public ProviderRelationshipsDbContext Db { get; }
    public CreateOrUpdateUserCommand CreateOrUpdateUserCommand { get; set; }
    public IRequestHandler<CreateOrUpdateUserCommand> Handler { get; }
    public DateTime Now { get; }
    public User User { get; set; }

    public CreateOrUpdateUserCommandHandlerTestsFixture()
    {
        Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        CreateOrUpdateUserCommand = new CreateOrUpdateUserCommand(Guid.NewGuid(), "foo@bar.com", "Foo", "Bar");
        Handler = new CreateOrUpdateUserCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db),
            Mock.Of<ILogger<CreateOrUpdateUserCommandHandler>>());
        Now = DateTime.UtcNow;
    }

    public async Task Handle() => await Handler.Handle(CreateOrUpdateUserCommand, CancellationToken.None);
    
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

    public void SetCommandWithChangedProperties() => 
        CreateOrUpdateUserCommand = new CreateOrUpdateUserCommand(User.Ref, "_" + User.Email, "_" + User.FirstName, "_" + User.LastName);

}