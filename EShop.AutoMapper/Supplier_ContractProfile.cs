using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class Supplier_ContractProfile : Profile
    {
        public Supplier_ContractProfile()
        {
            CreateMap<Supplier_Contract, Supplier_ContractViewModel>()
				.ForMember(dest => dest.SupplierTitle, opt => opt.MapFrom(src => src.Supplier.Title))	
				.ForMember(dest => dest.CategoryTitle, opt => opt.MapFrom(src => src.Category.Title))	
				.ForMember(dest => dest.SaleTypeTitle, opt => opt.MapFrom(src => src.SaleType.Title))	;

            CreateMap<Supplier_ContractViewModel, Supplier_Contract>();
        }
    }
}
