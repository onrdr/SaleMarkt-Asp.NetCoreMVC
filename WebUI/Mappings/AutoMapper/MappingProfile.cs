using AutoMapper;
using Models.Entities;
using Models.ViewModels;

namespace WebUI.Mappings.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CategoryViewModel, Category>().ReverseMap(); 
    }
}