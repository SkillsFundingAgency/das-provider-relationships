using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.Testing;
using SFA.DAS.Testing.EntityFramework;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests
{
    [TestFixture]
    public class UpdatedLegalEntityEventHandlerTests : FluentTest<UpdatedLegalEntityEventHandlerTestsFixture>
    {
        [Test]
        public async Task WhenHandlingAnUpdatedLegalEntityEvent_ThenLegalEntityShouldBeUpdated()
        {
            const long AccountLegalEntityId = 888L;
            const string OldName = "Old Name", NewName = "New Name";

            await RunAsync(f =>
                {
                    f.Db.AccountLegalEntities = new DbSetStub<AccountLegalEntity>(new AccountLegalEntity
                        { AccountLegalEntityId = AccountLegalEntityId, Name = OldName});   
                    f.Event.AccountLegalEntityId = AccountLegalEntityId;
                    f.Event.Name = NewName;
                    //f.Event.Address = NewName;
                    //todo: add AccountId
                    //todo: username & userref?
                }, f => f.Handle(),
                f =>
                {
                    f.Db.AccountLegalEntities.Should().BeEquivalentTo(new[]
                    {
                        new AccountLegalEntity
                        {
                            AccountLegalEntityId = f.Event.AccountLegalEntityId, Name = NewName
                        }
                    });
                });
        }
    }

    public class UpdatedLegalEntityEventHandlerTestsFixture : EventHandlerTestsFixture<UpdatedLegalEntityEvent, UpdatedLegalEntityEventHandler>
    {
    }
}
