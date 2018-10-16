using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.Testing;
using SFA.DAS.Testing.EntityFramework;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests
{
    //https://github.com/AutoFixture/AutoFixture/wiki/Known-Issues#test-name-strategies-for-nunit3
    //public class AutoDataFixedName : AutoDataAttribute
    //{
    //    public AutoDataFixedName()
    //    {
    //        TestMethodBuilder = new FixedNameTestMethodBuilder();
    //    }
    //}

    [TestFixture]
    [Parallelizable]
    public class CreatedAccountEventHandlerTests : FluentTest<CreatedAccountEventHandlerTestsFixture>
    {
        //todo: wtf is AutoData stopping the test from running?
        //[Test, AutoDataFixedName]
        //public async Task X(CreatedAccountEvent createdAccountEvent)
        [Test]
        public async Task WhenHandlingACreatedAccountEvent_ThenAccountShouldBeAddedToDb()
        {
            //todo: clone in DbSetStub or test?
            await RunAsync(f => f.Event = new Fixture().Create<CreatedAccountEvent>(),
                f => f.Handle(), 
                f =>
                {
                    f.Db.AccountsAtLastSaveChanges.Should().BeEquivalentTo(new[]
                    {
                        new Account
                        {
                            Id = f.Event.AccountId, Name = f.Event.Name,
                        }
                    });
                });
        }
    }

    public class CreatedAccountEventHandlerTestsFixture : EventHandlerTestsFixture<CreatedAccountEvent, CreatedAccountEventHandler>
    {
    }
}
