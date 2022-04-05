using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogPosts.Domain.Helpers;
using BlogPosts.Domain.Repositories;
using BlogPosts.Infrastructure.EFCore.Databases;
using BlogPosts.Infrastructure.EFCore.Repositories;

namespace BlogPosts.Infrastructure.EFCore.Helpers
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            ArticleRepository = new ArticleRepository(dbContext);
        }

        public IArticleRepository ArticleRepository { get; }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}