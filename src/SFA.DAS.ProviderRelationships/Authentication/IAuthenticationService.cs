﻿namespace SFA.DAS.ProviderRelationships.Authentication
{
    public interface IAuthenticationService
    {
        string GetCurrentUserClaimValue(string key);
        bool IsUserAuthenticated();
        void SignOutUser();
        bool TryGetCurrentUserClaimValue(string key, out string value);
    }
}
