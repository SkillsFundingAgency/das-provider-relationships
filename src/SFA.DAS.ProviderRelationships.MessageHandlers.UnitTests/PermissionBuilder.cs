using System;
using System.Collections.Generic;
using Moq;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.Types.Models;

// TODO This needs to go into a shared Testing library
namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests
{
    internal class PermissionBuilder
    {
        private readonly Mock<Permission> _permission = new Mock<Permission> { CallBase = true };

        public PermissionBuilder WithId(Guid? id)
        {
            _permission.SetupProperty(p => p.Id, id);

            return this;
        }

        public PermissionBuilder WithETag(string eTag)
        {
            _permission.SetupProperty(p => p.ETag, eTag);

            return this;
        }

        public PermissionBuilder WithUkprn(long ukprn)
        {
            _permission.SetupProperty(p => p.Ukprn, ukprn);

            return this;
        }

        public PermissionBuilder WithAccountProviderLegalEntityId(long accountProviderLegalEntityId)
        {
            _permission.SetupProperty(p => p.AccountProviderLegalEntityId, accountProviderLegalEntityId);

            return this;
        }

        public PermissionBuilder WithAccountId(long employerAccountId)
        {
            _permission.SetupProperty(p => p.AccountId, employerAccountId);
            
            return this;
        }

        public PermissionBuilder WithAccountPublicHashedId(string employerAccountPublicHashedId)
        {
            _permission.SetupProperty(p => p.AccountPublicHashedId, employerAccountPublicHashedId);
            
            return this;
        }

        public PermissionBuilder WithAccountName(string employerAccountName)
        {
            _permission.SetupProperty(p => p.AccountName, employerAccountName);
            
            return this;
        }

        public PermissionBuilder WithAccountLegalEntityId(long employerAccountLegalEntityId)
        {
            _permission.SetupProperty(p => p.AccountLegalEntityId, employerAccountLegalEntityId);
            
            return this;
        }

        public PermissionBuilder WithAccountLegalEntityPublicHashedId(string employerAccountLegalEntityPublicHashedId)
        {
            _permission.SetupProperty(p => p.AccountLegalEntityPublicHashedId, employerAccountLegalEntityPublicHashedId);
            
            return this;
        }

        public PermissionBuilder WithAccountLegalEntityName(string employerAccountLegalEntityName)
        {
            _permission.SetupProperty(p => p.AccountLegalEntityName, employerAccountLegalEntityName);
            
            return this;
        }

        public PermissionBuilder WithAccountProviderId(int employerAccountProviderId)
        {
            _permission.SetupProperty(p => p.AccountProviderId, employerAccountProviderId);
            
            return this;
        }


        public PermissionBuilder WithOperation(Operation operation)
        {
            _permission.SetupProperty(p => p.Operations, new List<Operation> { operation });
            
            return this;
        }

        public PermissionBuilder WithOutboxMessage(OutboxDataItem item)
        {
            _permission.SetupProperty(p => p.OutboxData, new List<OutboxDataItem> { item });

            return this;
        }

        public PermissionBuilder WithDeleted(DateTime deleted)
        {
            _permission.SetupProperty(p => p.Deleted, deleted);

            return this;
        }

        public PermissionBuilder WithCreated(DateTime created)
        {
            _permission.SetupProperty(p => p.Created, created);

            return this;
        }



        public Permission Build()
        {
            return _permission.Object;
        }
    }
}