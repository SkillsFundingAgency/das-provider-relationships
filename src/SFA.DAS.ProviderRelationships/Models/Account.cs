using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class Account : Entity
    {
        public virtual long Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual DateTime Created { get; protected set; }
        public virtual DateTime CreatedAt { get; protected set; }
        public virtual DateTime? Updated { get; protected set; }
        public virtual IEnumerable<AccountLegalEntity> AccountLegalEntities { get; protected set; } = new List<AccountLegalEntity>();

        public Account(long id, string name, DateTime created)
        {
            Id = id;
            Name = name;
            Created = created;
            CreatedAt = DateTime.UtcNow;
        }
    }
}