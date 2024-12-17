using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class UserOrderStatusProfile : Profile
    {
        public UserOrderStatusProfile()
        {
            CreateMap<UserOrderStatus, UserOrderStatusViewModel>();

            CreateMap<UserOrderStatusViewModel, UserOrderStatus>();
        }
    }
}
