using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.Testing;
using System.Threading.Tasks;
using FluentAssertions;
using SFA.DAS.Testing.EntityFramework;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests
{
    [TestFixture]
    public class ChangedAccountNameEventHandlerTests : FluentTest<ChangedAccountNameEventHandlerTestsFixture>
    {
        [Test]
        public async Task WhenHandlingAChangedAccountNameEvent_ThenAccountNameShouldBeUpdated()
        {
            const long AccountId = 54321;
            const string OldName = "Old Name", NewName = "New Name";

            //todo: clone in DbSetStub or test?
            await RunAsync(f =>
                {
                    f.Db.Accounts = new DbSetStub<Account>(new Account {AccountId = AccountId, Name = OldName});
                    f.Event.AccountId = AccountId;
                    f.Event.PreviousName = OldName;
                    f.Event.CurrentName = NewName;
                }, f => f.Handle(),
                f =>
                {
                    f.Db.AccountsAtLastSaveChanges.Should().BeEquivalentTo(new[]
                    {
                        new Account
                        {
                            AccountId = f.Event.AccountId, Name = NewName
                        }
                    });
                });
        }
    }

    public class ChangedAccountNameEventHandlerTestsFixture : EventHandlerTestsFixture<ChangedAccountNameEvent, ChangedAccountNameEventHandler>
    {
    }
}
    