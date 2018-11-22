namespace SFA.DAS.ProviderRelationships.ReadStore.Mediator
{
    internal static class ReadStoreServiceFactoryExtensions
    {
        public static T GetInstance<T>(this ReadStoreServiceFactory serviceFactory) => (T)serviceFactory(typeof(T));
    }
}