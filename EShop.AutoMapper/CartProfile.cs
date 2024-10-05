using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, CartViewModel>();

            CreateMap<CartViewModel, Cart>();
        }
    }
}
