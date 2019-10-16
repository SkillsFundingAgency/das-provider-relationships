CREATE TABLE [dbo].[Unsubscribed]
(
	[EmailAddress] VARCHAR(255) NOT NULL,
    [Ukprn] VARCHAR(255) NOT NULL, 
    CONSTRAINT [PK_Unsubscribe] PRIMARY KEY ([EmailAddress], [Ukprn])
)
