using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IUserProfileRepository: IRepository<UserProfile, UserProfileViewModel>
    {
        Task<Result<PaginatedViewModel<UserProfileViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
