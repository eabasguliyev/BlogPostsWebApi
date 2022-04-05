using System.Collections.Generic;
using System.Threading.Tasks;
using BlogPosts.Application.Services.Requests;
using BlogPosts.Application.Services.Responses;

namespace BlogPosts.Application.Services.Abstracts
{
    public interface IArticleService
    {
        Task<List<GetAllArticleResponse>> GetAll();
        Task<GetAllArticleResponse> Get(int id);
        Task<CreateArticleResponse> Create(CreateArticleRequest req);
        Task Delete(int id);
        Task Update(UpdateArticleRequest req);
    }
}