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
                .ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.Product.Title))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image.Url))
                .ForMember(dest => dest.ImageAlt, opt => opt.MapFrom(src => src.Image.AltText))
                .ForMember(dest => dest.OptionTitle, opt => opt.MapFrom(src => src.Product_Option == null ? "" : src.Product_Option.Option.Title))
                .ForMember(dest => dest.ValueTitle, opt => opt.MapFrom(src => src.Product_Option == null ? "" : src.Product_Option.OptionValue.Title));

            CreateMap<Product_ImageViewModel, Product_Image>();
        }
    }
}
