CREATE TABLE [dbo].[DeletedPermissionsEventAudits]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [AccountProviderLegalEntityId] BIGINT NOT NULL, 
    [Ukprn] BIGINT NOT NULL, 
    [Deleted] DATETIME2 NOT NULL, 
    [TimeLogged] DATETIME2 NOT NULL
)
