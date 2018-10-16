using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.Testing;
using System.Threading.Tasks;
using FluentAssertions;
using SFA.DAS.Testing.EntityFramework;
using SFA.DAS.EmployerAccounts.Messages.Events;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class ChangedAccountNameEventHandlerTests : FluentTest<ChangedAccountNameEventHandlerTestsFixture>
    {
        [Test]
        public async Task WhenHandlingAChangedAccountNameEvent_ThenAccountNameShouldBeUpdated()
        {
            const long accountId = 54321;
            const string oldName = "Old Name", newName = "New Name";

            //todo: clone in DbSetStub or test?
            await RunAsync(f =>
                {
                    f.Db.Accounts = new DbSetStub<Account>(new Account {Id = accountId, Name = oldName});
                    f.Event.AccountId = accountId;
                    f.Event.PreviousName = oldName;
                    f.Event.CurrentName = newName;
                }, f => f.Handle(),
                f =>
                {
                    f.Db.AccountsAtLastSaveChanges.Should().BeEquivalentTo(new[]
                    {
                        new Account
                        {
                            Id = f.Event.AccountId, Name = newName
                        }
                    });
                });
        }
    }

    public class ChangedAccountNameEventHandlerTestsFixture : EventHandlerTestsFixture<ChangedAccountNameEvent, ChangedAccountNameEventHandler>
    {
    }
}
    