using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class Product_Variant_OptionProfile : Profile
    {
        public Product_Variant_OptionProfile()
        {
            CreateMap<Product_Variant_Option, Product_Variant_OptionViewModel>();
				//.ForMember(dest => dest.ProductVariantTitle, opt => opt.MapFrom(src => src.ProductVariant.Title))	
				//.ForMember(dest => dest.Product_OptionTitle, opt => opt.MapFrom(src => src.Product_Option.Title))	;

            CreateMap<Product_Variant_OptionViewModel, Product_Variant_Option>();
        }
    }
}
