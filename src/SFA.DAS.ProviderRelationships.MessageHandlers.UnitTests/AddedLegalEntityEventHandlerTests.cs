using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.Testing;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class AddedLegalEntityEventHandlerTests : FluentTest<AddedLegalEntityEventHandlerTestsFixture>
    {
        [Test]
        public async Task WhenHandlingAAddedLegalEntityEvent_ThenLegalEntityShouldBeAddedToDb()
        {
            await RunAsync(f => f.Event = new Fixture().Create<AddedLegalEntityEvent>(),
                f => f.Handle(),
                f =>
                {
                    f.Db.AccountLegalEntitiesAtLastSaveChanges.Should().BeEquivalentTo(new[]
                    {
                        new AccountLegalEntity
                        {
                            Id = f.Event.AccountLegalEntityId,
                            Name = f.Event.OrganisationName,
                            AccountId = f.Event.AccountId,
                            PublicHashedId = f.Event.AccountLegalEntityPublicHashedId
                        }
                    });
                });
        }
    }

    public class AddedLegalEntityEventHandlerTestsFixture : EventHandlerTestsFixture<AddedLegalEntityEvent, AddedLegalEntityEventHandler>
    {
    }
}
