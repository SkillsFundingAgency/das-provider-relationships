namespace SFA.DAS.ProviderRelationships.ReadStore.Mediator
{
    public static class ServiceFactoryExtensions
    {
        public static T GetInstance<T>(this ServiceFactory serviceFactory) => (T)serviceFactory(typeof(T));
    }
}