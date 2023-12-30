using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IItem_Feature_DetailsRepository: IRepository<Item_Feature_Details, Item_Feature_DetailsViewModel>
    {
        Task<Result<PaginatedViewModel<Item_Feature_DetailsViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
