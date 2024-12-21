using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IProduct_GroupRepository: IRepository<Product_Group, Product_GroupViewModel>
    {
        Task<PaginatedViewModel<Product_GroupViewModel>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
