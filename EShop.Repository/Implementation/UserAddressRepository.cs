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
    public class UserAddressRepository : Repository<UserAddress, UserAddressViewModel, EShopContext>, IUserAddressRepository
    {
        private readonly IUnitOfWork<EShopContext> _unitOfWork;
        private readonly DbSet<UserAddress> _service;

        public UserAddressRepository(IUnitOfWork<EShopContext> unitOfWork, IMapper mappingEngine) : base(unitOfWork, mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _service = _unitOfWork.Set<UserAddress>();
        }
        public async Task RemoveOtherDefaultAddress(UserAddressViewModel model)
        {
            try
            {
                var userAddresses = _service.Where(m => m.UserId == model.UserId && m.IsDefault == true && m.Id != model.Id).ToList();

                foreach (var userAddress in userAddresses)
                {
                    userAddress.IsDefault = false;
                }

                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<PaginatedViewModel<UserAddressViewModel>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0)
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

                var r = await GetProcedureAsync<UserAddressViewModel>("UserAddress_Get", sparam);

                return new PaginatedViewModel<UserAddressViewModel>
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
