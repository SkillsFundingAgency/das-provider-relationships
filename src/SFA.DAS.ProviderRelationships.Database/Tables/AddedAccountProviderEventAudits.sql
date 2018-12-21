CREATE TABLE [dbo].[AddedAccountProviderEventAudits]
(
	[Id] BIGINT IDENTITY(1,1) NOT NULL, 
    [AccountProviderId] BIGINT NOT NULL, 
    [AccountId] BIGINT NOT NULL, 
    [ProviderUkprn] BIGINT NOT NULL, 
    [UserRef] UNIQUEIDENTIFIER NOT NULL, 
    [Added] DATETIME2 NOT NULL, 
    [Logged] DATETIME2 NOT NULL,
    CONSTRAINT [PK_AddedAccountProviderEventAudits] PRIMARY KEY CLUSTERED ([Id] ASC)
)
