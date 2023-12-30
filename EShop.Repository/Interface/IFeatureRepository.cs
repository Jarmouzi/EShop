using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IFeatureRepository: IRepository<Feature, FeatureViewModel>
    {
        Task<Result<PaginatedViewModel<FeatureViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
