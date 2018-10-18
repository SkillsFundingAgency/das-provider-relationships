CREATE TABLE [dbo].[Providers]
(
-- 	[Id] BIGINT NOT NULL IDENTITY,
	[UKPRN] BIGINT NOT NULL PRIMARY KEY,
	[Name] nvarchar(100) NOT NULL,  -- 100, max? what's source?
	[Created] DateTime2 NOT NULL,
	[Updated] DateTime2 NULL,
	--CONSTRAINT AK_UKPRN UNIQUE(UKPRN)
-- 	CONSTRAINT AK_Providers_Id UNIQUE CLUSTERED(Id)
)
