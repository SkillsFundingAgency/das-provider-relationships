CREATE TABLE [dbo].[DeletedPermissionsEventAudits]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [AccountProviderLegalEntityId] BIGINT NOT NULL, 
    [Ukprn] BIGINT NOT NULL, 
    [Deleted] DATETIME NOT NULL, 
    [TimeLogged] DATETIME NOT NULL
)
