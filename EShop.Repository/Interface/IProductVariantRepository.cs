using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IProductVariantRepository: IRepository<ProductVariant, ProductVariantViewModel>
    {
        Task<Result<PaginatedViewModel<ProductVariantViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
