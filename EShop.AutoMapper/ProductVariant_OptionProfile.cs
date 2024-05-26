using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class ProductVariant_OptionProfile : Profile
    {
        public ProductVariant_OptionProfile()
        {
            CreateMap<ProductVariant_Option, ProductVariant_OptionViewModel>()
				.ForMember(dest => dest.OptionTitle, opt => opt.MapFrom(src => src.Option.Title))	
				.ForMember(dest => dest.OptionValueTitle, opt => opt.MapFrom(src => src.OptionValue.Title))	;

            CreateMap<ProductVariant_OptionViewModel, ProductVariant_Option>();
        }
    }
}
