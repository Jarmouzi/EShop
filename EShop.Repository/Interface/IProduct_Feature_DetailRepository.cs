using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IProduct_Feature_DetailRepository: IRepository<Product_Feature_Detail, Product_Feature_DetailViewModel>
    {
        Task<Result<PaginatedViewModel<Product_Feature_DetailViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
