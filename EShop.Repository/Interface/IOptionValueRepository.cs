using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IOptionValueRepository: IRepository<OptionValue, OptionValueViewModel>
    {
        Task<Result<PaginatedViewModel<OptionValueViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
