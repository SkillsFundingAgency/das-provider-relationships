using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers
{
    [TestFixture]
    [Parallelizable]
    public class CreatedAccountEventHandlerTests : FluentTest<CreatedAccountEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingCreatedAccountEvent_ThenShouldAddAccount()
        {
            return RunAsync(f => f.Handle(), f => f.Db.Accounts.SingleOrDefault(a => a.Id == f.Message.AccountId).Should().NotBeNull()
                .And.Match<Account>(a => 
                    a.Id == f.Message.AccountId &&
                    a.Name == f.Message.Name &&
                    a.Created == f.Message.Created));
        }
    }

    public class CreatedAccountEventHandlerTestsFixture : EventHandlerTestsFixture<CreatedAccountEvent>
    {
        public CreatedAccountEventHandlerTestsFixture()
            : base(ldb => new CreatedAccountEventHandler(ldb))
        {
            Message = new CreatedAccountEvent
            {
                AccountId = 123,
                Name = "Acme",
                Created = DateTime.UtcNow
            };
        }
    }
}