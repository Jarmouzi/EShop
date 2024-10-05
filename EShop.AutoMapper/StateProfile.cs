using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class StateProfile : Profile
    {
        public StateProfile()
        {
            CreateMap<State, StateViewModel>();

            CreateMap<StateViewModel, State>();
        }
    }
}
