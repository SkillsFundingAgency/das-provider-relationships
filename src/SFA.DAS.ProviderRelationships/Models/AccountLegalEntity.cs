﻿using System;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class AccountLegalEntity : Entity
    {
        public virtual long Id { get; protected set; }
        public virtual string PublicHashedId { get; protected set; }
        public virtual Account Account { get; protected set; }
        public virtual long AccountId { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual DateTime Created { get; protected set; }
        public virtual DateTime? Updated { get; protected set; }
        
        public AccountLegalEntity(long id, string publicHashedId, long accountId, string name, DateTime created)
        {
            Id = id;
            PublicHashedId = publicHashedId;
            AccountId = accountId;
            Name = name;
            Created = created;
        }

        protected AccountLegalEntity()
        {
        }

        public void ChangeName(string name, DateTime changed)
        {
            if (Updated == null || changed > Updated.Value)
            {
                Name = name;
                Updated = changed;
            }
        }
    }
}
