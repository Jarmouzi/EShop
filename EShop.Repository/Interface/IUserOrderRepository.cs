using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IUserOrderRepository: IRepository<UserOrder, UserOrderViewModel>
    {
        Task<Result<PaginatedViewModel<UserOrderViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
