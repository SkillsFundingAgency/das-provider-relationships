/*
Script to populate Account/Legal Entity data

Instructions for use:
1. Turn on SQL CMD mode
2. Execute this script against MA employer_account database
3. Execute the resulting script against provider-relationships database
*/

:OUT STDOUT 

SET NOCOUNT ON

select
 q.sql_for_provider_relationships
+ case when rownum%100=0 then char(13) + char(10) + 'GO' + char(13) + char(10) else '' end
from
(
	select
	'insert into [dbo].[Accounts] ([AccountId],[Name]) values (' + convert(varchar,[Id]) + ', ' + '''' + replace([Name],'''','''''') + '''' + ')'
	as sql_for_provider_relationships,
	ROW_NUMBER() OVER (ORDER BY a.Id) as rownum
	from
	[employer_account].[Account] a
	union all
	select
	'insert into AccountLegalEntities ([AccountLegalEntityId],[AccountLegalEntityPublicHashedId],[AccountId],[Name]) values (' 
	 + convert(varchar,ale.[Id]) + ', '
	 + '''' + ale.[PublicHashedId] + '''' + ', '
	 + convert(varchar,ale.[AccountId]) + ', '
	 + '''' + replace(ale.[Name],'''','''''') + ''''
	+ ')' as sql_for_provider_relationships,
	ROW_NUMBER() OVER (ORDER BY ale.Id) as rownum
	from [employer_account].[AccountLegalEntity] ale
	join [employer_account].[LegalEntity] le on le.Id = ale.LegalEntityId
	where ale.PublicHashedId is not null
) as q

