IF (NOT EXISTS (SELECT 1 FROM [dbo].[Accounts] WHERE Id = 8080))
BEGIN
    INSERT INTO [dbo].[Accounts] ([Id], [Name], [Created]) VALUES (8080, 'SAINSBURY''S LIMITED', GETUTCDATE())
END

IF (NOT EXISTS (SELECT 1 FROM [dbo].[AccountLegalEntities] WHERE Id = 1))
BEGIN
    INSERT INTO [dbo].[AccountLegalEntities] ([Id], [PublicHashedId], [AccountId], [Name], [Created]) VALUES (1, 'XXXXXX', 8080, 'XXXXXX', GETUTCDATE())
END

IF (NOT EXISTS (SELECT 1 FROM [dbo].[AccountLegalEntities] WHERE Id = 2))
BEGIN
    INSERT INTO [dbo].[AccountLegalEntities] ([Id], [PublicHashedId], [AccountId], [Name], [Created]) VALUES (2, 'XXXXXX', 8080, 'XXXXXX', GETUTCDATE())
END