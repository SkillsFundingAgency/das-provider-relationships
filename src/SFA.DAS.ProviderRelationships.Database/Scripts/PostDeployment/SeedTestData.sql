IF (NOT EXISTS (SELECT 1 FROM [dbo].[Accounts] WHERE Id = 8080))
BEGIN
    INSERT INTO [dbo].[Accounts] ([Id], [PublicHashedId], [Name], [Created]) VALUES (8080, '6K7XWL', 'SAINSBURY''S LIMITED', GETUTCDATE())
END

IF (NOT EXISTS (SELECT 1 FROM [dbo].[AccountLegalEntities] WHERE Id = 9))
BEGIN
    INSERT INTO [dbo].[AccountLegalEntities] ([Id], [PublicHashedId], [AccountId], [Name], [Created]) VALUES (9, 'PZ26D2', 8080, 'SAINSBURY''S LIMITED', GETUTCDATE())
END