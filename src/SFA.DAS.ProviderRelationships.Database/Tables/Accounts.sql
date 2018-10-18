CREATE TABLE [dbo].[Accounts]               -- Employer_Account Data Type
(                                           
	[Id] BIGINT NOT NULL,                   -- bigint
	[Name] NVARCHAR(100) NOT NULL,          -- nvarchar(100)
	[Created] DateTime2 NOT NULL,
	[Updated] DateTime2 NULL,
    CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED ([Id] ASC),
)
