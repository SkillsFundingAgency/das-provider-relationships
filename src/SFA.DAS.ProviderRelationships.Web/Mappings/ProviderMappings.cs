﻿using AutoMapper;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Web.ViewModels.Providers;

namespace SFA.DAS.ProviderRelationships.Web.Mappings
{
    public class ProviderMappings : Profile
    {
        public ProviderMappings()
        {
            CreateMap<GetAddedProviderQueryResponse, AddedProviderViewModel>()
                .ForMember(d => d.Choice, o => o.Ignore());
            
            CreateMap<GetAddedProviderQueryResponse, AlreadyAddedProviderViewModel>()
                .ForMember(d => d.Choice, o => o.Ignore());
            
            CreateMap<GetProviderQueryResponse, AddProviderViewModel>()
                .ForMember(d => d.Ukprn, o => o.MapFrom(s => s.Provider.Ukprn))
                .ForMember(d => d.AccountId, o => o.Ignore())
                .ForMember(d => d.UserRef, o => o.Ignore())
                .ForMember(d => d.Choice, o => o.Ignore());

            //required?
            //CreateMap<GetAddedProvidersQueryResponse, TrainingProviderPermissionsViewModel>();
        }
    }
}