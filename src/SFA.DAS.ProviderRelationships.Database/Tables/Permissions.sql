CREATE TABLE [dbo].[Permissions]
(
    [Id] INT NOT NULL IDENTITY, 
    [Type] SMALLINT NOT NULL, 
    [AccountLegalEntityId] BIGINT NOT NULL, 
    [Ukprn] BIGINT NOT NULL,
    CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Permissions_AccountLegalEntityProvider] FOREIGN KEY ([AccountLegalEntityId], [Ukprn]) REFERENCES [AccountLegalEntityProvider]([AccountLegalEntityId], [Ukprn]) ON DELETE CASCADE,
    INDEX [IX_Permissions_AccountLegalEntityId_Ukprn] NONCLUSTERED ([AccountLegalEntityId],[Ukprn])
)
