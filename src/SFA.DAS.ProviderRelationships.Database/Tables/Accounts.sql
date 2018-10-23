CREATE TABLE [dbo].[Accounts]               -- Employer_Account Data Type
(                                           
    [Id] BIGINT NOT NULL,                   -- bigint
    [Name] NVARCHAR(100) NOT NULL,          -- nvarchar(100)
    [Created] DATETIME2 NOT NULL,
    [Updated] DATETIME2 NULL,
    CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED ([Id] ASC),
)
