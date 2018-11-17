using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.EmployerAccounts
{
    [TestFixture]
    [Parallelizable]
    public class AddedLegalEntityEventHandlerTests : FluentTest<AddedLegalEntityEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingAddedLegalEntityEvent_ThenShouldAddAccountLegalEntity()
        {
            return RunAsync(f => f.Handle(), f => f.Db.AccountLegalEntities.SingleOrDefault(ale => ale.Id == f.Message.AccountLegalEntityId).Should().NotBeNull()
                .And.Match<AccountLegalEntity>(a => 
                    a.Id == f.Message.AccountLegalEntityId &&
                    a.PublicHashedId == f.Message.AccountLegalEntityPublicHashedId &&
                    a.AccountId == f.Message.AccountId &&
                    a.Name == f.Message.OrganisationName &&
                    a.Created == f.Message.Created));
        }
    }

    public class AddedLegalEntityEventHandlerTestsFixture : EventHandlerTestsFixture<AddedLegalEntityEvent>
    {
        public AddedLegalEntityEventHandlerTestsFixture()
            : base(db => new AddedLegalEntityEventHandler(db))
        {
            Message = new AddedLegalEntityEvent
            {
                AccountId = 1,
                AccountLegalEntityId = 2,
                AccountLegalEntityPublicHashedId = "ALE123",
                OrganisationName = "Foo",
                Created = DateTime.UtcNow
            };
        }
    }
}