using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class Supplier_BrandProfile : Profile
    {
        public Supplier_BrandProfile()
        {
            CreateMap<Supplier_Brand, Supplier_BrandViewModel>()
				.ForMember(dest => dest.SupplierTitle, opt => opt.MapFrom(src => src.Supplier.Title))	
				.ForMember(dest => dest.BrandTitle, opt => opt.MapFrom(src => src.Brand.Title));

            CreateMap<Supplier_BrandViewModel, Supplier_Brand>();
        }
    }
}
