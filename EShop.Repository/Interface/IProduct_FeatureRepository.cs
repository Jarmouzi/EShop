using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IProduct_FeatureRepository: IRepository<Product_Feature, Product_FeatureViewModel>
    {
        Task<IEnumerable<Product_FeatureViewModel>> GetProductFeatures(Int64 productId);
    }
}
