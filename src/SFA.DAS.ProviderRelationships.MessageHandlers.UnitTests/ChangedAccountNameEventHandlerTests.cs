using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.Testing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests
{
    [TestFixture]
    public class ChangedAccountNameEventHandlerTests : FluentTest<ChangedAccountNameEventHandlerTestsFixture>
    {
    }

    public class ChangedAccountNameEventHandlerTestsFixture : EventHandlerTestsFixture
    {
        public IHandleMessages<ChangedAccountNameEvent> Handler { get; set; }
        public List<Account> Accounts { get; set; }

        public ChangedAccountNameEventHandlerTestsFixture()
            : base()
        {
            Handler = new ChangedAccountNameEventHandler(new Lazy<IProviderRelationshipsDbContext>(() => Db), Mock.Of<ILog>());
        }

        public Task Handle(ChangedAccountNameEvent changedAccountNameEvent)
        {
            return Handler.Handle(changedAccountNameEvent, MessageHandlerContext);
        }
    }
}
    