CREATE TABLE [dbo].[DeletedPermissionsEventAudits]
(
	[Id] BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [AccountProviderLegalEntityId] BIGINT NOT NULL, 
    [Ukprn] BIGINT NOT NULL, 
    [Deleted] DATETIME2 NOT NULL, 
    [Logged] DATETIME2 NOT NULL
)
