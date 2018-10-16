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
    [Parallelizable]
    public class RemovedLegalEntityEventHandlerTests : FluentTest<RemovedLegalEntityEventHandlerTestsFixture>
    {
        //todo: test cascade delete. create graph for test

        [Test]
        public async Task WhenHandlingARemovedLegalEntityEvent_ThenLegalEntityShouldBeRemoved()
        {
            const long AccountLegalEntityId = 888L;

            await RunAsync(f =>
                {
                    f.Db.AccountLegalEntities = new DbSetStub<AccountLegalEntity>(new AccountLegalEntity
                        {Id = AccountLegalEntityId});
                    f.Event.AccountLegalEntityId = AccountLegalEntityId;
                    //todo: username & userref?
                }, f => f.Handle(),
                f =>
                {
                    f.Db.AccountLegalEntities.Should().BeEquivalentTo(new AccountLegalEntity[] { });
                });
        }
    }

    public class RemovedLegalEntityEventHandlerTestsFixture : EventHandlerTestsFixture<RemovedLegalEntityEvent, RemovedLegalEntityEventHandler>
    {
    }
}
