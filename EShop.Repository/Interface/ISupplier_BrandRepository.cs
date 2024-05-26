using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface ISupplier_BrandRepository: IRepository<Supplier_Brand, Supplier_BrandViewModel>
    {
        Task<Result<PaginatedViewModel<Supplier_BrandViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
