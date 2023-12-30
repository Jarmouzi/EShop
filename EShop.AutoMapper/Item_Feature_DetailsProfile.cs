using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class Item_Feature_DetailsProfile : Profile
    {
        public Item_Feature_DetailsProfile()
        {
            CreateMap<Item_Feature_Details, Item_Feature_DetailsViewModel>()
				.ForMember(dest => dest.Page_Item_FeatureTitle, opt => opt.MapFrom(src => src.Page_Item_Feature.Title))	;

            CreateMap<Item_Feature_DetailsViewModel, Item_Feature_Details>();
        }
    }
}
