using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class ProductSeoProfile : Profile
    {
        public ProductSeoProfile()
        {
            CreateMap<ProductSeo, ProductSeoViewModel>();

            CreateMap<ProductSeoViewModel, ProductSeo>();
        }
    }
}
