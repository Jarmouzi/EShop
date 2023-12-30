using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IPanelResourceRepository: IRepository<PanelResource, PanelResourceViewModel>
    {
        Task<Result<PaginatedViewModel<PanelResourceViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
