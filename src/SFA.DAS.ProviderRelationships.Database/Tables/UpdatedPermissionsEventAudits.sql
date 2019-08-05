CREATE TABLE [dbo].[UpdatedPermissionsEventAudits]
(
	[Id] BIGINT IDENTITY(1,1) NOT NULL, 
    [AccountId] BIGINT NOT NULL, 
    [AccountLegalEntityId] BIGINT NOT NULL, 
    [AccountProviderId] BIGINT NOT NULL, 
    [AccountProviderLegalEntityId] BIGINT NOT NULL, 
    [Ukprn] BIGINT NOT NULL, 
    [UserRef] UNIQUEIDENTIFIER NULL, 
    [GrantedOperations] NVARCHAR(MAX) NOT NULL, 
    [Updated] DATETIME2 NOT NULL, 
    [Logged] DATETIME2 NOT NULL,
    CONSTRAINT [PK_UpdatedPermissionsEventAudits] PRIMARY KEY CLUSTERED ([Id] ASC)
)
