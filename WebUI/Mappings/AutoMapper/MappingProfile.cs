using AutoMapper;
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
        CreateMap<ProductViewModel, Product>();

        CreateMap<RegisterViewModel, AppUser>()
             .ForMember(dest => dest.UserName, opt =>
                 opt.MapFrom(src => $"{src.Name.Trim()}{src.LastName.Trim()}".ToLower()));

        CreateMap<RegisterViewModel, AppUser>()
            .ForMember(dest => dest.UserName, opt =>
                opt.MapFrom(src => $"{src.Name.Replace(" ", "-")}-{src.LastName.Replace(" ", "-")}".Trim().ToLower()));

    }
}