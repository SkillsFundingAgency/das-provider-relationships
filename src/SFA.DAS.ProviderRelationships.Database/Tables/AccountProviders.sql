CREATE TABLE [dbo].[AccountProviders]
(                                           
    [Id] INT NOT NULL IDENTITY,
    [AccountId] BIGINT NOT NULL,
    [ProviderUkprn] BIGINT NOT NULL,
    [Created] DATETIME2 NOT NULL,
    CONSTRAINT [PK_AccountProviders] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AccountProviders_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AccountProviders_Providers_ProviderUkprn] FOREIGN KEY ([ProviderUkprn]) REFERENCES [Providers]([Ukprn]) ON DELETE CASCADE,
    CONSTRAINT [UK_AccountProviders_AccountId_ProviderUkprn] UNIQUE ([AccountId] ASC, [ProviderUkprn] ASC), 
    INDEX [IX_AccountProviders_AccountId] NONCLUSTERED ([AccountId]),
    INDEX [IX_AccountProviders_ProviderUkprn] NONCLUSTERED ([ProviderUkprn])
)
