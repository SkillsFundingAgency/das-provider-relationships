/*
Script to populate Account/Legal Entity data for provider relationships from MA source data

Instructions for use:
1. Think about obtaining a prod db backup for sourcing the data, since it kills the db
2. Turn on SQL CMD mode
3. Execute this script against MA employer_account database
4. Execute the resulting script against provider-relationships database

Expectations:
~30 minutes on employer_account database
~20 seconds on provider-relationships db
~15k+ Accounts created
~27k+ AccountLegalEntities created
*/

--:OUT STDOUT 

SET NOCOUNT ON

declare @MAXINSERT int = 1000 --insert values() batch size (cannot be more than 1000)

--Some table var declarations
print 'declare @CreateDateTime DATETIME = GETDATE()'
print 'declare @Accounts table ([AccountId] bigint,[Name] nvarchar(100))'
print 'declare @AccountLegalEntities table ([AccountLegalEntityId] bigint,[AccountLegalEntityPublicHashedId] nvarchar(6),[AccountId] bigint,[Name] nvarchar(100))'

BEGIN TRY

	IF(EXISTS(SELECT * FROM [employer_account].[AccountLegalEntity] WHERE PublicHashedId is null))
	BEGIN
		THROW 50001, 'Blank Public Hashed Ids', 1
	END

	--Accounts
	select
	case (ROW_NUMBER() OVER (ORDER BY a.Id) % @MAXINSERT) when 1 then 'insert into @Accounts ([AccountId],[Name]) values' + char(13) + char(10) else '' end +
	' (' + convert(varchar,[Id]) + ', ' + '''' + replace([Name],'''','''''') + '''' + ')' + 
	case when ((ROW_NUMBER() OVER (ORDER BY a.Id) % @MAXINSERT = 0) OR (ROW_NUMBER() OVER (ORDER BY a.Id) = (select count(1) from [employer_account].[Account]))) then '' else ',' end
	from
	[employer_account].[Account] a
	order by a.Id asc
	

	--AccountLegalEntities
	select
	case (ROW_NUMBER() OVER (ORDER BY ale.Id) % @MAXINSERT) when 1 then 'insert into @AccountLegalEntities ([AccountLegalEntityId],[AccountLegalEntityPublicHashedId],[AccountId],[Name]) values' + char(13) + char(10) else '' end +
	' (' + convert(varchar,ale.[Id]) + ', '
		+ '''' + ale.[PublicHashedId] + '''' + ', '
		+ convert(varchar,ale.[AccountId]) + ', '
		+ '''' + replace(ale.[Name],'''','''''') + ''''
	+ ')'  + 
	case when
		((ROW_NUMBER() OVER (ORDER BY ale.Id) % @MAXINSERT = 0)
		OR (ROW_NUMBER() OVER (ORDER BY ale.Id) = (select count(1) from [employer_account].[AccountLegalEntity] where PublicHashedId is not null)))
	then '' else ',' end
	from [employer_account].[AccountLegalEntity] ale
	join [employer_account].[LegalEntity] le on le.Id = ale.LegalEntityId
	where ale.PublicHashedId is not null
	order by ale.Id asc

	--Final inserts
	print '
	BEGIN TRANSACTION

	insert into Accounts ([Id], [Name], [Created])
	select a.[AccountId], a.[Name], @CreateDateTime from @Accounts a
	left join Accounts e on e.[Id] = a.[AccountId]
	where e.[Id] is null --skip existing
	print ''Inserted '' + convert(varchar,@@ROWCOUNT) + '' Accounts''
	'

	print '
	insert into AccountLegalEntities([Id],[PublicHashedId],[AccountId],[Name], [Created])
	select ale.[AccountLegalEntityId], ale.[AccountLegalEntityPublicHashedId], ale.[AccountId], ale.[Name], @CreateDateTime 
	from @AccountLegalEntities ale
	left join AccountLegalEntities e on e.[Id] = ale.[AccountLegalEntityId]
	where e.[Id] is null --skip existing
	print ''Inserted '' + convert(varchar,@@ROWCOUNT) + '' AccountLegalEntities''
	print ''Completed''


	ROLLBACK TRANSACTION
	'

END TRY
BEGIN CATCH
	PRINT 'Problem, there are blank public hashed Id'
END CATCH




