using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Moq;
using NServiceBus;
using SFA.DAS.NServiceBus;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using Permission = SFA.DAS.ProviderRelationships.ReadStore.Models.Permission;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers
{
    internal abstract class DocumentEventHandlerTestsFixture<TEvent> where TEvent : Event
    {
        internal Mock<IPermissionsRepository> PermissionsRepository;
        internal Mock<IMessageHandlerContext> MessageHandlerContext;

        internal List<Permission> Permissions;
        internal FakeDocumentQuery<Permission> DocumentQuery;

        public IHandleMessages<TEvent> Handler { get; set; }
        public TEvent Message { get; set; }

        protected DocumentEventHandlerTestsFixture(Func<IPermissionsRepository, IHandleMessages<TEvent>> createEventHandler)
        {
            MessageHandlerContext = new Mock<IMessageHandlerContext>();

            PermissionsRepository = new Mock<IPermissionsRepository>();
            Permissions = new List<Permission>();
            DocumentQuery = new FakeDocumentQuery<Permission>(Permissions);
            PermissionsRepository.Setup(r => r.CreateQuery(null)).Returns(DocumentQuery);

            Handler = createEventHandler(PermissionsRepository.Object);
        }

        public virtual async Task Handle()
        {
            await Handler.Handle(Message, MessageHandlerContext.Object);
        }
        public DocumentEventHandlerTestsFixture<TEvent> SetMessageIdInContext(string messageId)
        {
            MessageHandlerContext.Setup(x => x.MessageId).Returns(messageId);
            return this;
        }


    }





    /*

    Temp Location for Fakes until we start using the Shared Packages

    */

    public class FakeDocumentQuery<T> : IDocumentQuery<T>, IOrderedQueryable<T>
    {
        public Expression Expression => _query.Expression;
        public Type ElementType => _query.ElementType;
        public bool HasMoreResults => ++_page <= _pages;
        public IQueryProvider Provider => new FakeDocumentQueryProvider<T>(_query.Provider);

        private readonly IQueryable<T> _query;
        private readonly int _pages = 1;
        private int _page = 0;

        public FakeDocumentQuery(IEnumerable<T> data)
        {
            _query = data.AsQueryable();
        }

        public Task<FeedResponse<TResult>> ExecuteNextAsync<TResult>(CancellationToken token = default)
        {
            return Task.FromResult(new FeedResponse<TResult>(this.Cast<TResult>()));
        }

        public Task<FeedResponse<dynamic>> ExecuteNextAsync(CancellationToken token = default)
        {
            return Task.FromResult(new FeedResponse<dynamic>(this.Cast<dynamic>()));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _query.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
        }
    }


    public class FakeDocumentQueryProvider<T> : IQueryProvider
    {
        private readonly IQueryProvider _queryProvider;

        public FakeDocumentQueryProvider(IQueryProvider queryProvider)
        {
            _queryProvider = queryProvider;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new FakeDocumentQuery<T>(_queryProvider.CreateQuery<T>(expression));
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new FakeDocumentQuery<TElement>(_queryProvider.CreateQuery<TElement>(expression));
        }

        public object Execute(Expression expression)
        {
            return _queryProvider.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _queryProvider.Execute<TResult>(expression);
        }
    }
}