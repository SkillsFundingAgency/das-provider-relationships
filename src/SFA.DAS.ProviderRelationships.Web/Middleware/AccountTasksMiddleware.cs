namespace SFA.DAS.ProviderRelationships.Web.Middleware;

public class AccountTasksMiddleware
{
    private const string AccountTasksKey = nameof(AccountTasksKey);
    private readonly RequestDelegate _next;

    public AccountTasksMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if user is authenticated
        if (context.User.Identity.IsAuthenticated)
        {
            if (context.Session.TryGetValue(AccountTasksKey, out var val))
            {
                context.Items[AccountTasksKey] = true;
            }
            else
            {
                // Check if referrer URL contains accountTasks=true
                if (context.Request.Query.ContainsKey("AccountTasks"))
                {
                    context.Session.SetString(AccountTasksKey, "true");
                    context.Items.TryAdd(AccountTasksKey, true);
                }
            }
        }

        // Call the next delegate/middleware in the pipeline
        await _next(context);
    }
}