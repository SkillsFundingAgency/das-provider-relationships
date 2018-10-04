CREATE TABLE [dbo].[Permissions]
(
	[PermissionId] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [Type] SMALLINT NOT NULL, 
    [AccountLegalEntityId] BIGINT NOT NULL, 
    CONSTRAINT [FK_Permissions_ToAccountLegalEntities] FOREIGN KEY ([AccountLegalEntityId]) REFERENCES [AccountLegalEntities]([AccountLegalEntityId])
)
