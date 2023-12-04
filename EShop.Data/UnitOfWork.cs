using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
using EShop.Model;

namespace EShop.DataContext
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        Task<int> SaveAsync();
        IEnumerable<T> ExecWithStoreProcedure<T>(string query, params object[] parameters);
        int ExecWithStoreProcedure(string query, params object[] parameters);
        Microsoft.EntityFrameworkCore.DbSet<TEntity> Set<TEntity>() where TEntity : class;

    }

    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        private readonly TContext _context;
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(TContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public new Microsoft.EntityFrameworkCore.DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IEnumerable<T> ExecWithStoreProcedure<T>(string query, params object[] parameters)

        {
            try
            {
                if (parameters == null)
                {
                    parameters = new object[] { };
                }

                var a = _context.Database.SqlQueryRaw<T>(query, parameters);
                return a;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int ExecWithStoreProcedure(string query, params object[] parameters)
        {
            try
            {
                return _context.Database.ExecuteSqlRaw(query, parameters);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
}
