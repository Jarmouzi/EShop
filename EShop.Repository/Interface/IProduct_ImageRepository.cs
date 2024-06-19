using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IProduct_ImageRepository: IRepository<Product_Image, Product_ImageViewModel>
    {
        Task<Result<IEnumerable<Product_ImageViewModel>>> GetPaginatedResult(Int64? productId, Int64? productOptionId);
    }
}
