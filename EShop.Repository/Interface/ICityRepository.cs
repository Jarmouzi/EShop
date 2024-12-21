using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface ICityRepository: IRepository<City, CityViewModel>
    {
        Task<PaginatedViewModel<CityViewModel>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
