using AutoMapper;
using EShop.DataContext;
using EShop.Model;
using EShop.Model.TypeSafe;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Repository.Implementation
{
    public class CategoryRepository : Repository<Model.Category, CategoryViewModel, EShopContext>, ICategoryRepository
    {
        public CategoryRepository(IUnitOfWork<EShopContext> unitOfWork, IMapper mappingEngine) : base(unitOfWork, mappingEngine)
        {
        }

        public async Task<Result<PaginatedViewModel<CategoryViewModel>>> GetPaginatedResult(Guid? Level1Id = null, Guid? Level2Id = null, int take = 10, int skip = 0)
        {
            var result = new Result<PaginatedViewModel<CategoryViewModel>> ();

            try
            {
                var totalCount = new SqlParameter("@TotalCount", System.Data.SqlDbType.Int);
                totalCount.Direction = System.Data.ParameterDirection.Output;
                var sparam = new SqlParameter[] {
                new SqlParameter("@Level1Id", Level1Id == null ? DBNull.Value : Level1Id),
                new SqlParameter("@Level2Id", Level2Id == null ? DBNull.Value : Level2Id),
                new SqlParameter("@Take", take),
                    new SqlParameter("@Skip", skip),
                    totalCount
                };

                var r = await GetPrecedureAsync<CategoryViewModel>("Category_Get", sparam);

                if(r.Status == TS.Status.Success) {
                    result.Data = new PaginatedViewModel<CategoryViewModel>
                    {
                        Data = r.Data,
                        Pagination = new PaginationViewModel
                        {
                            Take = take,
                            Skip = skip,
                            TotalCount = Convert.ToInt32(totalCount.Value)
                        }
                    };
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
