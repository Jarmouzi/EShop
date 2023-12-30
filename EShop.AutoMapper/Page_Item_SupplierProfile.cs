using AutoMapper;
using EShop.Model;
using EShop.ViewModel;

namespace EShop.AutoMapper
{
    public class Page_Item_SupplierProfile : Profile
    {
        public Page_Item_SupplierProfile()
        {
            CreateMap<Page_Item_Supplier, Page_Item_SupplierViewModel>();

            CreateMap<Page_Item_SupplierViewModel, Page_Item_Supplier>();
        }
    }
}
