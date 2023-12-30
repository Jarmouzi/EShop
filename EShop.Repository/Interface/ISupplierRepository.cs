using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface ISupplierRepository: IRepository<Supplier, SupplierViewModel>
    {
        Task<Result<PaginatedViewModel<SupplierViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
