using SeniorVlogger.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SeniorVlogger.DataAccess.Data;

namespace SeniorVlogger.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        #region Fields

        private readonly ApplicationDbContext _db;
        internal DbSet<T> _dbSet;

        #endregion

        #region Constructor

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            _dbSet = db.Set<T>();
        }

        #endregion

        #region IRepository Implementation

        public async Task<T> Get(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll(
            Expression<Func<T, bool>> filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            string includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            query = AddFilter(query, filter);
            query = AddIncludeProperties(query, includeProperties);

            return (orderBy == null) ? await query.ToListAsync() : await orderBy(query).ToListAsync();
        }

        public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            query = AddFilter(query, filter);
            query = AddIncludeProperties(query, includeProperties);

            return await query.FirstOrDefaultAsync();
        }

        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRange(IEnumerable<T> entity)
        {
            await _dbSet.AddRangeAsync(entity);
        }

        public async Task Remove(int id)
        {
            T entity = await _dbSet.FindAsync(id);
            await Remove(entity);
        }

        public Task Remove(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public Task RemoveRange(IEnumerable<T> entity)
        {
            _dbSet.RemoveRange(entity);
            return Task.CompletedTask;
        }

        #endregion

        #region Private Methods
        private IQueryable<T> AddIncludeProperties(IQueryable<T> query, string includeProperties)
        {
            if (string.IsNullOrEmpty(includeProperties)) return query;

            var splittedProperties = includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var includeProperty in splittedProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        private IQueryable<T> AddFilter(IQueryable<T> query, Expression<Func<T, bool>> filter)
        {
            if (filter == null) return query;
            query = query.Where(filter);
            return query;
        }

        #endregion
    }
}
