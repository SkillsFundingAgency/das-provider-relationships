-- CREATE TABLE [dbo].[AccountLegalEntities]
-- (
-- 	[Id] BIGINT NOT NULL IDENTITY,
-- 	[AccountLegalEntityId] BIGINT PRIMARY KEY NOT NULL,
-- 	[AccountLegalEntityPublicHashedId] CHAR(6) NOT NULL, 
-- 	[AccountId] BIGINT NOT NULL, 
-- 	[Name] NVARCHAR(100) NOT NULL, 
-- 	CONSTRAINT [FK_AccountLegalEntities_ToAccounts] FOREIGN KEY ([AccountId]) REFERENCES [Accounts]([AccountId]),
--  --if a new delete account event appears, we'll probably want... (should be set it anyway??)
--  --ON DELETE CASCADE
-- 	--CONSTRAINT AK_AccountLegalEntityId UNIQUE NONCLUSTERED(AccountLegalEntityId)
-- 	CONSTRAINT AK_AccountLegalEntities_Id UNIQUE CLUSTERED(Id)
-- )

-- EF 6 does not support FK on unique constraint and never will - EF Core 1.0 has it though :-(
-- https://data.uservoice.com/forums/72025-entity-framework-feature-suggestions/suggestions/1050579-unique-constraint-i-e-candidate-key-support

CREATE TABLE [dbo].[AccountLegalEntities]
(
	[Id] BIGINT PRIMARY KEY NOT NULL,
	[PublicHashedId] CHAR(6) NOT NULL, 
	[AccountId] BIGINT NOT NULL, 
	[Name] NVARCHAR(100) NOT NULL, 
  [Created] DateTime2 NOT NULL,
	[Updated] DateTime2 NOT NULL,
	CONSTRAINT [FK_AccountLegalEntities_ToAccounts] FOREIGN KEY ([AccountId]) REFERENCES [Accounts]([AccountId]),
 --if a new delete account event appears, we'll probably want... (should be set it anyway??)
 --ON DELETE CASCADE
)

