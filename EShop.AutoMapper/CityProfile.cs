using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityViewModel>();
				//.ForMember(dest => dest.StateTitle, opt => opt.MapFrom(src => src.State.Title))	;

            CreateMap<CityViewModel, City>();
        }
    }
}
