﻿CREATE TABLE [dbo].[Users]
(
	[Ref] UNIQUEIDENTIFIER NOT NULL,
	[Email] VARCHAR(255) NOT NULL,
    [FirstName] NVARCHAR(50) NOT NULL,
    [LastName] NVARCHAR(50) NOT NULL,
    [Created] DATETIME2 NOT NULL,
    [Updated] DATETIME2 NULL,
    --todo: clustered??
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Ref] ASC)
)