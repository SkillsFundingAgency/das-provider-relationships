using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;
using Fix = SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.UpdatedLegalEntityEventHandlerTestsFixture;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers
{
    [TestFixture]
    [Parallelizable]
    public class UpdatedLegalEntityEventHandlerTests : FluentTest<UpdatedLegalEntityEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingUpdatedLegalEntityEvent_ThenShouldChangeAccountLegalEntityName()
        {
            return RunAsync(f => f.Handle(), f =>
            {
                f.AccountLegalEntity.Name.Should().Be(f.Message.Name);
                f.AccountLegalEntity.Updated.Should().Be(f.Message.Created);
            });
        }
        
        [Test]
        public Task Handle_WhenHandlingUpdatedLegalEntityEventOutOfOrder_ThenShouldNotChangeAccountLegalEntityName()
        {
            return RunAsync(f => f.SetAccountLegalEntityPreviouslyUpdated(), f => f.Handle(), f =>
            {
                f.AccountLegalEntity.Name.Should().Be(Fix.PreviouslyUpdatedName);
                f.AccountLegalEntity.Updated.Should().BeBefore(f.Now);
            });
        }
    }
    
    public class UpdatedLegalEntityEventHandlerTestsFixture : EventHandlerTestsFixture<UpdatedLegalEntityEvent>
    {
        public const string PreviouslyUpdatedName = "Previously Updated Name";
        public AccountLegalEntity AccountLegalEntity { get; set; }

        public UpdatedLegalEntityEventHandlerTestsFixture()
            : base(db => new UpdatedLegalEntityEventHandler(db))
        {
            Message = new UpdatedLegalEntityEvent
            {
                AccountLegalEntityId = 123,
                Name = "Updated Name",
                Created = DateTime.UtcNow
            };

            AccountLegalEntity = new AccountLegalEntityBuilder()
                .WithId(Message.AccountLegalEntityId)
                .WithName(Message.Name);

            Db.AccountLegalEntities.Add(AccountLegalEntity);
            Db.SaveChanges();
        }

        public UpdatedLegalEntityEventHandlerTestsFixture SetAccountLegalEntityPreviouslyUpdated()
        {
            AccountLegalEntity = new AccountLegalEntityBuilder()
                .WithId(Message.AccountLegalEntityId)
                .WithName(PreviouslyUpdatedName)
                .WithUpdated(Message.Created.AddHours(-1));
            
            return this;
        }
    }
}