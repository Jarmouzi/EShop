using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class ProductVariantProfile : Profile
    {
        public ProductVariantProfile()
        {
            CreateMap<ProductVariant, ProductVariantViewModel>()
				.ForMember(dest => dest.SupplierTitle, opt => opt.MapFrom(src => src.Supplier.Title))	
				.ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.Product.Title))	;

            CreateMap<ProductVariantViewModel, ProductVariant>();
        }
    }
}
