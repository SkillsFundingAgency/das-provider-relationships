CREATE TABLE [dbo].[AccountLegalEntities]
(
	[AccountLegalEntityId] BIGINT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(100) NOT NULL, 
    [PublicHashedId] CHAR(6) NOT NULL, 
    [AccountId] BIGINT NOT NULL, 
    CONSTRAINT [FK_AccountLegalEntities_ToAccounts] FOREIGN KEY ([AccountId]) REFERENCES [Accounts]([AccountId])
)
