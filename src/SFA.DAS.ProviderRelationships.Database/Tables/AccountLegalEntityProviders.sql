CREATE TABLE [dbo].[AccountLegalEntityProviders] (
  [AccountLegalEntity_AccountLegalEntityId] BIGINT NOT NULL,
  [Provider_UKPRN] BIGINT NOT NULL,
  PRIMARY KEY ([AccountLegalEntity_AccountLegalEntityId], [Provider_UKPRN]),
  CONSTRAINT [FK_AccountLegalEntityProviders_ToAccountLegalEntities] FOREIGN KEY ([AccountLegalEntity_AccountLegalEntityId]) REFERENCES [AccountLegalEntities]([Id]),
  CONSTRAINT [FK_AccountLegalEntityProviders_ToProviders] FOREIGN KEY ([Provider_UKPRN]) REFERENCES [Providers]([UKPRN])
)

-- CREATE TABLE [dbo].[AccountLegalEntityProviders] (
--   [AccountLegalEntityAccountLegalEntityId] BIGINT NOT NULL,
--   [ProviderUKPRN] BIGINT NOT NULL,
--   PRIMARY KEY ([AccountLegalEntityAccountLegalEntityId], [ProviderUKPRN]),
--   CONSTRAINT [FK_AccountLegalEntityProviders_ToAccountLegalEntities] FOREIGN KEY ([AccountLegalEntityAccountLegalEntityId]) REFERENCES [AccountLegalEntities]([Id]),
--   CONSTRAINT [FK_AccountLegalEntityProviders_ToProviders] FOREIGN KEY ([ProviderUKPRN]) REFERENCES [Providers]([UKPRN])
-- )