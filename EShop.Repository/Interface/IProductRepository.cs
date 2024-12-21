using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IProductRepository: IRepository<Product, ProductViewModel>
    {
        Task<PaginatedViewModel<ProductViewModel>> GetPaginatedResult(Int64? categoryId = null, Int64? brandId = null, string? title = null, int take = 10, int skip = 0);
    }
}
