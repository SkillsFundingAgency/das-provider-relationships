IF (NOT EXISTS (SELECT 1 FROM [dbo].[Accounts] WHERE Id = 1))
BEGIN
    INSERT INTO [dbo].[Accounts] ([Id], [HashedId], [PublicHashedId], [Name], [Created]) VALUES (1, 'JRML7V', 'LDMVWV', 'Foobar', GETUTCDATE())
END

IF (NOT EXISTS (SELECT 1 FROM [dbo].[AccountLegalEntities] WHERE Id = 1))
BEGIN
    INSERT INTO [dbo].[AccountLegalEntities] ([Id], [PublicHashedId], [AccountId], [Name], [Created]) VALUES (1, '7B9RP9', 1, 'Foo Ltd', GETUTCDATE())
END

IF (NOT EXISTS (SELECT 1 FROM [dbo].[AccountLegalEntities] WHERE Id = 2))
BEGIN
    INSERT INTO [dbo].[AccountLegalEntities] ([Id], [PublicHashedId], [AccountId], [Name], [Created]) VALUES (2, 'VD9WPJ', 1, 'Bar Ltd', GETUTCDATE())
END