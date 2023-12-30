using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IPage_Item_FeatureRepository: IRepository<Page_Item_Feature, Page_Item_FeatureViewModel>
    {
        Task<Result<PaginatedViewModel<Page_Item_FeatureViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
