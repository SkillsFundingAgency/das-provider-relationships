CREATE TABLE [dbo].[UpdatedPermissionsEventAudits]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [AccountId] BIGINT NOT NULL, 
    [AccountLegalEntityId] BIGINT NOT NULL, 
    [AccountProviderId] BIGINT NOT NULL, 
    [AccountProviderLegalEntityId] BIGINT NOT NULL, 
    [Ukprn] BIGINT NOT NULL, 
    [UserRef] UNIQUEIDENTIFIER NOT NULL, 
    [GrantedOperations] NVARCHAR(255) NOT NULL, 
    [Updated] DATETIME NOT NULL, 
    [TimeLogged] DATETIME NOT NULL
)
