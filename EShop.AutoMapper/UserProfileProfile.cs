using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class UserProfileProfile : Profile
    {
        public UserProfileProfile()
        {
            CreateMap<UserProfile, UserProfileViewModel>();

            CreateMap<UserProfileViewModel, UserProfile>();
        }
    }
}
