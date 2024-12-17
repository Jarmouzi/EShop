using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class PaymentGatewayProfile : Profile
    {
        public PaymentGatewayProfile()
        {
            CreateMap<PaymentGateway, PaymentGatewayViewModel>();

            CreateMap<PaymentGatewayViewModel, PaymentGateway>();
        }
    }
}
