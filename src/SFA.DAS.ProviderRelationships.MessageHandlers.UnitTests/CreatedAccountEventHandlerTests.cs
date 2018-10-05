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
using CloneExtensions;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
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
        public async Task WhenHandlingACreatedAccountEvent_ThenAccountShouldBeAddedToDb()
        {
            var createdAccountEvent = new Fixture().Create<CreatedAccountEvent>();

            //todo: clone in DbSetStub or test?
            await RunAsync(f => f.Handle(createdAccountEvent), 
                f =>
                {
                    f.Db.AccountsAtLastSaveChanges.Should().BeEquivalentTo(new[]
                    //f.Db.Accounts.Should().BeEquivalentTo(new[]
                    {
                        new Account
                        {
                            AccountId = createdAccountEvent.AccountId, Name = createdAccountEvent.Name,
                            PublicHashedId = createdAccountEvent.PublicHashedId
                        }
                    });
                    //todo: doesn't guarantee savechanges was called after adding. have array of dbsets at savechanges time?
                    f.Db.SaveChangesCount.Should().Be(1);
                });
        }
    }

    public class CreatedAccountEventHandlerTestsFixture
    {
        public IHandleMessages<CreatedAccountEvent> Handler { get; set; }
        public TestProviderRelationshipsDbContext Db { get; set; }
        public List<Account> Accounts { get; set; }
        public IMessageHandlerContext MessageHandlerContext { get; }

        public CreatedAccountEventHandlerTestsFixture()
        {
            Db = new TestProviderRelationshipsDbContext();

            MessageHandlerContext = new TestableMessageHandlerContext();

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

        //no need for copy at each save
        //public List<DbSet<Account>> AccountsAtSaveChanges;

        //public DbSet<Account> AccountsAtLastSaveChanges;

        public IEnumerable<Account> AccountsAtLastSaveChanges;
        public IEnumerable<AccountLegalEntity> AccountLegalEntitiesAtLastSaveChanges;
        public IEnumerable<Permission> PermissionsAtLastSaveChanges;
        public IEnumerable<HealthCheck> HealthChecksAtLastSaveChanges;

        public int SaveChanges()
        {
            //AccountsAtLastSaveChanges = TestHelper.Clone(Accounts);
            //AccountsAtLastSaveChanges = Accounts.CloneJson();
            //AccountsAtLastSaveChanges = Accounts.GetClone();
            AccountsAtLastSaveChanges = new List<Account>(Accounts).GetClone();//((DbSetStubX<Account>) Accounts).GetClone();
            AccountLegalEntitiesAtLastSaveChanges = new List<AccountLegalEntity>(AccountLegalEntities).GetClone();
            PermissionsAtLastSaveChanges = new List<Permission>(Permissions).GetClone();
            HealthChecksAtLastSaveChanges = new List<HealthCheck>(HealthChecks).GetClone();

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
            //todo: clone on the way in?
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

    public static class TestHelper
    {
        //public static T Clone<T>(T source)
        //{
        //    var serialized = JsonConvert.SerializeObject(source);
        //    return JsonConvert.DeserializeObject<T>(serialized);
        //}

        public static T CloneJson<T>(this T source)
        {
            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            // initialize inner objects individually
            // for example in default constructor some list property initialized with some values,
            // but in 'source' these items are cleaned -
            // without ObjectCreationHandling.Replace default constructor values will be added to result
            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
        }
    }
}
