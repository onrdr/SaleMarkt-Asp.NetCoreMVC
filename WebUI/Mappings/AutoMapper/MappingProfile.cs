﻿using AutoMapper;
using Models.Entities.Concrete;
using Models.Identity;
using Models.ViewModels;

namespace WebUI.Mappings.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CategoryViewModel, Category>().ReverseMap();
        CreateMap<ProductViewModel, Product>().ReverseMap();
        CreateMap<CompanyViewModel, Company>().ReverseMap();

        CreateMap<RegisterViewModel, AppUser>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.FullName.Trim().Replace(" ", "-")));

        CreateMap<AppUser, UserViewModel>().ReverseMap();
    }
}