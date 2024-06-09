using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IFeatureRepository: IRepository<Feature, FeatureViewModel>
    {
        Task<Result<PaginatedViewModel<FeatureViewModel>>> GetPaginatedResult(Int64? categoryId = null, Int64? parentId = null, string? title = null, int take = 10, int skip = 0);
        Task<Result<bool>> ChangeDisplayOrder(Int64 id, int order);
    }
}
