using System.Web.Mvc;

namespace SFA.DAS.ProviderRelationships.Web.Extensions
{
    public static class TempDataDictionaryExtensions
    {
        public static T Get<T>(this TempDataDictionary tempData) where T : class
        {
            return tempData[typeof(T).FullName] as T;
        }

        public static void Set<T>(this TempDataDictionary tempData, T value) where T : class
        {
            tempData[typeof(T).FullName] = value;
        }
    }
}