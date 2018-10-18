CREATE TABLE [dbo].[Permissions]
(
	[Id] BIGINT NOT NULL IDENTITY, 
	[Type] SMALLINT NOT NULL, 
	[AccountLegalEntityId] BIGINT NOT NULL, 
	[Ukprn] BIGINT NOT NULL,
	CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED ([Id] ASC),
	--todo: need to make sure cascade delete is right way around!
	CONSTRAINT [FK_Permissions_AccountLegalEntities_AccountLegalEntityId] FOREIGN KEY ([AccountLegalEntityId]) REFERENCES [AccountLegalEntities]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Permissions_ToProviders] FOREIGN KEY ([Ukprn]) REFERENCES [Providers]([Ukprn])
)
