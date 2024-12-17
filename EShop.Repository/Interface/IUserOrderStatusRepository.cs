using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IUserOrderStatusRepository: IRepository<UserOrderStatus, UserOrderStatusViewModel>
    {
        Task<Result<PaginatedViewModel<UserOrderStatusViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
