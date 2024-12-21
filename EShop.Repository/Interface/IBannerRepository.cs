using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IBannerRepository: IRepository<Banner, BannerViewModel>
    {
        Task<PaginatedViewModel<BannerViewModel>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
