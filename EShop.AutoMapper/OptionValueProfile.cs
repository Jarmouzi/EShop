using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class OptionValueProfile : Profile
    {
        public OptionValueProfile()
        {
            CreateMap<OptionValue, OptionValueViewModel>();

            CreateMap<OptionValueViewModel, OptionValue>();
        }
    }
}
