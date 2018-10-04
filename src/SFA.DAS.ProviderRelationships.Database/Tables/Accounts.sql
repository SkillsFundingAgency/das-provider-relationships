CREATE TABLE [dbo].[Accounts]
(												-- Employer_Account Data Type
	[AccountId] BIGINT NOT NULL PRIMARY KEY,	-- bitint
    [Name] NVARCHAR(100) NOT NULL,				-- nvarchar(100)
    [PublicHashedId] CHAR(6) NOT NULL				-- nvarchar(100)
)
