CREATE TABLE [dbo].[Providers]
(
    [Id] INT NOT NULL IDENTITY,
    [Ukprn] BIGINT NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,  -- 100, max? what's source?
    [Created] DATETIME2 NOT NULL,
    [Updated] DATETIME2 NULL,
    CONSTRAINT [PK_Providers] PRIMARY KEY NONCLUSTERED ([Ukprn] ASC),
    CONSTRAINT [AK_Providers_Id] UNIQUE CLUSTERED ([Id] ASC) 
)
