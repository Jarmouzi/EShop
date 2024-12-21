using AutoMapper;
using EShop.DataContext;
using EShop.Model;
using EShop.ViewModel;
using EShop.Model.TypeSafe;
using EShop.Repository.Interface;
using Microsoft.Data.SqlClient;

namespace EShop.Repository.Implementation
{
    public class CartItemRepository : Repository<CartItem, CartItemViewModel, EShopContext>, ICartItemRepository
    {
        public CartItemRepository(IUnitOfWork<EShopContext> unitOfWork, IMapper mappingEngine) : base(unitOfWork, mappingEngine)
        {
        }

        public async Task<PaginatedViewModel<CartItemViewModel>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<CartItemViewModel>();

            try
            {
                var totalCount = new SqlParameter("@TotalCount", System.Data.SqlDbType.Int);
                totalCount.Direction = System.Data.ParameterDirection.Output;
                var sparam = new SqlParameter[] {
                    new SqlParameter("@Title", title == null ? DBNull.Value : title),
                    new SqlParameter("@Take", take),
                    new SqlParameter("@Skip", skip),
                    totalCount
                };

                var r = await GetProcedureAsync<CartItemViewModel>("CartItem_Get", sparam);


                result = new PaginatedViewModel<CartItemViewModel>
                {
                    Data = r,
                    Pagination = new PaginationViewModel
                    {
                        Take = take,
                        Skip = skip,
                        TotalCount = Convert.ToInt32(totalCount.Value)
                    }
                };

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return result;
        }

    }
}
