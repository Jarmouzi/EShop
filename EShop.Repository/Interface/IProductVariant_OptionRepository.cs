using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IProductVariant_OptionRepository: IRepository<ProductVariant_Option, ProductVariant_OptionViewModel>
    {
        Task<Result<PaginatedViewModel<ProductVariant_OptionViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
