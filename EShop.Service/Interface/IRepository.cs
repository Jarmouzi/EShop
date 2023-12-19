using EShop.ViewModel;
using EShop.Model;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using System.Text.Json.Nodes;

namespace EShop.Repository.Interface
{
    public interface IRepository<T, TViewModel> where T : BaseModel where TViewModel : BaseViewModel
    {
        Task<Result<TViewModel>> AddAsync(TViewModel model);
        Task<Result<TViewModel>> Update(TViewModel model);
        Task<Result<Guid>> Delete(Guid id);
        Task<Result<IEnumerable<TViewModel>>> GetAllAsync();
        Task<Result<IEnumerable<TViewModel>>> GetAllAsync(Expression<Func<T, bool>> filter);
        Task<Result<IEnumerable<TResult>>> GetPrecedureAsync<TResult>(string procedureName, SqlParameter[] sparams) where TResult : class;
        Task<Result<JsonArray>> GetPrecedureAsync(string procedureName, SqlParameter[] sparams);
        Task<Result<TViewModel?>> GetAsync(Expression<Func<T, bool>> filter);
        Task<Result<TViewModel?>> GetByIdAsync(Guid id);
        //Task<Result<bool>> ExistsAsync(Guid id);
    }
}