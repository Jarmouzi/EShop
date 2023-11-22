using AutoMapper;
using EShop.DataContext;
using EShop.Model;
using EShop.Model.TypeSafe;
using EShop.Service.Interface;
using EShop.ViewModel;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace EShop.Service.Implementation
{
    public class Repository<T, TViewModel> : IRepository<T, TViewModel> where T : BaseModel where TViewModel : BaseModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mappingEngine;
        private readonly DbSet<T> _service;

        public Repository(IUnitOfWork unitOfWork, IMapper mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _mappingEngine = mappingEngine;

            _service = _unitOfWork.Set<T>();
        }
        public async Task<Result<TViewModel>> AddAsync(TViewModel model)
        {
            var result = new Result<TViewModel>();
            result.Data = model;
            try
            {
                var entity = _mappingEngine.Map<T>(model);
                _service.Add(entity);
                if (await _unitOfWork.SaveAsync() > 0)
                {
                    result.Data.Id = entity.Id;
                    result.Message = Resource.Notifications.SuccessfulInsert;
                    result.Status = TS.Status.Success;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Status = TS.Status.ServerError;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<Result<TViewModel>> Update(TViewModel model)
        {
            var result = new Result<TViewModel>();
            try
            {
                result.Data = model;

                var newModel = _service.Find(model.Id);
                _mappingEngine.Map(model, newModel);
                result.Data = _mappingEngine.Map<TViewModel>(newModel);

                if (await _unitOfWork.SaveAsync() > 0)
                {
                    result.Message = Resource.Notifications.SuccessfulUpdate;
                    result.Status = TS.Status.Success;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Status = TS.Status.ServerError;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<Result<Guid>> Delete(Guid id)
        {
            var result = new Result<Guid>();
            result.Data = id;
            try
            {
                var item = _service.Find(id);
                if (item != null)
                {
                    _service.Remove(item);
                    if (await _unitOfWork.SaveAsync() > 0)
                    {
                        result.Message = Resource.Notifications.SuccessfulDelete;
                        result.Status = TS.Status.Success;
                        return result;
                    }
                }
                result.Status = "warning"; result.Message = Resource.Notifications.NotFound;
            }
            catch (Exception ex)
            {
                result.Status = TS.Status.ServerError;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<Result<IEnumerable<TViewModel>>> GetAllAsync()
        {
            var result = new Result<IEnumerable<TViewModel>>();

            try
            {
                var item = await _service.ToArrayAsync();
                if (item != null)
                {
                    result.Data = _mappingEngine.Map<IEnumerable<TViewModel>>(item);
                    result.Status = TS.Status.Success;
                    return result;
                }
                result.Status = TS.Status.Warning;
                result.Message = Resource.Notifications.NotFound;
            }
            catch (Exception ex)
            {
                result.Status = TS.Status.ServerError;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<Result<IEnumerable<TViewModel>>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            var result = new Result<IEnumerable<TViewModel>>();

            try
            {
                var item = await _service.Where(filter).ToListAsync();
                if (item != null)
                {
                    result.Data = _mappingEngine.Map<IEnumerable<TViewModel>>(item);
                    result.Status = TS.Status.Success;
                    return result;
                }
                result.Status = TS.Status.Warning;
                result.Message = Resource.Notifications.NotFound;
            }
            catch (Exception ex)
            {
                result.Status = TS.Status.ServerError;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<Result<TViewModel?>> GetAsync(Expression<Func<T, bool>> filter)
        {
            var result = new Result<TViewModel>();

            try
            {
                var item = await _service.Where(filter).FirstOrDefaultAsync();
                if (item != null)
                {
                    result.Data = _mappingEngine.Map<TViewModel>(item);
                    result.Status = TS.Status.Success;
                    return result;
                }
                result.Status = TS.Status.Warning;
                result.Message = Resource.Notifications.NotFound;
            }
            catch (Exception ex)
            {
                result.Status = TS.Status.ServerError;
                result.Message = ex.Message;
            }
            return result;
        }

        public Result<TViewModel?> GetByIdAsync(Guid id)
        {
            var result = new Result<TViewModel>();
            try
            {
                var item = _service.Find(id);
                if (item != null)
                {
                    result.Data = _mappingEngine.Map<TViewModel>(item);
                    result.Status = TS.Status.Success;
                    return result;
                }
                result.Status = TS.Status.Warning;
                result.Message = Resource.Notifications.NotFound;
            }
            catch (Exception ex)
            {
                result.Status = TS.Status.ServerError;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}