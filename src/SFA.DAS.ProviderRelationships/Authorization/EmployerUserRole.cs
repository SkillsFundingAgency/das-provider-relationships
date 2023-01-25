namespace SFA.DAS.ProviderRelationships.Authorization
{
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