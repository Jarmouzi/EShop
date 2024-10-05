using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class UserAddressProfile : Profile
    {
        public UserAddressProfile()
        {
            CreateMap<UserAddress, UserAddressViewModel>();

            CreateMap<UserAddressViewModel, UserAddress>();
        }
    }
}
