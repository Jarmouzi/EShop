using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class PageProfile : Profile
    {
        public PageProfile()
        {
            CreateMap<Page, PageViewModel>();

            CreateMap<PageViewModel, Page>();
        }
    }
}
