using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IFeatureValueRepository: IRepository<FeatureValue, FeatureValueViewModel>
    {
        Task<PaginatedViewModel<FeatureValueViewModel>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
