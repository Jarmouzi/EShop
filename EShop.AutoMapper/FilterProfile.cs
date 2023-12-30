using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class FilterProfile : Profile
    {
        public FilterProfile()
        {
            CreateMap<Filter, FilterViewModel>()
				.ForMember(dest => dest.CategoryTitle, opt => opt.MapFrom(src => src.Category.Title))	
				.ForMember(dest => dest.FeatureTitle, opt => opt.MapFrom(src => src.Feature.Title))	;

            CreateMap<FilterViewModel, Filter>();
        }
    }
}
