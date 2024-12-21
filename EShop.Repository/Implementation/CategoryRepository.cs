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
    public class CategoryRepository : Repository<Category, CategoryViewModel, EShopContext>, ICategoryRepository
    {
        private readonly IUnitOfWork<EShopContext> _unitOfWork;
        private readonly DbSet<Category> _service;
        public CategoryRepository(IUnitOfWork<EShopContext> unitOfWork, IMapper mappingEngine) : base(unitOfWork, mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _service = _unitOfWork.Set<Category>();
        }

        public async Task<PaginatedViewModel<CategoryViewModel>> GetPaginatedResult(Int64? parentId = null, int take = 10, int skip = 0)
        {
            try
            {
                var totalCount = new SqlParameter("@TotalCount", System.Data.SqlDbType.Int);
                totalCount.Direction = System.Data.ParameterDirection.Output;
                var sparam = new SqlParameter[] {
                new SqlParameter("@ParentId", parentId == null ? DBNull.Value : parentId),
                new SqlParameter("@Take", take),
                    new SqlParameter("@Skip", skip),
                    totalCount
                };

                var r = await GetProcedureAsync<CategoryViewModel>("Category_Get", sparam);

                return new PaginatedViewModel<CategoryViewModel>
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

        public async Task<bool> ChangeDisplayOrder(Int64 id, int order)
        {
            try
            {
                var model = _service.Find(id);

                if (model == null) return false;

                model.ModifyDate = DateTime.Now;
                model.DisplayOrder += order;

                if (model.DisplayOrder < 1)
                {
                    model.DisplayOrder = 1;
                }
                else
                {
                    var exchangeModel = await _service.Where(m => m.DisplayOrder == model.DisplayOrder && m.ParentId == model.ParentId).FirstOrDefaultAsync();
                    if (exchangeModel != null)
                    {
                        exchangeModel.DisplayOrder += order * -1;
                    }
                }
                if (await _unitOfWork.SaveAsync() > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetGroupedChildren()
        {
            try
            {
                var sparam = new SqlParameter[] { };

                var result = await GetProcedureAsync<CategoryViewModel>("Category_GroupedChildren", sparam);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
