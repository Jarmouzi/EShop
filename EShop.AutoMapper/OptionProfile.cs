using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class OptionProfile : Profile
    {
        public OptionProfile()
        {
            CreateMap<Option, OptionViewModel>();

            CreateMap<OptionViewModel, Option>();
        }
    }
}
