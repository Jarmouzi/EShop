using AutoMapper;
using EShop.Model;
using EShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.AutoMapper
{
    public class SelectItemProfile : Profile
    {
        public SelectItemProfile()
        {
            CreateMap<State, SelectItemViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));

            CreateMap<City, SelectItemViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
        }
    }
}
