CREATE TABLE [dbo].[CreatedAccountEventAudits]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [AccountId] BIGINT NOT NULL, 
    [PublicHashedId] CHAR(6) NOT NULL, 
    [Name] NVARCHAR(100) NOT NULL, 
    [UserName] NVARCHAR(100) NOT NULL, 
    [UserRef] UNIQUEIDENTIFIER NOT NULL, 
    [TimeLogged] DATETIME2 NOT NULL, 
    [HashedId] CHAR(6) NOT NULL
)