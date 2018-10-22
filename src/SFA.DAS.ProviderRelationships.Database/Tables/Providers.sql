CREATE TABLE [dbo].[Providers]
(
	[Id] INT NOT NULL IDENTITY,
	[Ukprn] BIGINT NOT NULL,
	[Name] nvarchar(100) NOT NULL,  -- 100, max? what's source?
	[Created] DateTime2 NOT NULL,
	[Updated] DateTime2 NULL,
    CONSTRAINT [PK_Providers] PRIMARY KEY NONCLUSTERED ([Ukprn] ASC),
    CONSTRAINT [AK_Providers_Id] UNIQUE CLUSTERED ([Id] ASC) 
)
