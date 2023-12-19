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
        Task<Result<PaginatedViewModel<CategoryViewModel>>> GetPaginatedResult(Guid? Level1Id = null, Guid? Level2Id = null, int take = 10, int skip = 0);
    }
}
