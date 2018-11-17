using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.EmployerAccounts
{
    [TestFixture]
    [Parallelizable]
    public class UpdatedLegalEntityEventHandlerTests : FluentTest<UpdatedLegalEntityEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenEventIsHandledChronologically_ThenShouldUpdateAccountLegalEntityName()
        {
            return RunAsync(f => f.Handle(), f =>
            {
                f.AccountLegalEntity.Name.Should().Be(f.Message.Name);
                f.AccountLegalEntity.Updated.Should().Be(f.Message.Created);
            });
        }
        
        [Test]
        public Task Handle_WhenEventIsHandledChronologically_ThenShouldPublishUpdatedAccountLegalEntityNameEvent()
        {
            return RunAsync(f => f.Handle(), f => f.UnitOfWorkContext.GetEvents().SingleOrDefault().Should().NotBeNull()
                .And.Match<UpdatedAccountLegalEntityNameEvent>(e =>
                    e.AccountLegalEntityId == f.AccountLegalEntity.Id &&
                    e.AccountId == f.AccountLegalEntity.AccountId &&
                    e.Name == f.AccountLegalEntity.Name &&
                    e.Created == f.AccountLegalEntity.Updated));
        }
        
        [Test]
        public Task Handle_WhenEventIsHandledNonChronologically_ThenShouldNotUpdateAccountLegalEntityName()
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
        public IUnitOfWorkContext UnitOfWorkContext { get; set; }

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
                .WithAccountId(2)
                .WithName(OriginalAccountLegalEntityName);

            Db.AccountLegalEntities.Add(AccountLegalEntity);
            Db.SaveChanges();

            UnitOfWorkContext = new UnitOfWorkContext();
        }

        public UpdatedLegalEntityEventHandlerTestsFixture SetAccountLegalEntityUpdatedAfterEvent()
        {
            AccountLegalEntity.SetPropertyTo(ale => ale.Updated, Now);
            Db.SaveChanges();
            
            return this;
        }
    }
}