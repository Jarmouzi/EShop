using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface ISaleTypeRepository: IRepository<SaleType, SaleTypeViewModel>
    {
        Task<Result<PaginatedViewModel<SaleTypeViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
