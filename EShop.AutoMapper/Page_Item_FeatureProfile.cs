using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class Page_Item_FeatureProfile : Profile
    {
        public Page_Item_FeatureProfile()
        {
            CreateMap<Page_Item_Feature, Page_Item_FeatureViewModel>();

            CreateMap<Page_Item_FeatureViewModel, Page_Item_Feature>();
        }
    }
}
