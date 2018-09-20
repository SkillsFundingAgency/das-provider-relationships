namespace SFA.DAS.ProviderRelationships.Configuration
{
    public class ClaimIdentifierConfiguration : IClaimIdentifierConfiguration
    {
        public string ClaimsBaseUrl { get; set; }
        public string Id { get; set; }
        //public string GivenName { get; set; }
        ////todo: fixed spelling, change any copied config settings
        //public string FamilyName { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
    }
}