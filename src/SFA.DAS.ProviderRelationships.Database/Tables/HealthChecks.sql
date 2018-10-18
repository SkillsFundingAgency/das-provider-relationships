CREATE TABLE [dbo].[HealthChecks]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[UserRef] UNIQUEIDENTIFIER NOT NULL,
	[SentApprenticeshipInfoServiceApiRequest] DATETIME NOT NULL,
	[ReceivedApprenticeshipInfoServiceApiResponse] DATETIME NULL,
	[PublishedProviderRelationshipsEvent] DATETIME NOT NULL,
	[ReceivedProviderRelationshipsEvent] DATETIME NULL,
)