﻿namespace SFA.DAS.ProviderRelationships.Web.Authentication;

public static class EmployerClaims
{
    public const string IdamsUserIdClaimTypeIdentifier = "http://das/employer/identity/claims/id";
    public const string AccountsClaimsTypeIdentifier = "http://das/employer/identity/claims/associatedAccounts";
    public const string IdamsUserDisplayNameClaimTypeIdentifier = "http://das/employer/identity/claims/display_name";
    public const string IdamsUserEmailClaimTypeIdentifier = "http://das/employer/identity/claims/email_address";
    
    public const string GivenName = "http://das/employer/identity/claims/given_name";
    public const string FamilyName = "http://das/employer/identity/claims/family_name";
}