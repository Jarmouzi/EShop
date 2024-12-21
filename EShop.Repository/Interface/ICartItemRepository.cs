using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface ICartItemRepository: IRepository<CartItem, CartItemViewModel>
    {
        Task<PaginatedViewModel<CartItemViewModel>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
