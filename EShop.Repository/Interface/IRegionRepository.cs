using EShop.DataContext;
using EShop.Repository.Implementation;
using EShop.ViewModel;
using EShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Repository.Interface
{
    public interface IRegionRepository: IRepository<Region, RegionViewModel>
    {
        Task<Result<PaginatedViewModel<RegionViewModel>>> GetPaginatedResult(string? title = null, string? country = null, int take = 10, int skip = 0);
    }
}
