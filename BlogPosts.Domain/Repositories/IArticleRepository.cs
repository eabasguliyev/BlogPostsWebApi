using System.Threading.Tasks;
using BlogPosts.Domain.Entities;

namespace BlogPosts.Domain.Repositories
{
    public interface IArticleRepository : IRepository<Article>
    {
        Task Update(Article entity);
    }
}