using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IProduct_OptionRepository: IRepository<Product_Option, Product_OptionViewModel>
    {
        Task<Result<PaginatedViewModel<Product_OptionViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
