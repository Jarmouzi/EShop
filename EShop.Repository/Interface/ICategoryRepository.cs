using EShop.DataContext;
using EShop.Repository.Implementation;
using EShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Repository.Interface
{
    public interface ICategoryRepository: IRepository<Model.Category, CategoryViewModel>
    {
        Task<PaginatedViewModel<CategoryViewModel>> GetPaginatedResult(Int64? parentId = null, int take = 10, int skip = 0);
        Task<bool> ChangeDisplayOrder(Int64 id, int order);
        Task<IEnumerable<CategoryViewModel>> GetGroupedChildren();
    }
}
