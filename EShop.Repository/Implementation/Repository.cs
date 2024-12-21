using AutoMapper;
using EShop.Model;
using EShop.Model.TypeSafe;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using EShop.DataContext;
using Microsoft.Data.SqlClient;

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
        public async Task<TViewModel> AddAsync(TViewModel model)
        {
            var result = new TViewModel();
            try
            {
                var entity = _mappingEngine.Map<T>(model);
                entity.CreateDate = DateTime.Now;

                _service.Add(entity);
                if (await _unitOfWork.SaveAsync() > 0)
                {
                    result.Id = entity.Id;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<TViewModel> UpdateAsync(TViewModel model)
        {
            var result = new TViewModel();
            try
            {
                result = model;

                var newModel = _service.Find(model.Id);
                _mappingEngine.Map(model, newModel);
                newModel.ModifyDate = DateTime.Now;

                result = _mappingEngine.Map<TViewModel>(newModel);

                if (await _unitOfWork.SaveAsync() > 0)
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<Int64> DeleteAsync(Int64 id)
        {
            var result = id;
            try
            {
                var item = _service.Find(id);
                if (item != null)
                {
                    item.ExpireDate = DateTime.Now;
                    //_service.Remove(item);
                    if (await _unitOfWork.SaveAsync() > 0)
                    {
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<IEnumerable<TViewModel>> GetAllAsync()
        {
            IEnumerable<TViewModel> result = new List<TViewModel>();
            try
            {
                var item = await _service.Where(m => m.ExpireDate == null).ToArrayAsync();
                if (item != null)
                {
                    result = _mappingEngine.Map<IEnumerable<TViewModel>>(item);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<IEnumerable<TViewModel>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            IEnumerable<TViewModel> result = new List<TViewModel>();

            try
            {
                var item = await _service.Where(filter).ToListAsync();
                if (item != null)
                {
                    result = _mappingEngine.Map<IEnumerable<TViewModel>>(item);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<IEnumerable<TResult>> GetProcedureAsync<TResult>(string procedureName, SqlParameter[] sparams) where TResult : class
        {
            List<TResult> result = new List<TResult>();

            try
            {
                string Query = $"exec {procedureName} " +
                    string.Join(", ", sparams.Select(m => m.ParameterName + (m.Direction == System.Data.ParameterDirection.Output ? " OUTPUT" : "")));
                var list = _unitOfWork.ExecWithStoreProcedure<TResult>(Query, sparams).ToList();

                if (list != null)
                {
                    result = list;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public async Task<string> GetProcedureAsync(string procedureName, string? jsonparams = null)
        {
            var result = "new string> ()";

            try
            {
                var p = "";
                var sp = new List<SqlParameter>();
                if (jsonparams != null)
                {
                    p = "@JsonParams";
                    sp.Add(new SqlParameter("JsonParams", jsonparams));
                }
                string Query = $"exec {procedureName} {p}";
                var list = _unitOfWork.ExecWithStoreProcedure<string>(Query, sp.ToArray()).FirstOrDefault();

                if (list != null)
                {
                    result = list;

                    return result;
                }


                result = "[]";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<TViewModel?> GetAsync(Expression<Func<T, bool>> filter)
        {
            var result = new TViewModel();

            try
            {
                var item = await _service.Where(filter).FirstOrDefaultAsync();
                if (item != null)
                {
                    result = _mappingEngine.Map<TViewModel>(item);

                    return result;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<TViewModel?> GetByIdAsync(Int64 id)
        {
            var result = new TViewModel();
            try
            {
                var item = await _service.FindAsync(id);
                if (item != null)
                {
                    result = _mappingEngine.Map<TViewModel>(item);

                    return result;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<IEnumerable<SelectItemViewModel>> GetAllItemAsync()
        {
            IEnumerable<SelectItemViewModel> result = [];
            try
            {
                var item = await _service.Where(m => m.ExpireDate == null).ToArrayAsync();
                if (item != null)
                {
                    result = _mappingEngine.Map<IEnumerable<SelectItemViewModel>>(item);

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<IEnumerable<SelectItemViewModel>> GetAllItemAsync(Expression<Func<T, bool>> filter)
        {
            IEnumerable<SelectItemViewModel> result = new List<SelectItemViewModel>();

            try
            {
                var item = await _service.Where(filter).ToListAsync();
                if (item != null)
                {
                    result = _mappingEngine.Map<IEnumerable<SelectItemViewModel>>(item);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

    }
}