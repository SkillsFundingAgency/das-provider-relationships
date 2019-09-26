using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Domain.Models
{
    public class Provider : Entity
    {
        public long Ukprn { get; private set; }
        public string Name { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? Updated { get; private set; }
        public IEnumerable<AccountProvider> AccountProviders => _accountProviders;

        private readonly List<AccountProvider> _accountProviders = new List<AccountProvider>();

        private Provider()
        {
        }
    }
}