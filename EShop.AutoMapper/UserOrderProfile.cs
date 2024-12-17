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
				.ForMember(dest => dest.UserOrderStatusTitle, opt => opt.MapFrom(src => src.UserOrderStatus.Title))	
				.ForMember(dest => dest.CartTitle, opt => opt.MapFrom(src => src.Cart.TotalAmount))	
				.ForMember(dest => dest.UserAddressTitle, opt => opt.MapFrom(src => src.UserAddress.Address))	;

            CreateMap<UserOrderViewModel, UserOrder>();
        }
    }
}
