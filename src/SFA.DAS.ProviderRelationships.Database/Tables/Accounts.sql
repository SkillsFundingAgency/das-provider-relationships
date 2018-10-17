-- CREATE TABLE [dbo].[Accounts]
-- (												-- Employer_Account Data Type
-- 	[Id] BIGINT NOT NULL IDENTITY,
-- 	[AccountId] BIGINT NOT NULL PRIMARY KEY,				-- bitint
-- 	[Name] NVARCHAR(100) NOT NULL,				-- nvarchar(100)
-- 	[PublicHashedId] CHAR(6) NOT NULL,			-- nvarchar(100)
-- 	--CONSTRAINT AK_AccountId UNIQUE(AccountId)
-- 	CONSTRAINT AK_Accounts_Id UNIQUE CLUSTERED(Id)
-- )

CREATE TABLE [dbo].[Accounts]
(												                -- Employer_Account Data Type
	[Id] BIGINT NOT NULL PRIMARY KEY,		  -- bigint
	[Name] NVARCHAR(100) NOT NULL,				-- nvarchar(100)
	[Created] DateTime2 NOT NULL,
	[Updated] DateTime2 NULL,
)
