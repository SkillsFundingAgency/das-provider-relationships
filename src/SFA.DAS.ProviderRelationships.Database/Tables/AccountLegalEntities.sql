CREATE TABLE [dbo].[AccountLegalEntities]
(
	[Id] BIGINT NOT NULL,
	[PublicHashedId] CHAR(6) NOT NULL, 
	[AccountId] BIGINT NOT NULL, 
	[Name] NVARCHAR(100) NOT NULL, 
    [Created] DateTime2 NOT NULL,
	[Updated] DateTime2 NULL,
	CONSTRAINT [PK_AccountLegalEntities] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_AccountLegalEntities_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts]([Id]),
 --if a new delete account event appears, we'll probably want... (should be set it anyway??)
 --ON DELETE CASCADE
    INDEX [IX_AccountLegalEntities_AccountId] NONCLUSTERED ([AccountId])
)

