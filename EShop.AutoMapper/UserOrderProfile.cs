using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class UserOrderProfile : Profile
    {
        public UserOrderProfile()
        {
            CreateMap<UserOrder, UserOrderViewModel>()
				.ForMember(dest => dest.UserOrderStatusTitle, opt => opt.MapFrom(src => src.UserOrderStatus.Title));

            CreateMap<UserOrderViewModel, UserOrder>();
        }
    }
}
