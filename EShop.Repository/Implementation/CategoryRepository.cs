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

        public async Task<Result<PaginatedViewModel<CategoryViewModel>>> GetPaginatedResult(Int64? parentId = null, int take = 10, int skip = 0)
        {
            var result = new Result<PaginatedViewModel<CategoryViewModel>> ();

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

        public async Task<Result<bool>> ChangeDisplayOrder(Int64 id, int order)
        {
            var result = new Result<bool>();
            try
            {
                result.Data = false;

                var model = _service.Find(id);
                model.ModifyDate = DateTime.Now;
                model.DisplayOrder += order;

                var exchangeModel = await _service.Where(m => m.DisplayOrder == model.DisplayOrder && m.ParentId == model.ParentId).FirstOrDefaultAsync();
                if (exchangeModel != null)
                {
                    exchangeModel.DisplayOrder += order * -1;
                }
                if (await _unitOfWork.SaveAsync() > 0)
                {
                    result.Data = true;
                    result.Message = Resource.Notifications.SuccessfulUpdate;
                    result.Status = TS.Status.Success;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Status = TS.Status.ServerError;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<Result<IEnumerable<CategoryViewModel>>> GetGroupedChildren()
        {
            var result = new Result<IEnumerable<CategoryViewModel>>();

            try
            {
                var sparam = new SqlParameter[] {};

                var r = await GetProcedureAsync<CategoryViewModel>("Category_GroupedChildren", sparam);

                if (r.Status == TS.Status.Success)
                {
                    result.Data =  r.Data;
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
