using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IImageRepository: IRepository<Image, ImageViewModel>
    {
        Task<Result<PaginatedViewModel<ImageViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
