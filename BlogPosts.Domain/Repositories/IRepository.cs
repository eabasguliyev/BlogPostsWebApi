using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BlogPosts.Domain.Commons.EntityBase;

namespace BlogPosts.Domain.Repositories
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<T> Find(int id, bool noTracking = false, string? includeProperties = null);
        Task<T> GetFirstOrDefault(Expression<Func<T, bool>> expression, bool noTracking = false, string? includeProperties = null);
        IQueryable<T> GetAll(Expression<Func<T, bool>>? expression = null, bool noTracking = false, string? includeProperties = null);
        Task Add(T entity);
        Task Remove(T entity);
        Task RemoveRange(IEnumerable<T> entities);
    }
}
