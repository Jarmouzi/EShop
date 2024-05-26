using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class GroupTypeProfile : Profile
    {
        public GroupTypeProfile()
        {
            CreateMap<GroupType, GroupTypeViewModel>();

            CreateMap<GroupTypeViewModel, GroupType>();
        }
    }
}
