using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IProductInPageRepository: IRepository<ProductInPage, ProductInPageViewModel>
    {
        Task<Result<PaginatedViewModel<ProductInPageViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
