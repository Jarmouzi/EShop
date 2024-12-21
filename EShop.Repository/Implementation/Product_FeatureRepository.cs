using AutoMapper;
using EShop.DataContext;
using EShop.Model;
using EShop.ViewModel;
using EShop.Model.TypeSafe;
using EShop.Repository.Interface;
using Microsoft.Data.SqlClient;

namespace EShop.Repository.Implementation
{
    public class Product_FeatureRepository : Repository<Product_Feature, Product_FeatureViewModel, EShopContext>, IProduct_FeatureRepository
    {
        public Product_FeatureRepository(IUnitOfWork<EShopContext> unitOfWork, IMapper mappingEngine) : base(unitOfWork, mappingEngine)
        {
        }

        public async Task<IEnumerable<Product_FeatureViewModel>> GetProductFeatures(Int64 productId)
        {
            try
            {
                var sparam = new SqlParameter[] {
                    new SqlParameter("@ProductId", productId)
                };

                var r = await GetProcedureAsync<Product_FeatureViewModel>("Product_Feature_Get", sparam);

                return r;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
