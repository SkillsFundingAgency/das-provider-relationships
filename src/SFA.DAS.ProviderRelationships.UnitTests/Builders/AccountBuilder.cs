using System;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.UnitTests.Builders
{
    public class AccountBuilder
    {
        private readonly Account _account = (Account)Activator.CreateInstance(typeof(Account), true);

        public AccountBuilder WithId (long id)
        {
            _account.SetPropertyTo(a => a.Id, id);

            return this;
        }

        public AccountBuilder WithName(string name)
        {
            _account.SetPropertyTo(a => a.Name, name);
            
            return this;
        }

        public AccountBuilder WithUpdated(DateTime updated)
        {
            _account.SetPropertyTo(a => a.Updated, updated);
            
            return this;
        }

        public Account Build()
        {
            return _account;
        }
        
        public static implicit operator Account(AccountBuilder builder)
        {
            return builder.Build();
        }
    }
}