using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<Image, ImageViewModel>();

            CreateMap<ImageViewModel, Image>();
        }
    }
}
