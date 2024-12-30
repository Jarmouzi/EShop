using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IUserAddressRepository: IRepository<UserAddress, UserAddressViewModel>
    {
        Task RemoveOtherDefaultAddress(UserAddressViewModel model);
        Task<PaginatedViewModel<UserAddressViewModel>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
