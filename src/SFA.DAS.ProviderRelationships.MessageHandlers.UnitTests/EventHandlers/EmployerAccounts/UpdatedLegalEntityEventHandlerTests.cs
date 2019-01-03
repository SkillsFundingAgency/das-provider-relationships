using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Application.Commands.UpdateAccountLegalEntityName;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.EmployerAccounts
{
    [TestFixture]
    [Parallelizable]
    public class UpdatedLegalEntityEventHandlerTests : FluentTest<UpdatedLegalEntityEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingUpdatedLegalEntityEvent_ThenShouldSendUpdateAccountLegalEntityNameCommand()
        {
            return RunAsync(f => f.Handle(), f => f.VerifySend<UpdateAccountLegalEntityNameCommand>((c, m) => 
                c.AccountLegalEntityId == m.AccountLegalEntityId && c.Name == m.Name && c.Created == m.Created));
        }
    }
    
    public class UpdatedLegalEntityEventHandlerTestsFixture : EventHandlerTestsFixture<UpdatedLegalEntityEvent, UpdatedLegalEntityEventHandler>
    {
        public UpdatedLegalEntityEventHandlerTestsFixture()
        {
            Message.AccountLegalEntityId = 1;
            Message.Name = "Foo";
        }
    }
}