using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Data;
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
    public class CreatedAccountEventHandlerTests : FluentTest<CreatedAccountEventHandlerTestsFixture>
    {
        //todo: wtf is AutoData stopping the test from running?
        //[Test, AutoDataFixedName]
        //public async Task X(CreatedAccountEvent createdAccountEvent)
        [Test]
        public async Task X()
        {
            var createdAccountEvent = new Fixture().Create<CreatedAccountEvent>();
            //var createdAccountEvent = new CreatedAccountEvent
            //{
            //    AccountId = 10,
            //    Created = new DateTime(2020, 1, 1),
            //    Name = "Account Name",
            //    PublicHashedId = "123456",
            //    UserName = "User Name",
            //    UserRef = Guid.NewGuid()
            //};

            //todo: clone in DbSetStub or test?
            await RunAsync(f => f.Handle(createdAccountEvent), f => f.Db.Accounts.Should().BeEquivalentTo(new[] {new Account {AccountId = createdAccountEvent.AccountId, Name = createdAccountEvent.Name, PublicHashedId = createdAccountEvent .PublicHashedId} }));
        }
    }

    public class CreatedAccountEventHandlerTestsFixture
    {
        public IHandleMessages<CreatedAccountEvent> Handler { get; set; }
        //public Mock<ProviderRelationshipsDbContext> Db { get; set; }
        public IProviderRelationshipsDbContext Db { get; set; }
        public List<Account> Accounts { get; set; }
        public IMessageHandlerContext MessageHandlerContext { get; }

        public CreatedAccountEventHandlerTestsFixture()
        {
            //Db = new Mock<ProviderRelationshipsDbContext>();
            Db = new TestProviderRelationshipsDbContext();

            //Db.Setup(d => d.Accounts).Returns(new DbSetStubX<Account>());

            MessageHandlerContext = new TestableMessageHandlerContext();

            //Handler = new CreatedAccountEventHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db.Object), Mock.Of<ILog>());
            Handler = new CreatedAccountEventHandler(new Lazy<IProviderRelationshipsDbContext>(() => Db), Mock.Of<ILog>());
        }

        public Task Handle(CreatedAccountEvent createdAccountEvent)
        {
            return Handler.Handle(createdAccountEvent, MessageHandlerContext);
        }
    }
    //base for boilerplate?
    public class TestProviderRelationshipsDbContext : IProviderRelationshipsDbContext
    {
        public TestProviderRelationshipsDbContext()
        {
            Accounts = new DbSetStubX<Account>();
            AccountLegalEntities = new DbSetStubX<AccountLegalEntity>();
            Permissions = new DbSetStubX<Permission>();
            HealthChecks = new DbSetStubX<HealthCheck>();
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountLegalEntity> AccountLegalEntities { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<HealthCheck> HealthChecks { get; set; }

        public int SaveChangesCount { get; private set; }

        public int SaveChanges()
        {
            ++SaveChangesCount;
            return 1;
        }

        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult(SaveChanges());
        }

        public void Delete(object entity)
        {
            switch (entity)
            {
                case Account account:
                    Accounts.Remove(account);
                    break;
                case AccountLegalEntity accountLegalEntity:
                    AccountLegalEntities.Remove(accountLegalEntity);
                    break;
                case Permission permission:
                    Permissions.Remove(permission);
                    break;
                case HealthCheck healthCheck:
                    HealthChecks.Remove(healthCheck);
                    break;
            }
        }
    }

    /// <remarks>
    /// See https://docs.microsoft.com/en-us/ef/ef6/fundamentals/testing/writing-test-doubles
    /// </remarks>
    public class DbSetStubX<T> : DbSet<T>, IDbAsyncEnumerable<T>, IQueryable<T> where T : class
    {
        public Expression Expression => _query.Expression;
        public Type ElementType => _query.ElementType;
        public override ObservableCollection<T> Local => _local;
        public IQueryProvider Provider => new DbAsyncQueryProviderStub<T>(_query.Provider);

        private readonly IQueryable<T> _query;
        private readonly ObservableCollection<T> _local;

        public DbSetStubX(params T[] data) : this(data.AsEnumerable())
        {
        }

        public DbSetStubX(IEnumerable<T> data)
        {
            _query = data.AsQueryable();
            _local = new ObservableCollection<T>(_query);
        }

        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new DbAsyncEnumeratorStub<T>(_local.GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _local.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region X

        public override T Add(T item)
        {
            _local.Add(item);
            return item;
        }

        public override T Remove(T item)
        {
            _local.Remove(item);
            return item;
        }

        public override T Attach(T item)
        {
            _local.Add(item);
            return item;
        }

        public override T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public override TDerivedEntity Create<TDerivedEntity>()
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        #endregion X
    }

    //public static class TestHelper
    //{
    //    public static T Clone<T>(T toClone)
    //    {

    //    }
    //}
}
