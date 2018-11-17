using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers
{
    [TestFixture]
    [Parallelizable]
    public class UpdatedLegalEntityEventHandlerTests : FluentTest<UpdatedLegalEntityEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenEventIsHandledChronologically_ThenShouldChangeAccountLegalEntityName()
        {
            return RunAsync(f => f.Handle(), f =>
            {
                f.AccountLegalEntity.Name.Should().Be(f.Message.Name);
                f.AccountLegalEntity.Updated.Should().Be(f.Message.Created);
            });
        }
        
        [Test]
        public Task Handle_WhenEventIsHandledNonChronologically_ThenShouldNotChangeAccountLegalEntityName()
        {
            return RunAsync(f => f.SetAccountLegalEntityUpdatedAfterEvent(), f => f.Handle(), f =>
            {
                f.AccountLegalEntity.Name.Should().Be(f.OriginalAccountLegalEntityName);
                f.AccountLegalEntity.Updated.Should().Be(f.Now);
            });
        }
    }
    
    public class UpdatedLegalEntityEventHandlerTestsFixture : EventHandlerTestsFixture<UpdatedLegalEntityEvent>
    {
        public string OriginalAccountLegalEntityName { get; set; }
        public AccountLegalEntity AccountLegalEntity { get; set; }

        public UpdatedLegalEntityEventHandlerTestsFixture()
            : base(db => new UpdatedLegalEntityEventHandler(db))
        {
            OriginalAccountLegalEntityName = "Foo";
            
            Message = new UpdatedLegalEntityEvent
            {
                AccountLegalEntityId = 1,
                Name = "Bar",
                Created = Now.AddHours(-1)
            };

            AccountLegalEntity = new AccountLegalEntityBuilder()
                .WithId(Message.AccountLegalEntityId)
                .WithName(OriginalAccountLegalEntityName);

            Db.AccountLegalEntities.Add(AccountLegalEntity);
            Db.SaveChanges();
        }

        public UpdatedLegalEntityEventHandlerTestsFixture SetAccountLegalEntityUpdatedAfterEvent()
        {
            AccountLegalEntity.SetPropertyTo(ale => ale.Updated, Now);
            Db.SaveChanges();
            
            return this;
        }
    }
}