-- Number of accounts that have granted providers permissions
SELECT COUNT(1)
FROM Accounts a
WHERE EXISTS
(
    SELECT 1
    FROM AccountProviders ap
    INNER JOIN AccountProviderLegalEntities aple ON aple.AccountProviderId = ap.Id
    INNER JOIN Permissions p ON p.AccountProviderLegalEntityId = aple.Id
    WHERE ap.AccountId = a.Id
)

-- Number of account legal entities that have granted providers permissions
SELECT COUNT(1)
FROM AccountLegalEntities ale
WHERE EXISTS
(
   SELECT 1
   FROM AccountProviderLegalEntities aple
   INNER JOIN Permissions p ON p.AccountProviderLegalEntityId = aple.Id
   WHERE aple.AccountLegalEntityId = ale.Id
)

-- Number of providers that have been granted permissions
SELECT COUNT(1)
FROM Providers p
WHERE EXISTS
(
    SELECT 1
    FROM AccountProviders ap
    INNER JOIN AccountProviderLegalEntities aple ON aple.AccountProviderId = ap.Id
    INNER JOIN Permissions pe ON pe.AccountProviderLegalEntityId = aple.Id
    WHERE ap.ProviderUkprn = p.Ukprn
)