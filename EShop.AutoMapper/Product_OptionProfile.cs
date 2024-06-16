using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class Product_OptionProfile : Profile
    {
        public Product_OptionProfile()
        {
            CreateMap<Product_Option, Product_OptionViewModel>()
				.ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.Product.Title))	
				.ForMember(dest => dest.OptionTitle, opt => opt.MapFrom(src => src.Option.Title))	
				.ForMember(dest => dest.OptionValueTitle, opt => opt.MapFrom(src => src.OptionValue.Title))	;

            CreateMap<Product_OptionViewModel, Product_Option>();
        }
    }
}
