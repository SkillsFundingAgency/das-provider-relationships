namespace SFA.DAS.ProviderRelationships.Web.Controllers;

public static class AccountProviders
{
    public const string ControllerName = nameof(AccountProviders);

    public static class ViewNames
    {
        public const string Confirm = nameof(Confirm);
    }

    public static class ActionNames
    {
        public const string Add = nameof(Add);
        public const string Added = nameof(Added);
        public const string AlreadyAdded = nameof(AlreadyAdded);
        public const string Find = nameof(Find);
        public const string Get = nameof(Get);
        public const string Index = nameof(Index);
    }
}

public static class AccountProviderLegalEntities
{
    public const string ControllerName = nameof(AccountProviderLegalEntities);

    public static class ViewNames
    {
        public const string Confirm = nameof(Confirm);
        public const string Permissions = nameof(Permissions);
    }

    public static class ActionNames
    {
        public const string Index = nameof(Index);
        public const string Permissions = nameof(Permissions);
    }
}