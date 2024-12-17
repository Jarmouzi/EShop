using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class ProductVariant_OptionProfile : Profile
    {
        public ProductVariant_OptionProfile()
        {
            CreateMap<ProductVariant_Option, ProductVariant_OptionViewModel>();
				//.ForMember(dest => dest.Product_OptionTitle, opt => opt.MapFrom(src => src.Product_Option.Title))	
				//.ForMember(dest => dest.ProductVariantTitle, opt => opt.MapFrom(src => src.ProductVariant.Title))	;

            CreateMap<ProductVariant_OptionViewModel, ProductVariant_Option>();
        }
    }
}
