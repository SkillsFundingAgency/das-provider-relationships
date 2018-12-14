﻿CREATE TABLE [dbo].[UpdatedPermissionsEventAudits]
(
	[Id] BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [AccountId] BIGINT NOT NULL, 
    [AccountLegalEntityId] BIGINT NOT NULL, 
    [AccountProviderId] BIGINT NOT NULL, 
    [AccountProviderLegalEntityId] BIGINT NOT NULL, 
    [Ukprn] BIGINT NOT NULL, 
    [UserRef] UNIQUEIDENTIFIER NOT NULL, 
    [GrantedOperations] NVARCHAR(255) NOT NULL, 
    [Updated] DATETIME2 NOT NULL, 
    [Logged] DATETIME2 NOT NULL
)
