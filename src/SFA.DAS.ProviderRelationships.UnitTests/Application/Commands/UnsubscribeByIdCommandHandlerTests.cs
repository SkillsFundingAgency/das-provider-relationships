using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands.UnsubscribeById;
using SFA.DAS.ProviderRelationships.Domain.Data;
using SFA.DAS.ProviderRelationships.Domain.Models;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class UnsubscribeByIdCommandHandlerTests : FluentTest<UnsubscribeByIdCommandHandlerTestFixture>
    {
        [Test]
        public Task Handle_WhenCommandIsHandled_ThenShouldUpdateInvitationStatus()
        {
            return RunAsync(f => f.Handle(), f =>
            {
                f.Db.Unsubscribed.SingleOrDefault(u => u.Ukprn == "PRN" && u.EmailAddress == "Email").Should().NotBeNull();
            });
        }
    }

    public class UnsubscribeByIdCommandHandlerTestFixture
    {
        public ProviderRelationshipsDbContext Db { get; set; }
        public Invitation Invitation { get; set; }
        public UnsubscribeByIdCommand Command { get; set; }
        public IUnitOfWorkContext UnitOfWorkContext { get; set; }
        public IRequestHandler<UnsubscribeByIdCommand, Unit> Handler { get; set; }
        
        public UnsubscribeByIdCommandHandlerTestFixture()
        {
            Guid correlationId = Guid.NewGuid();
           
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            Command = new UnsubscribeByIdCommand(correlationId);
            
            Invitation = new Invitation(correlationId, "PRN", "Ref", "Org", "FirstName", "LastName", "Email", "Email", (int) InvitationStatus.InvitationSent, DateTime.Now, DateTime.Now);
           
            Db.Invitations.Add(Invitation); 
            Db.SaveChanges();

            Handler = new UnsubscribeByIdCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
            UnitOfWorkContext = new UnitOfWorkContext();
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }
    }
}