namespace SFA.DAS.ProviderRelationships.Web.Authorisation
{
    public static class PolicyNames
    {
        public static string HasEmployerOwnerAccount => nameof(HasEmployerOwnerAccount);
        public static string HasEmployerViewAccount => nameof(HasEmployerViewAccount);
        public static string HasEmployerOwnerOrViewerAccount = nameof(HasEmployerOwnerOrViewerAccount);
    }
}