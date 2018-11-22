using System.Web.Mvc;

namespace SFA.DAS.ProviderRelationships.Web.Extensions
{
    public static class TempDataDictionaryExtensions
    {
        public static void Add<T>(this TempDataDictionary tempData, T value) where T : class
        {
            tempData.Add(typeof(T).Name, value);
        }
        
        public static T Get<T>(this TempDataDictionary tempData) where T : class
        {
            return tempData[typeof(T).Name] as T;
        }
    }
}