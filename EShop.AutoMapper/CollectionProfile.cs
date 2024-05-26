using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class CollectionProfile : Profile
    {
        public CollectionProfile()
        {
            CreateMap<Collection, CollectionViewModel>();

            CreateMap<CollectionViewModel, Collection>();
        }
    }
}
