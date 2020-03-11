CREATE TABLE [dbo].[OutboxData]
(
	[MessageId] NVARCHAR(200) NOT NULL PRIMARY KEY NONCLUSTERED,
	[Dispatched] BIT NOT NULL DEFAULT(0),
	[DispatchedAt] DATETIME NULL,
	[Operations] NVARCHAR(MAX) NOT NULL,
)
GO

CREATE INDEX [IX_DispatchedAt] ON [dbo].[OutboxData] ([DispatchedAt] ASC) WHERE [Dispatched] = 1
GO