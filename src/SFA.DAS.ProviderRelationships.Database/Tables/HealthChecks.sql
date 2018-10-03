CREATE TABLE [dbo].[HealthChecks]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[SentApprenticeshipInfoServiceApiRequest] DATETIME NOT NULL,
	[ReceivedApprenticeshipInfoServiceApiResponse] DATETIME NULL,
	[PublishedProviderRelationshipsEvent] DATETIME NOT NULL,
	[ReceivedProviderRelationshipsEvent] DATETIME NULL,
)