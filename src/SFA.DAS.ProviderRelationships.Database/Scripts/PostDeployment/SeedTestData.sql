IF (NOT EXISTS (SELECT 1 FROM [dbo].[Accounts] WHERE Id = 8080))
BEGIN
    INSERT INTO [dbo].[Accounts] ([Id], [Name], [Created]) VALUES (8080, 'SAINSBURY''S LIMITED', GETUTCDATE())
END