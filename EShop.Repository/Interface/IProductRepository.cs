using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IProductRepository: IRepository<Product, ProductViewModel>
    {
        Task<Result<PaginatedViewModel<ProductViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
