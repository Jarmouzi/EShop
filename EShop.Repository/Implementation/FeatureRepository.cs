using AutoMapper;
using EShop.DataContext;
using EShop.Model;
using EShop.ViewModel;
using EShop.Model.TypeSafe;
using EShop.Repository.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EShop.Repository.Implementation
{
    public class FeatureRepository : Repository<Feature, FeatureViewModel, EShopContext>, IFeatureRepository
    {
        private readonly IUnitOfWork<EShopContext> _unitOfWork;
        private readonly DbSet<Feature> _service;
        public FeatureRepository(IUnitOfWork<EShopContext> unitOfWork, IMapper mappingEngine) : base(unitOfWork, mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _service = _unitOfWork.Set<Feature>();
        }

        public async Task<Result<PaginatedViewModel<FeatureViewModel>>> GetPaginatedResult(Int64? categoryId = null, Int64? parentId = null, string? title = null, int take = 10, int skip = 0)
        {
            var result = new Result<PaginatedViewModel<FeatureViewModel>> ();

            try
            {
                var totalCount = new SqlParameter("@TotalCount", System.Data.SqlDbType.Int);
                totalCount.Direction = System.Data.ParameterDirection.Output;
                var sparam = new SqlParameter[] {
                    new SqlParameter("@CategoryId", categoryId == null ? DBNull.Value : categoryId),
                    new SqlParameter("@ParentId", parentId == null ? DBNull.Value : parentId),
                    new SqlParameter("@Title", title == null ? DBNull.Value : title),
					new SqlParameter("@Take", take),
                    new SqlParameter("@Skip", skip),
                    totalCount
                };

                var r = await GetProcedureAsync<FeatureViewModel>("Feature_Get", sparam);

                if(r.Status == TS.Status.Success || r.Status == TS.Status.Warning) {
                    result.Data = new PaginatedViewModel<FeatureViewModel>
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

    }
}
