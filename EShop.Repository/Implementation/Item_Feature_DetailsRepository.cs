using AutoMapper;
using EShop.DataContext;
using EShop.Model;
using EShop.ViewModel;
using EShop.Model.TypeSafe;
using EShop.Repository.Interface;
using Microsoft.Data.SqlClient;

namespace EShop.Repository.Implementation
{
    public class Item_Feature_DetailsRepository : Repository<Item_Feature_Details, Item_Feature_DetailsViewModel, EShopContext>, IItem_Feature_DetailsRepository
    {
        public Item_Feature_DetailsRepository(IUnitOfWork<EShopContext> unitOfWork, IMapper mappingEngine) : base(unitOfWork, mappingEngine)
        {
        }

        public async Task<Result<PaginatedViewModel<Item_Feature_DetailsViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0)
        {
            var result = new Result<PaginatedViewModel<Item_Feature_DetailsViewModel>> ();

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

                var r = await GetPrecedureAsync<Item_Feature_DetailsViewModel>("Item_Feature_Details_Get", sparam);

                if(r.Status == TS.Status.Success) {
                    result.Data = new PaginatedViewModel<Item_Feature_DetailsViewModel>
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
