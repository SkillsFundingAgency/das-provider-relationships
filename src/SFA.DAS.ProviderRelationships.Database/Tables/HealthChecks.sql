CREATE TABLE [dbo].[HealthChecks]
(
	[Id] INT NOT NULL IDENTITY,
	[UserRef] UNIQUEIDENTIFIER NOT NULL,
	[SentApprenticeshipInfoServiceApiRequest] DATETIME2 NOT NULL,
	[ReceivedApprenticeshipInfoServiceApiResponse] DATETIME2 NULL,
	[PublishedProviderRelationshipsEvent] DATETIME2 NOT NULL,
	[ReceivedProviderRelationshipsEvent] DATETIME2 NULL,
	CONSTRAINT [PK_HealthChecks] PRIMARY KEY CLUSTERED ([Id] ASC),
)