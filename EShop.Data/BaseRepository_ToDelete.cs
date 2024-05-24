//using EShop.Model;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace EShop.DataContext
//{
//    public interface IBaseRepository_ToDelete<T> where T : BaseModel
//    {
//        Task<IEnumerable<T>> GetAllAsync();
//        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter);
//        Task<T?> GetAsync(Expression<Func<T, bool>> filter);
//        Task<T?> GetByIdAsync(Int64 id);
//        Task AddAsync(T entity);
//        void Update(T entity);
//        void Delete(T entity);
//        Task<bool> ExistsAsync(Int64 id);
//    }

//    public class BaseRepository_ToDelete<T> : IBaseRepository_ToDelete<T> where T : BaseModel
//    {
//        protected readonly EShopContext _context;

//        public BaseRepository_ToDelete(EShopContext context)
//        {
//            _context = context;
//        }

//        public virtual async Task<IEnumerable<T>> GetAllAsync()
//        {
//            return await _context.Set<T>().ToListAsync();
//        }
//        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter)
//        {
//            return await _context.Set<T>().Where(filter).ToListAsync();
//        }

//        public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> filter)
//        {
//            return await _context.Set<T>().Where(filter).FirstOrDefaultAsync();
//        }

//        //public virtual async Task<T?> GetByIdAsync(Int64 id)
//        //{
//        //    return await _context.Set<T>().FindAsync(id);
//        //}

//        //public virtual async Task AddAsync(T entity)
//        //{
//        //    await _context.Set<T>().AddAsync(entity);
//        //}

//        public virtual void Update(T entity)
//        {
//            _context.Entry(entity).State = EntityState.Modified;
//        }

//        public virtual void Delete(T entity)
//        {
//            _context.Set<T>().Remove(entity);
//        }

//        public virtual async Task<bool> ExistsAsync(Int64 id)
//        {
//            return await _context.Set<T>().AnyAsync(x => x.Id == id);
//        }
//    }
//}
