using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class Product_FeatureProfile : Profile
    {
        public Product_FeatureProfile()
        {
            CreateMap<Product_Feature, Product_FeatureViewModel>()
				.ForMember(dest => dest.FeatureTitle, opt => opt.MapFrom(src => src.Feature.Title))	
				.ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.Product.Title))	;

            CreateMap<Product_FeatureViewModel, Product_Feature>();
        }
    }
}
