using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IPageRepository: IRepository<Page, PageViewModel>
    {
        Task<Result<PaginatedViewModel<PageViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
