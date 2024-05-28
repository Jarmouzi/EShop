using AutoMapper;
using EShop.DataContext;
using EShop.Model;
using EShop.Model.TypeSafe;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace EShop.Repository.Implementation
{
    public class Repository<T, TViewModel, TContext> : IRepository<T, TViewModel> where T : BaseModel where TViewModel : BaseViewModel, new() where TContext : DbContext 
    {
        private readonly IUnitOfWork<TContext> _unitOfWork;
        private readonly IMapper _mappingEngine;
        private readonly DbSet<T> _service;

        public Repository(IUnitOfWork<TContext> unitOfWork, IMapper mappingEngine)
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
                entity.CreateDate = DateTime.Now;

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

        public async Task<Result<TViewModel>> UpdateAsync(TViewModel model)
        {
            var result = new Result<TViewModel>();
            result.Data = model;
            try
            {
                result.Data = model;

                var newModel = _service.Find(model.Id);
                _mappingEngine.Map(model, newModel);
                newModel.ModifyDate = DateTime.Now;

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

        public async Task<Result<Int64>> DeleteAsync(Int64 id)
        {
            var result = new Result<Int64>();
            result.Data = id;
            try
            {
                var item = _service.Find(id);
                if (item != null)
                {
                    item.ExpireDate = DateTime.Now;
                    //_service.Remove(item);
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
            result.Data = new List<TViewModel>();
            try
            {
                var item = await _service.Where(m => m.ExpireDate == null).ToArrayAsync();
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
            result.Data = new List<TViewModel>();

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

        public async Task<Result<IEnumerable<TResult>>> GetProcedureAsync<TResult>(string procedureName, SqlParameter[] sparams) where TResult : class
        {
            var result = new Result<IEnumerable<TResult>>();
            result.Data = new List<TResult>();

            try
            {
                string Query = $"exec {procedureName} " +
                    string.Join(", ", sparams.Select(m => m.ParameterName + (m.Direction == System.Data.ParameterDirection.Output ? " OUTPUT" : "")));
                var list = _unitOfWork.ExecWithStoreProcedure<TResult>(Query, sparams).ToList();

                if (list != null)
                {
                    result.Data = list;
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
        public async Task<Result<string>> GetProcedureAsync(string procedureName, string? jsonparams = null)
        {
            var result = new Result<string>();

            try
            {
                var p = "";
                var sp = new List<SqlParameter>();
                if (jsonparams != null)
                {
                    p = "@JsonParams";
                    sp.Add( new SqlParameter("JsonParams", jsonparams));
                }
                string Query = $"exec {procedureName} {p}";
                var list = _unitOfWork.ExecWithStoreProcedure<string>(Query, sp.ToArray()).FirstOrDefault();

                if (list != null)
                {
                    result.Data = list;
                    result.Status = TS.Status.Success;
                    return result;
                }
                result.Status = TS.Status.Warning;
                result.Message = Resource.Notifications.NotFound;
                result.Data = "[]";
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
            result.Data = new TViewModel();

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

        public async Task<Result<TViewModel?>> GetByIdAsync(Int64 id)
        {
            var result = new Result<TViewModel>();
            result.Data = new TViewModel();
            try
            {
                var item = await _service.FindAsync(id);
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