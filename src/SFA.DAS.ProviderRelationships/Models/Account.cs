using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFA.DAS.ProviderRelationships.Models
{
    //todo: attributes so model matches + add othe lookup to index
    public class Account : Entity
    {
        public virtual long Id { get; protected set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public virtual string Name { get; protected set; }
        public virtual DateTime Created { get; protected set; }
        public virtual DateTime? Updated { get; protected set; }
        public virtual ICollection<AccountLegalEntity> AccountLegalEntities { get; protected set; } = new List<AccountLegalEntity>();

        public Account(long id, string name, DateTime created)
        {
            Id = id;
            Name = name;
            Created = created;
        }

        protected Account()
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