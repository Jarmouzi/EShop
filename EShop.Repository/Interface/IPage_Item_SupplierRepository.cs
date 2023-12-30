using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IPage_Item_SupplierRepository: IRepository<Page_Item_Supplier, Page_Item_SupplierViewModel>
    {
        Task<Result<PaginatedViewModel<Page_Item_SupplierViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
