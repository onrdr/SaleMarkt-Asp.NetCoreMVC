using AutoMapper;
using Models.Entities.Concrete;
using Models.ViewModels;

namespace WebUI.Mappings.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CategoryViewModel, Category>().ReverseMap(); 
        CreateMap<ProductViewModel, Product>().ReverseMap(); 
    }
}