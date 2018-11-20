CREATE TABLE [dbo].[Permissions]
(
    [Id] BIGINT NOT NULL IDENTITY,
    [AccountProviderId] BIGINT NOT NULL,
    [AccountLegalEntityId] BIGINT NOT NULL,
    [Operation] SMALLINT NOT NULL,
    CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Permissions_AccountProviders_AccountProviderId] FOREIGN KEY ([AccountProviderId]) REFERENCES [AccountProviders] ([Id]),
    CONSTRAINT [FK_Permissions_AccountLegalEntities_AccountLegalEntityId] FOREIGN KEY ([AccountLegalEntityId]) REFERENCES [AccountLegalEntities] ([Id]),
    CONSTRAINT [UK_Permissions_AccountProviderId_AccountLegalEntityId_Operation] UNIQUE ([AccountProviderId] ASC, [AccountLegalEntityId] ASC, [Operation] ASC),
    INDEX [IX_Permissions_AccountProviderId] NONCLUSTERED ([AccountProviderId] ASC),
    INDEX [IX_Permissions_AccountLegalEntityId] NONCLUSTERED ([AccountLegalEntityId] ASC),
    INDEX [IX_Permissions_AccountProviderId_AccountLegalEntityId] NONCLUSTERED ([AccountProviderId] ASC, [AccountLegalEntityId] ASC)
)