using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IComponentRepository: IRepository<Component, ComponentViewModel>
    {
        Task<Result<PaginatedViewModel<ComponentViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
