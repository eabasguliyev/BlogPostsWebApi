using System.Threading.Tasks;
using BlogPosts.Domain.Repositories;

namespace BlogPosts.Domain.Helpers
{
    public interface IUnitOfWork
    {
        public IArticleRepository ArticleRepository { get; }
        Task Save();
    }

}