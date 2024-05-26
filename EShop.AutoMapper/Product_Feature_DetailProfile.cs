using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class Product_Feature_DetailProfile : Profile
    {
        public Product_Feature_DetailProfile()
        {
            CreateMap<Product_Feature_Detail, Product_Feature_DetailViewModel>()
				.ForMember(dest => dest.Product_FeatureTitle, opt => opt.MapFrom(src => src.Product_Feature.Title))	;

            CreateMap<Product_Feature_DetailViewModel, Product_Feature_Detail>();
        }
    }
}
