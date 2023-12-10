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
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<Region, RegionViewModel>();

            CreateMap<RegionViewModel, Region>();
        }
    }
}
