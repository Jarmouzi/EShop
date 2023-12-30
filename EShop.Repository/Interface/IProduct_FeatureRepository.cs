using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IProduct_FeatureRepository: IRepository<Product_Feature, Product_FeatureViewModel>
    {
        Task<Result<PaginatedViewModel<Product_FeatureViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
