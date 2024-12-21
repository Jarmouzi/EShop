using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IStockRepository: IRepository<Stock, StockViewModel>
    {
        Task<PaginatedViewModel<StockViewModel>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
