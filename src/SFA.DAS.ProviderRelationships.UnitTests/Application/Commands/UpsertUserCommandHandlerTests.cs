using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands.UpsertUser;
using SFA.DAS.ProviderRelationships.Domain.Data;
using SFA.DAS.ProviderRelationships.Domain.Models;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class UpsertUserCommandHandlerTests : FluentTest<UpsertUserCommandHandlerTestFixture>
    {
        [Test]
        public Task Handle_WhenCommandIsHandled_ThenShouldUpdateInvitationStatus()
        {
            return RunAsync(f => f.Handle(), f =>
            {
                f.Invitation.Status.Should().Be((int) InvitationStatus.AccountStarted);
            });
        }

        [Test]
        public Task Handle_WhenDoesntExistCommandIsHandled_ThenNoChangesAreMade()
        {
            return RunAsync(f => f.HandleDoesntExist(), f =>
            {
                f.Invitation.Status.Should().Be((int) InvitationStatus.InvitationSent);
                f.InvitationDoesntExist.Status.Should().Be((int) InvitationStatus.InvitationSent);
                f.InvitationInvalidStatus.Status.Should().Be((int) InvitationStatus.PayeSchemeAdded);
            });
        }

        [Test]
        public Task Handle_WhenInvalidStatusCommandIsHandled_ThennoChangesAreMade()
        {
            return RunAsync(f => f.HandleInvalidStatus(), f =>
            {
                f.InvitationInvalidStatus.Status.Should().Be((int) InvitationStatus.PayeSchemeAdded);
            });
        }
    }

    public class UpsertUserCommandHandlerTestFixture
    {
        public ProviderRelationshipsDbContext Db { get; set; }
        public Invitation Invitation { get; set; }
        public Invitation InvitationDoesntExist { get; set; }
        public Invitation InvitationInvalidStatus { get; set; }
        public UpsertUserCommand Command { get; set; }
        public UpsertUserCommand CommandDoesntExist { get; set; }
        public UpsertUserCommand CommandInvalidStatus { get; set; }
        public IUnitOfWorkContext UnitOfWorkContext { get; set; }
        public IRequestHandler<UpsertUserCommand, Unit> Handler { get; set; }
        
        public UpsertUserCommandHandlerTestFixture()
        {
            Guid correlationId1 = Guid.NewGuid();
            Guid correlationId2 = Guid.NewGuid();
            Guid correlationId3 = Guid.NewGuid();
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            Command = new UpsertUserCommand(Guid.NewGuid().ToString(), DateTime.Now, correlationId1.ToString());
            CommandDoesntExist = new UpsertUserCommand(Guid.NewGuid().ToString(), DateTime.Now, Guid.NewGuid().ToString());
            CommandInvalidStatus = new UpsertUserCommand(Guid.NewGuid().ToString(), DateTime.Now, correlationId3.ToString());

            Invitation = new Invitation(correlationId1, "PRN", "Ref", "Org", "FirstName", "LastName", "Email", "Email", (int) InvitationStatus.InvitationSent, DateTime.Now, DateTime.Now);
            InvitationDoesntExist = new Invitation(correlationId2, "PRN", "Ref", "Org", "FirstName", "LastName", "Email", "Email", (int) InvitationStatus.InvitationSent, DateTime.Now, DateTime.Now);
            InvitationInvalidStatus = new Invitation(correlationId3, "PRN", "Ref", "Org", "FirstName", "LastName", "Email", "Email", (int) InvitationStatus.PayeSchemeAdded, DateTime.Now, DateTime.Now);

            Db.Invitations.Add(Invitation);
            Db.Invitations.Add(InvitationDoesntExist);
            Db.Invitations.Add(InvitationInvalidStatus);
            Db.SaveChanges();

            Handler = new UpsertUserCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
            UnitOfWorkContext = new UnitOfWorkContext();
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }

        public async Task HandleDoesntExist()
        {
            await Handler.Handle(CommandDoesntExist, CancellationToken.None);
            await Db.SaveChangesAsync();
        }

        public async Task HandleInvalidStatus()
        {
            await Handler.Handle(CommandInvalidStatus, CancellationToken.None);
            await Db.SaveChangesAsync();
        }
    }
}