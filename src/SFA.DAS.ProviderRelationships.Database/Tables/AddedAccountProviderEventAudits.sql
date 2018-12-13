CREATE TABLE [dbo].[AddedAccountProviderEventAudits]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [AccountProviderId] BIGINT NOT NULL, 
    [AccountId] BIGINT NOT NULL, 
    [ProviderUkprn] BIGINT NOT NULL, 
    [UserRef] UNIQUEIDENTIFIER NOT NULL, 
    [Added] DATETIME2 NOT NULL, 
    [TimeLogged] DATETIME2 NOT NULL
)
