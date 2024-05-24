using AutoMapper;
using EShop.Model;
using EShop.ViewModel;
using System.Reflection.Emit;

namespace EShop.AutoMapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryViewModel>()
                     .ForMember(dest => dest.ParentTitle, opt => opt.MapFrom(src => src.Parent.Title))
                     .ForMember(dest => dest.ParentOrder, opt => opt.MapFrom(src => Math.Pow(100, ((src.Parent.Level * -1) + 3)) * src.Parent.DisplayOrder))
                     .ForMember(dest => dest.DisplayOrder, opt => opt.MapFrom(src => Math.Pow(100, ((src.Level * -1) + 3)) * src.DisplayOrder));

            CreateMap<CategoryViewModel, Category>();
        }
    }
}
