using EShop.ViewModel;
using EShop.Model;
using System.Linq.Expressions;

namespace EShop.IService
{
    public interface IBaseService<T, TViewModel> where T : BaseModel where TViewModel : BaseModel
    {
        Task<Result<TViewModel>> AddAsync(TViewModel model);
        Task<Result<TViewModel>> Update(TViewModel model);
        Task<Result<Guid>> Delete(Guid id);
        Task<Result<IEnumerable<TViewModel>>> GetAllAsync();
        Task<Result<IEnumerable<TViewModel>>> GetAllAsync(Expression<Func<T, bool>> filter);
        Task<Result<TViewModel?>> GetAsync(Expression<Func<T, bool>> filter);
        Result<TViewModel?> GetByIdAsync(Guid id);
        //Task<Result<bool>> ExistsAsync(Guid id);
    }
}