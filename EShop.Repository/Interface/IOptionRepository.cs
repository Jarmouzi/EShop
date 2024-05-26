using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IOptionRepository: IRepository<Option, OptionViewModel>
    {
        Task<Result<PaginatedViewModel<OptionViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
