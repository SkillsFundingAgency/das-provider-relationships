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

:OUT STDOUT 

SET NOCOUNT ON

declare @MAXINSERT int = 1000 --insert values() batch size (cannot be more than 1000)

--Some table var declarations
print 'declare @Accounts table ([AccountId] bigint,[Name] nvarchar(100))'
print 'declare @AccountLegalEntities table ([AccountLegalEntityId] bigint,[AccountLegalEntityPublicHashedId] nvarchar(6),[AccountId] bigint,[Name] nvarchar(100))'

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
insert into Accounts ([AccountId], [Name])
select a.[AccountId], a.[Name] from @Accounts a
left join Accounts e on e.[AccountId] = a.[AccountId]
where e.[AccountId] is null --skip existing
print ''Inserted '' + convert(varchar,@@ROWCOUNT) + '' Accounts''
'

print '
insert into AccountLegalEntities([AccountLegalEntityId],[AccountLegalEntityPublicHashedId],[AccountId],[Name])
select ale.[AccountLegalEntityId], ale.[AccountLegalEntityPublicHashedId], ale.[AccountId], ale.[Name] from @AccountLegalEntities ale
left join AccountLegalEntities e on e.[AccountLegalEntityId] = ale.[AccountLegalEntityId]
where e.[AccountLegalEntityId] is null --skip existing
print ''Inserted '' + convert(varchar,@@ROWCOUNT) + '' AccountLegalEntities''
print ''Completed''
'
