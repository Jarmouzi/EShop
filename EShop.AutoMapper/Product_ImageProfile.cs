using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class Product_ImageProfile : Profile
    {
        public Product_ImageProfile()
        {
            CreateMap<Product_Image, Product_ImageViewModel>()
				.ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.Product.Title));

            CreateMap<Product_ImageViewModel, Product_Image>();
        }
    }
}
