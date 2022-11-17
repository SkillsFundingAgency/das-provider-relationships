namespace SFA.DAS.ProviderRelationships.Web.Authentication.GovUk
{
    public static class DasClaimsTypesExtended
    {
        public static string Accounts => "http://das/employer/identity/claims/associatedAccounts";
        public static string UserId => "http://das/employer/identity/claims/id";
        public static string FirstName => "http://das/employer/identity/claims/given_name";
        public static string LastName => "http://das/employer/identity/claims/family_name";
    }
}