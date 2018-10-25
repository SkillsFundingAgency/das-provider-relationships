CREATE TABLE [dbo].[Providers]
(
    [Ukprn] BIGINT NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,  -- 100, max? what's source?
    [Created] DATETIME2 NOT NULL,
    [Updated] DATETIME2 NULL,
    CONSTRAINT [PK_Providers] PRIMARY KEY CLUSTERED ([Ukprn] ASC)
)
