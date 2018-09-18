namespace SFA.DAS.ProviderRelationships.Configuration
{
    /// <example>
    /// {
    ///   "Identity": {
    ///     "ClientId": "",
    ///     "ClientSecret": "",
    ///     "BaseAddress": "",
    ///     "AuthorizeEndPoint": "",
    ///     "LogoutEndpoint": "",
    ///     "TokenEndpoint": "",
    ///     "UserInfoEndpoint": "",
    ///     "UseCertificate": true,
    ///     "Scopes": "",
    ///     "ChangePasswordLink": "",
    ///     "ChangeEmailLink": "",
    ///     "RegisterLink": "",
    ///     "AccountActivationUrl": "",
    ///     "ClaimIdentifierConfiguration": {
    ///       "ClaimsBaseUrl": "",
    ///       "Id": "",
    ///       "Email": "",
    ///       "DisplayName": ""
    ///     }
    ///   },
    ///   "AllowedHashstringCharacters": "",
    ///   "Hashstring" : "",
    ///   "NServiceBusLicense": "",
    ///   "ServiceBusConnectionString": ""
    /// }
    /// </example>>
    public class ProviderRelationshipsConfiguration
    {
        public string AllowedHashstringCharacters { get; set; }
        public string Hashstring { get; set; }
        //public string DatabaseConnectionString { get; set; }
        public string NServiceBusLicense { get; set; }
        public string ServiceBusConnectionString { get; set; }
        public IdentityServerConfiguration Identity { get; set; }
    }
}