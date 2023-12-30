using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

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
