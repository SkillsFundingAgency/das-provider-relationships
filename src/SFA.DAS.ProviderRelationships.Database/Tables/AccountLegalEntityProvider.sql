CREATE TABLE [dbo].[AccountLegalEntityProvider] (
    [AccountLegalEntityId] BIGINT NOT NULL,
    [Ukprn] BIGINT NOT NULL,
    CONSTRAINT PK_AccountLegalEntityProvider PRIMARY KEY CLUSTERED ([AccountLegalEntityId], [Ukprn]),
    CONSTRAINT [FK_AccountLegalEntityProvider_AccountLegalEntities_AccountLegalEntityId] FOREIGN KEY ([AccountLegalEntityId]) REFERENCES [AccountLegalEntities]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AccountLegalEntityProvider_ToProviders] FOREIGN KEY ([Ukprn]) REFERENCES [Providers]([Ukprn])
);
GO

CREATE NONCLUSTERED INDEX [IX_AccountLegalEntityProvider_Ukprn] ON [dbo].[AccountLegalEntityProvider] ([Ukprn] ASC) INCLUDE ([AccountLegalEntityId]);
GO
