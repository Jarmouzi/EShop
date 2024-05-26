using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class Product_GroupProfile : Profile
    {
        public Product_GroupProfile()
        {
            CreateMap<Product_Group, Product_GroupViewModel>();

            CreateMap<Product_GroupViewModel, Product_Group>();
        }
    }
}
