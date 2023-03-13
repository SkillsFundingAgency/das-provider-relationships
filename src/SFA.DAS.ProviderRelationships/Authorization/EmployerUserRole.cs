namespace SFA.DAS.ProviderRelationships.Authorization
{
    public enum EmployerUserRoles
    {
        None = 0,
        Owner = 1,
        Transactor = 2,
        Viewer = 3
    }

    public static class EmployerUserRole
    {
        internal const string Prefix = "EmployerUserRole.";
        internal const string AnyOption = "Any";
        internal const string OwnerOption = "Owner";
        internal const string TransactorOption = "Transactor";
        internal const string ViewerOption = "Viewer";
        
        public const string Any = Prefix + AnyOption;
        public const string Owner = Prefix + OwnerOption;
        public const string OwnerOrTransactor = Owner + "," + Transactor;
        public const string Transactor = Prefix + TransactorOption;
        public const string Viewer = Prefix + ViewerOption;
    }
}