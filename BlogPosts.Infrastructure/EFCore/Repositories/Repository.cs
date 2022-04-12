using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BlogPosts.Domain.Commons.EntityBase;
using BlogPosts.Domain.Repositories;
using BlogPosts.Infrastructure.EFCore.Databases;
using BlogPosts.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BlogPosts.Infrastructure.EFCore.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly ApplicationDbContext _dbContext;
        protected DbSet<T> _dbSet;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            _dbSet = _dbContext.Set<T>();
        }

        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<T> Find(int id, bool noTracking = false, string includeProperties = null)
        {
            IQueryable<T> query = _dbSet.Where(e => e.Id == id);

            query = includeProperties != null ? query.IncludeProperties(includeProperties) : query;

            query = noTracking ? query.AsNoTracking() : query;

            return await query.FirstOrDefaultAsync();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> expression = null, bool noTracking = false, string includeProperties = null)
        {
            IQueryable<T> query = expression != null ? _dbSet.Where(expression) : _dbSet;

            query = includeProperties != null ? query.IncludeProperties(includeProperties) : query;

            query = noTracking ? query.AsNoTracking() : query;

            return query;
        }

        public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> expression, bool noTracking = false, string includeProperties = null)
        {
            IQueryable<T> query = _dbSet.Where(expression);

            query = includeProperties != null ? query.IncludeProperties(includeProperties) : query;

            query = noTracking ? query.AsNoTracking() : query;

            return await query.FirstOrDefaultAsync();
        }

        public Task Remove(T entity)
        {
            entity.IsDeleted = true;

            return Task.CompletedTask;
        }

        public Task RemoveRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
            }

            return Task.CompletedTask;
        }
    }
}
