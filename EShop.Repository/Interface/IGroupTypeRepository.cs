using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IGroupTypeRepository: IRepository<GroupType, GroupTypeViewModel>
    {
        Task<Result<PaginatedViewModel<GroupTypeViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
