using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface ICollectionRepository: IRepository<Collection, CollectionViewModel>
    {
        Task<Result<PaginatedViewModel<CollectionViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
