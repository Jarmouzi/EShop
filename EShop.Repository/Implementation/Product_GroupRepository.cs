using AutoMapper;
using EShop.DataContext;
using EShop.Model;
using EShop.ViewModel;
using EShop.Model.TypeSafe;
using EShop.Repository.Interface;
using Microsoft.Data.SqlClient;

namespace EShop.Repository.Implementation
{
    public class Product_GroupRepository : Repository<Product_Group, Product_GroupViewModel, EShopContext>, IProduct_GroupRepository
    {
        public Product_GroupRepository(IUnitOfWork<EShopContext> unitOfWork, IMapper mappingEngine) : base(unitOfWork, mappingEngine)
        {
        }

        public async Task<PaginatedViewModel<Product_GroupViewModel>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0)
        {
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

                var r = await GetProcedureAsync<Product_GroupViewModel>("Product_Group_Get", sparam);


                return new PaginatedViewModel<Product_GroupViewModel>
                {
                    Data = r,
                    Pagination = new PaginationViewModel
                    {
                        Take = take,
                        Skip = skip,
                        TotalCount = Convert.ToInt32(totalCount.Value)
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
