using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class StockProfile : Profile
    {
        public StockProfile()
        {
            CreateMap<Stock, StockViewModel>();

            CreateMap<StockViewModel, Stock>();
        }
    }
}
