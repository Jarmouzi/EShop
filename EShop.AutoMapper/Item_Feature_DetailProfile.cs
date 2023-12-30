using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class Item_Feature_DetailProfile : Profile
    {
        public Item_Feature_DetailProfile()
        {
            CreateMap<Item_Feature_Detail, Item_Feature_DetailViewModel>()
				.ForMember(dest => dest.Product_FeatureTitle, opt => opt.MapFrom(src => src.Product_Feature.Title))	;

            CreateMap<Item_Feature_DetailViewModel, Item_Feature_Detail>();
        }
    }
}
