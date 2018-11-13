namespace SFA.DAS.ProviderRelationships.ReadStore.Mediator
{
    internal static class ApiServiceFactoryExtensions
    {
        public static T GetInstance<T>(this ApiServiceFactory serviceFactory) => (T)serviceFactory(typeof(T));
    }
}