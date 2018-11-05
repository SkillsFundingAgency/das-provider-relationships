using System;
using Moq;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.UnitTests
{
    public class AccountBuilder
    {
        private readonly Mock<Account> _account = new Mock<Account> { CallBase = true };

        public AccountBuilder()
        {
            _account.SetupAllProperties();
        }

        public AccountBuilder WithId (long id)
        {
            _account.SetupProperty(a => a.Id, id);

            return this;
        }

        public AccountBuilder WithName(string name)
        {
            _account.SetupProperty(a => a.Name, name);
            
            return this;
        }

        public AccountBuilder WithUpdated(DateTime updated)
        {
            _account.SetupProperty(a => a.Updated, updated);
            
            return this;
        }

        public Account Build()
        {
            return _account.Object;
        }
        
        public static implicit operator Account(AccountBuilder builder)
        {
            return builder.Build();
        }
    }
}