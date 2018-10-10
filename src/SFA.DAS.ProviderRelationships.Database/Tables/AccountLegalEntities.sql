CREATE TABLE [dbo].[AccountLegalEntities]
(
	[AccountLegalEntityId] BIGINT NOT NULL PRIMARY KEY, 
	[AccountLegalEntityPublicHashedId] CHAR(6) NOT NULL, 
	[AccountId] BIGINT NOT NULL, 
	[Name] NVARCHAR(100) NOT NULL, 
	CONSTRAINT [FK_AccountLegalEntities_ToAccounts] FOREIGN KEY ([AccountId]) REFERENCES [Accounts]([AccountId])
 --if a new delete account event appears, we'll probably want... (should be set it anyway??)
 --ON DELETE CASCADE
)
