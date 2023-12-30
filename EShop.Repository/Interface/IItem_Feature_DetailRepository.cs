using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IItem_Feature_DetailRepository: IRepository<Item_Feature_Detail, Item_Feature_DetailViewModel>
    {
        Task<Result<PaginatedViewModel<Item_Feature_DetailViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
