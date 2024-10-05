using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IStateRepository: IRepository<State, StateViewModel>
    {
        Task<Result<PaginatedViewModel<StateViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
