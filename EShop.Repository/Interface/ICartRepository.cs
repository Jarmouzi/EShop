using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface ICartRepository: IRepository<Cart, CartViewModel>
    {
        Task<Result<PaginatedViewModel<CartViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
