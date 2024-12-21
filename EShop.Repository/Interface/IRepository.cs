using EShop.ViewModel;
using EShop.Model;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using System.Text.Json.Nodes;

namespace EShop.Repository.Interface
{
    public interface IRepository<T, TViewModel> where T : BaseModel where TViewModel : BaseViewModel, new()
    {
        Task<TViewModel> AddAsync(TViewModel model);
        Task<TViewModel> UpdateAsync(TViewModel model);
        Task<Int64> DeleteAsync(Int64 id);
        Task<IEnumerable<TViewModel>> GetAllAsync();
        Task<IEnumerable<TViewModel>> GetAllAsync(Expression<Func<T, bool>> filter);
        Task<IEnumerable<TResult>> GetProcedureAsync<TResult>(string procedureName, SqlParameter[] sparams) where TResult : class;
        Task<string> GetProcedureAsync(string procedureName, string? jsonparams = null);
        Task<TViewModel?> GetAsync(Expression<Func<T, bool>> filter);
        Task<TViewModel?> GetByIdAsync(Int64 id);
        //Task<bool>> ExistsAsync(Int64 id);
        Task<IEnumerable<SelectItemViewModel>> GetAllItemAsync();
        Task<IEnumerable<SelectItemViewModel>> GetAllItemAsync(Expression<Func<T, bool>> filter);
    }
}