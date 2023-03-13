namespace SFA.DAS.ProviderRelationships.Web.Authentication
{
    public static class EmployerClaimTypes
    {
        public static string UserId => "http://das/employer/identity/claims/id";
        public static string EmailAddress => "http://das/employer/identity/claims/email_address";
        public static string GivenName => "http://das/employer/identity/claims/given_name";
        public static string FamilyName => "http://das/employer/identity/claims/family_name";
        public static string DisplayName => "http://das/employer/identity/claims/display_name";
        public static string Account => "http://das/employer/identity/claims/account";
        public static string AssociatedAccounts => "http://das/employer/identity/claims/associatedAccounts";

        public static string IdamsUserIdClaimTypeIdentifier => "http://das/employer/identity/claims/id";
        public static string AccountsClaimsTypeIdentifier => "http://das/employer/identity/claims/associatedAccounts";
        public static string IdamsUserDisplayNameClaimTypeIdentifier = "http://das/employer/identity/claims/display_name";
        public static string IdamsUserEmailClaimTypeIdentifier = "http://das/employer/identity/claims/email_address";
    }
}