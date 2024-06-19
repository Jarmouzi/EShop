using AutoMapper;
using EShop.DataContext;
using EShop.Model;
using EShop.ViewModel;
using EShop.Model.TypeSafe;
using EShop.Repository.Interface;
using Microsoft.Data.SqlClient;

namespace EShop.Repository.Implementation
{
    public class Product_ImageRepository : Repository<Product_Image, Product_ImageViewModel, EShopContext>, IProduct_ImageRepository
    {
        public Product_ImageRepository(IUnitOfWork<EShopContext> unitOfWork, IMapper mappingEngine) : base(unitOfWork, mappingEngine)
        {
        }

        public async Task<Result<IEnumerable<Product_ImageViewModel>>> GetPaginatedResult(Int64? productId, Int64? productOptionId)
        {
            var result = new Result<IEnumerable<Product_ImageViewModel>> ();

            try
            {
                var sparam = new SqlParameter[] {
                    new SqlParameter("@ProductId", productId == null? DBNull.Value :productId ),
                    new SqlParameter("@ProductOptionId", productOptionId == null? DBNull.Value :productOptionId)
                };

                var r = await GetProcedureAsync<Product_ImageViewModel>("Product_Image_Get", sparam);

                if(r.Status == TS.Status.Success) {
                    result.Data = r.Data;
                    result.Status = TS.Status.Success;
                    return result;
                }
                result.Status = TS.Status.Warning;
                result.Message = Resource.Notifications.NotFound;
            }
            catch (Exception ex)
            {
                result.Status = TS.Status.ServerError;
                result.Message = ex.Message;
            }
            return result;
        }

    }
}
