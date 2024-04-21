using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class ProductInPageProfile : Profile
    {
        public ProductInPageProfile()
        {
            CreateMap<ProductInPage, ProductInPageViewModel>()
				.ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.Product.Title))	
				//.ForMember(dest => dest.Product_FeatureTitle1, opt => opt.MapFrom(src => src.Product_Feature1.Title))	
				//.ForMember(dest => dest.Product_FeatureTitle2, opt => opt.MapFrom(src => src.Product_Feature2.Title))	
				//.ForMember(dest => dest.Product_FeatureTitle3, opt => opt.MapFrom(src => src.Product_Feature3.Title))	
				.ForMember(dest => dest.SupplierTitle, opt => opt.MapFrom(src => src.Supplier.Title))	;

            CreateMap<ProductInPageViewModel, ProductInPage>();
        }
    }
}
