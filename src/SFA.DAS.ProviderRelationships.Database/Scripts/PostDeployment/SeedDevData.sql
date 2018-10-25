IF (NOT EXISTS (SELECT 1 FROM [dbo].[Accounts] WHERE Id = 1))
BEGIN
    INSERT INTO [dbo].[Accounts] ([Id], [Name], [Created]) VALUES (1, 'Foobar Ltd', GETUTCDATE())
END