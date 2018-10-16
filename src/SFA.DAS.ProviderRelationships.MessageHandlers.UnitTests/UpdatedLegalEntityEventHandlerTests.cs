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
            const long accountLegalEntityId = 888L;
            const string oldName = "Old Name", newName = "New Name";

            await RunAsync(f =>
                {
                    f.Db.AccountLegalEntities = new DbSetStub<AccountLegalEntity>(new AccountLegalEntity
                        { Id = accountLegalEntityId, Name = oldName});   
                    f.Event.AccountLegalEntityId = accountLegalEntityId;
                    f.Event.Name = newName;
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
                            Id = f.Event.AccountLegalEntityId, Name = newName
                        }
                    });
                });
        }
    }

    public class UpdatedLegalEntityEventHandlerTestsFixture : EventHandlerTestsFixture<UpdatedLegalEntityEvent, UpdatedLegalEntityEventHandler>
    {
    }
}
