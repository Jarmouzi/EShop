using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IFilterRepository: IRepository<Filter, FilterViewModel>
    {
        Task<PaginatedViewModel<FilterViewModel>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
