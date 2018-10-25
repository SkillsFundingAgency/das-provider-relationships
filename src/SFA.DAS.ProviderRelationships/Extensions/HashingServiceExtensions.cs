using System;
using SFA.DAS.HashingService;

namespace SFA.DAS.ProviderRelationships.Extensions
{
    public static class HashingServiceExtensions
    {
        public static bool TryDecodeValue(this IHashingService hashingService, string input, out long output)
        {
            try
            {
                output = hashingService.DecodeValue(input);
                
                return true;
            }
            catch (Exception)
            {
                output = default;
                
                return false;
            }
        }
    }
}