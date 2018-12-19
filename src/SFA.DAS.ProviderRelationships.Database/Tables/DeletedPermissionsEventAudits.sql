CREATE TABLE [dbo].[DeletedPermissionsEventAudits]
(
	[Id] BIGINT IDENTITY(1,1) NOT NULL, 
    [AccountProviderLegalEntityId] BIGINT NOT NULL, 
    [Ukprn] BIGINT NOT NULL, 
    [Deleted] DATETIME2 NOT NULL, 
    [Logged] DATETIME2 NOT NULL,
    CONSTRAINT [PK_DeletedPermissionsEventAudits] PRIMARY KEY CLUSTERED ([Id] ASC)
)
