using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductViewModel>()
				.ForMember(dest => dest.BrandTitle, opt => opt.MapFrom(src => src.Brand.Title))	
				.ForMember(dest => dest.CategoryTitle, opt => opt.MapFrom(src => src.Category.Title))	;

            CreateMap<ProductViewModel, Product>();
        }
    }
}
