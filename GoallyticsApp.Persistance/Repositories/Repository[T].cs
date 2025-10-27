using GoallyticsApp.Application.Interfaces;
using GoallyticsApp.Domain.Entities;
using GoallyticsApp.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Persistance.Repositories
{
    public class Repository<T> : IRepository<T> 
        where T : BaseEntity 
        
    {
        private readonly MatchContext _context;
        public Repository(MatchContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<List<T>> GetAllAsync()
        {
            var result = await _context.Set<T>().AsNoTracking().ToListAsync();
            return result;
        }

        public async Task<T?> GetByFilterAsync(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(filter);
        }

        public async Task<T?> GetByIdAsync(object id)
        {
           return await _context.Set<T>().FindAsync(id);
        }

        public IQueryable<T> GetQuery()
        {
            return _context.Set<T>();
        }
        public async Task<List<T>> QueryToListAsync(
            Expression<Func<T,bool>> filter=null,
            Expression<Func<T,object>> orderByDescending=null,
            int ntake = 0,
            CancellationToken ct =default,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> queryable = _context.Set<T>().AsQueryable();

            if (filter != null)
                queryable = queryable.Where(filter);
            if (includes != null)
                foreach (var include in includes)
                {
                    queryable = queryable.Include(include);
                }
            if (orderByDescending != null)
                queryable = queryable.OrderByDescending(orderByDescending);
            if (ntake != 0)
                queryable = queryable.Take(ntake);
            
            return await queryable.ToListAsync(ct);
        }
        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
