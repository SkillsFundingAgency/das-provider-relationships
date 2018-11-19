CREATE TABLE [dbo].[Permissions]
(
    [Id] BIGINT NOT NULL IDENTITY,
    [AccountProviderId] BIGINT NOT NULL,
    [Operation] SMALLINT NOT NULL,
    CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Permissions_AccountProviders_AccountProviderId] FOREIGN KEY ([AccountProviderId]) REFERENCES [AccountProviders]([Id]) ON DELETE CASCADE,
    CONSTRAINT [UK_Permissions_AccountProviderId_Operation] UNIQUE ([AccountProviderId] ASC, [Operation] ASC),
    INDEX [IX_Permissions_AccountProviderId] NONCLUSTERED ([AccountProviderId] ASC)
)