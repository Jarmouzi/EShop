using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IBrandRepository: IRepository<Brand, BrandViewModel>
    {
        Task<PaginatedViewModel<BrandViewModel>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
