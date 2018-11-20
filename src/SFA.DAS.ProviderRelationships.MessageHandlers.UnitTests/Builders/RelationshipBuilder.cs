using System;
using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.Builders
{
    internal class RelationshipBuilder
    {
        private readonly Relationship _relationship;

        public RelationshipBuilder()
        {
            _relationship = (Relationship)Activator.CreateInstance(typeof(Relationship), true);
        }

        public RelationshipBuilder WithId(Guid id)
        {
            _relationship.SetPropertyTo(p => p.Id, id);

            return this;
        }

        public RelationshipBuilder WithETag(string eTag)
        {
            _relationship.SetPropertyTo(p => p.ETag, eTag);

            return this;
        }

        public RelationshipBuilder WithCreated(DateTime created)
        {
            _relationship.SetPropertyTo(p => p.Created, created);

            return this;
        }

        public RelationshipBuilder WithAccount(Account account)
        {
            _relationship.SetPropertyTo(p => p.Account, account);

            return this;
        }

        public RelationshipBuilder WithUkprn(long ukprn)
        {
            _relationship.SetPropertyTo(p => p.Provider, new Provider(ukprn));

            return this;
        }

        public RelationshipBuilder WithAccountLegalEntity(AccountLegalEntity accountLegalEntity)
        {
            _relationship.SetPropertyTo(p => p.AccountLegalEntity, accountLegalEntity);

            return this;
        }

        public RelationshipBuilder WithAccountProvider(AccountProvider accountProvider)
        {
            _relationship.SetPropertyTo(p => p.AccountProvider, accountProvider);

            return this;
        }

        public RelationshipBuilder WithExplicitOperator(Operation operation, DateTime? updated = null)
        {
            var permissions = _relationship.AccountProvider;
            permissions.SetPropertyTo(p => p.Operations, new List<Operation> { operation });

            if (updated.HasValue)
            {
                permissions.SetPropertyTo(p => p.Updated, updated);

            }

            return this;
        }

        public RelationshipBuilder WithOutboxMessage(OutboxMessage item)
        {
            var outboxData = (List<OutboxMessage>)_relationship.OutboxData;

            outboxData.Clear();
            outboxData.Add(item);

            return this;
        }

        public RelationshipBuilder WithDeleted(DateTime deleted)
        {
            _relationship.SetPropertyTo(p => p.Deleted, deleted);

            return this;
        }

        public Relationship Build()
        {
            return _relationship;
        }
    }
}