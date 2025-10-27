using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Interfaces
{
    public interface IRepository<T> where T : class 
    {
        // Temel CRUD işlemleri için metod imzaları eklenebilir
        
        Task<T?> GetByIdAsync(object id);
        Task<List<T>> GetAllAsync();
        Task<T?> GetByFilterAsync(Expression<Func<T, bool>> filter);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        IQueryable<T> GetQuery();
        Task<List<T>> QueryToListAsync(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderByDescending = null, int ntake = 0,CancellationToken ct=default, params Expression<Func<T, object>>[] includes);
    }
}
