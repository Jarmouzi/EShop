using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class FeatureValueProfile : Profile
    {
        public FeatureValueProfile()
        {
            CreateMap<FeatureValue, FeatureValueViewModel>()
				.ForMember(dest => dest.FeatureTitle, opt => opt.MapFrom(src => src.Feature.Title))	;

            CreateMap<FeatureValueViewModel, FeatureValue>();
        }
    }
}
