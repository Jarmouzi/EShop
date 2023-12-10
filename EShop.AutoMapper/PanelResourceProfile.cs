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
    public class PanelResourceProfile : Profile
    {
        public PanelResourceProfile()
        {
            CreateMap<PanelResource, PanelResourceViewModel>();

            CreateMap<PanelResourceViewModel, PanelResource>();
        }
    }
}
