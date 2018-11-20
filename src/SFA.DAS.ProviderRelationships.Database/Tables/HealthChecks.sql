CREATE TABLE [dbo].[HealthChecks]
(
    [Id] INT NOT NULL IDENTITY,
    [UserRef] UNIQUEIDENTIFIER NOT NULL,
    [SentApprenticeshipInfoServiceApiRequest] DATETIME2 NOT NULL,
    [ReceivedApprenticeshipInfoServiceApiResponse] DATETIME2 NULL,
    [SentProviderRelationshipsApiRequest] DATETIME2 NOT NULL,
    [ReceivedProviderRelationshipsApiRequest] DATETIME2 NULL,
    [PublishedProviderRelationshipsEvent] DATETIME2 NOT NULL,
    [ReceivedProviderRelationshipsEvent] DATETIME2 NULL,
    CONSTRAINT [PK_HealthChecks] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_HealthChecks_Users_UserRef] FOREIGN KEY ([UserRef]) REFERENCES [Users] ([Ref]),
    INDEX [IX_HealthChecks_UserRef] NONCLUSTERED ([UserRef])
)