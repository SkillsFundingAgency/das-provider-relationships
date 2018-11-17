using System;
using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Extensions;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Builders
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

        public RelationshipBuilder WithUkprn(long ukprn)
        {
            _relationship.SetPropertyTo(p => p.Ukprn, ukprn);

            return this;
        }

        public RelationshipBuilder WithAccountProviderLegalEntityId(long accountProviderLegalEntityId)
        {
            _relationship.SetPropertyTo(p => p.AccountProviderLegalEntityId, accountProviderLegalEntityId);

            return this;
        }

        public RelationshipBuilder WithAccountId(long employerAccountId)
        {
            _relationship.SetPropertyTo(p => p.AccountId, employerAccountId);
            
            return this;
        }

        public RelationshipBuilder WithAccountPublicHashedId(string employerAccountPublicHashedId)
        {
            _relationship.SetPropertyTo(p => p.AccountPublicHashedId, employerAccountPublicHashedId);
            
            return this;
        }

        public RelationshipBuilder WithAccountName(string employerAccountName)
        {
            _relationship.SetPropertyTo(p => p.AccountName, employerAccountName);
            
            return this;
        }

        public RelationshipBuilder WithAccountLegalEntityId(long employerAccountLegalEntityId)
        {
            _relationship.SetPropertyTo(p => p.AccountLegalEntityId, employerAccountLegalEntityId);
            
            return this;
        }

        public RelationshipBuilder WithAccountLegalEntityPublicHashedId(string employerAccountLegalEntityPublicHashedId)
        {
            _relationship.SetPropertyTo(p => p.AccountLegalEntityPublicHashedId, employerAccountLegalEntityPublicHashedId);
            
            return this;
        }

        public RelationshipBuilder WithAccountLegalEntityName(string employerAccountLegalEntityName)
        {
            _relationship.SetPropertyTo(p => p.AccountLegalEntityName, employerAccountLegalEntityName);
            
            return this;
        }

        public RelationshipBuilder WithAccountProviderId(int employerAccountProviderId)
        {
            _relationship.SetPropertyTo(p => p.AccountProviderId, employerAccountProviderId);
            
            return this;
        }


        public RelationshipBuilder WithOperation(Operation operation)
        {
            _relationship.SetPropertyTo(p => p.Operations, new List<Operation> { operation });
            
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

        public RelationshipBuilder WithCreated(DateTime created)
        {
            _relationship.SetPropertyTo(p => p.Created, created);

            return this;
        }

        public RelationshipBuilder WithUpdated(DateTime? updated)
        {
            _relationship.SetPropertyTo(p => p.Updated, updated);

            return this;
        }

        public Relationship Build()
        {
            return _relationship;
        }
    }
}