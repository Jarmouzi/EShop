using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IProduct_Variant_OptionRepository: IRepository<Product_Variant_Option, Product_Variant_OptionViewModel>
    {
        Task<Result<PaginatedViewModel<Product_Variant_OptionViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
