CREATE TABLE [dbo].[AccountLegalEntityProvider] (
  [AccountLegalEntity_AccountLegalEntityId] BIGINT NOT NULL,
  [Provider_UKPRN] BIGINT NOT NULL,
  PRIMARY KEY ([AccountLegalEntity_AccountLegalEntityId], [Provider_UKPRN]),
  CONSTRAINT [FK_AccountLegalEntityProvider_ToAccountLegalEntities] FOREIGN KEY ([AccountLegalEntity_AccountLegalEntityId]) REFERENCES [AccountLegalEntities]([Id]),
  CONSTRAINT [FK_AccountLegalEntityProvider_ToProviders] FOREIGN KEY ([Provider_UKPRN]) REFERENCES [Providers]([UKPRN])
)
