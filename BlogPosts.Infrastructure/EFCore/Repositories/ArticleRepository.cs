using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogPosts.Domain.Entities;
using BlogPosts.Domain.Repositories;
using BlogPosts.Infrastructure.EFCore.Databases;

namespace BlogPosts.Infrastructure.EFCore.Repositories
{
    public class ArticleRepository:Repository<Article>, IArticleRepository
    {
        public ArticleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public Task Update(Article entity)
        {
            _dbSet.Update(entity);

            return Task.CompletedTask;
        }
    }

    public class AuthorRepository:Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
