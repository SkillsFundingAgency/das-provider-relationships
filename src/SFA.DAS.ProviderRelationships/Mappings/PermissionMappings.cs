using AutoMapper;
using SFA.DAS.ProviderRelationships.Dtos;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Mappings
{
    public class PermissionMappings : Profile
    {
        public PermissionMappings()
        {
            CreateMap<Permission, PermissionDto>();
        }
    }
}