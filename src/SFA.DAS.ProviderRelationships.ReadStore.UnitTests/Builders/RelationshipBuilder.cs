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
            _relationship.SetPropertyTo(p => p.Permissions, new Permissions(new HashSet<Operation>()));
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

        public RelationshipBuilder WithAccountProvider(AccountProvider accountProvider)
        {
            _relationship.SetPropertyTo(p => p.AccountProvider, accountProvider);

            return this;
        }

        public RelationshipBuilder WithAccountProviderLegalEntity(AccountProviderLegalEntity accountProviderLegalEntity)
        {
            _relationship.SetPropertyTo(p => p.AccountProviderLegalEntity, accountProviderLegalEntity);

            return this;
        }

        public RelationshipBuilder WithPermissions(Permissions permissions)
        {
            _relationship.SetPropertyTo(p => p.Permissions, permissions);

            return this;
        }

        public RelationshipBuilder WithPermissionsOperator(Operation operation, DateTime? updated = null)
        {
            var permissions = _relationship.Permissions;
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

        //public Relationship2Builder WithCreated(DateTime created)
        //{
        //    _relationship.SetPropertyTo(p => p.Created, created);

        //    return this;
        //}

        //public Relationship2Builder WithUpdated(DateTime? updated)
        //{
        //    _relationship.SetPropertyTo(p => p.Updated, updated);

        //    return this;
        //}

        public Relationship Build()
        {
            return _relationship;
        }
    }
}