using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IProductSeoRepository: IRepository<ProductSeo, ProductSeoViewModel>
    {
        Task<PaginatedViewModel<ProductSeoViewModel>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
