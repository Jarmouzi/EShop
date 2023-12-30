using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class SaleTypeProfile : Profile
    {
        public SaleTypeProfile()
        {
            CreateMap<SaleType, SaleTypeViewModel>();

            CreateMap<SaleTypeViewModel, SaleType>();
        }
    }
}
