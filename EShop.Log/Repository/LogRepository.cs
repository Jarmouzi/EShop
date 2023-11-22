using EShop.LogService.DataContext;
using EShop.LogService.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.LogService.Repository
{
    public class LogRepository : ILogRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<VisitLog> _visitService;
        private readonly DbSet<ActionLog> _actionService;
        public LogRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _visitService = _unitOfWork.Set<VisitLog>();

            _actionService = _unitOfWork.Set<ActionLog>();
        }
        public async Task<int> AddActionLogAsync(ActionLog model)
        {
            try
            {
                _actionService.Add(model);
                return await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public async Task<Guid?> AddVisitLogAsync(VisitLog model)
        {
            try
            {
                _visitService.Add(model);
                if (await _unitOfWork.SaveAsync() > 0)
                {
                    return model.Id;
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public Task<IEnumerable<ActionLog>> GetActionLogListAsync(ActionLog model, int take, int skip)
        {
            throw new NotImplementedException();
        }

        public Task<VisitLog> GetLastVisitLogAsync(VisitLog model)
        {
            throw new NotImplementedException();
        }
    }
}
