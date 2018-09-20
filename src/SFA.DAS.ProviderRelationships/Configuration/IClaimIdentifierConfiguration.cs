using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ProviderRelationships.Configuration
{
    public interface IClaimIdentifierConfiguration
    {
        string ClaimsBaseUrl { get; }
        string Id { get; }
        //string GivenName { get; }
        ////todo: fixed spelling, change any copied config settings
        //string FamilyName { get; }
        string Email { get; }
        string DisplayName { get; }
    }
}
