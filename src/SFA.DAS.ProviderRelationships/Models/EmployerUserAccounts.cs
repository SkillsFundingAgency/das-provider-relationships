using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderRelationships.Services.OuterApi;

namespace SFA.DAS.ProviderRelationships.Models;

public class EmployerUserAccounts
{
    public string EmployerUserId { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public IEnumerable<EmployerUserAccountItem> EmployerAccounts { get; set; }
    public bool IsSuspended { get; set; }

    public static implicit operator EmployerUserAccounts(GetUserAccountsResponse source)
    {
        var accounts = source?.UserAccounts == null
            ? new List<EmployerUserAccountItem>()
            : source.UserAccounts.Select(c => (EmployerUserAccountItem) c).ToList();
        
        return new EmployerUserAccounts
        {
            FirstName = source?.FirstName,
            LastName = source?.LastName,
            EmployerUserId = source?.EmployerUserId,
            IsSuspended = source?.IsSuspended ?? false,
            EmployerAccounts = accounts
        };
    }
}

public class EmployerUserAccountItem
{
    public string AccountId { get; set; }
    public string EmployerName { get; set; }
    public string Role { get; set; }

    public static implicit operator EmployerUserAccountItem(EmployerIdentifier source)
    {
        return new EmployerUserAccountItem
        {
            AccountId = source.AccountId,
            EmployerName = source.EmployerName,
            Role = source.Role
        };
    }
}
