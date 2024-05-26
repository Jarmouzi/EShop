using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IProduct_ImageRepository: IRepository<Product_Image, Product_ImageViewModel>
    {
        Task<Result<PaginatedViewModel<Product_ImageViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
