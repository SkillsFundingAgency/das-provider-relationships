CREATE TABLE [dbo].[AccountProviderLegalEntities]
(
    [Id] BIGINT NOT NULL IDENTITY,
    [AccountProviderId] BIGINT NOT NULL,
    [AccountLegalEntityId] BIGINT NOT NULL,
    [Created] DATETIME2 NOT NULL,
    [Updated] DATETIME2 NULL,
    CONSTRAINT [PK_AccountProviderLegalEntities] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AccountProviderLegalEntities_AccountProviders_AccountProviderId] FOREIGN KEY ([AccountProviderId]) REFERENCES [AccountProviders] ([Id]),
    CONSTRAINT [FK_AccountProviderLegalEntities_AccountLegalEntities_AccountLegalEntityId] FOREIGN KEY ([AccountLegalEntityId]) REFERENCES [AccountLegalEntities] ([Id]),
    CONSTRAINT [UK_AccountProviderLegalEntities_AccountProviderId_AccountLegalEntityId] UNIQUE ([AccountProviderId] ASC, [AccountLegalEntityId] ASC),
    INDEX [IX_AccountProviderLegalEntities_AccountProviderId] NONCLUSTERED ([AccountProviderId] ASC),
    INDEX [IX_AccountProviderLegalEntities_AccountLegalEntityId] NONCLUSTERED ([AccountLegalEntityId] ASC)
)