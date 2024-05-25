using EShop.LogService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.LogService.Repository
{
    public interface ILogRepository
    {
        Task<Int64?> AddVisitLogAsync(VisitLog model);
        Task<VisitLog> GetLastVisitLogAsync(VisitLog model);
        Task<int> AddActionLogAsync(ActionLog model);
        Task<int> AddErrorLogAsync(ErrorLog model);
        Task<IEnumerable<ActionLog>> GetActionLogListAsync(ActionLog model, int take, int skip);
    }
}
