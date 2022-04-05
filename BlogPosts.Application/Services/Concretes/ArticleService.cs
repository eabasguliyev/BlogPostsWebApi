using System.Collections.Generic;
using System.Threading.Tasks;
using BlogPosts.Application.Services.Abstracts;
using BlogPosts.Application.Services.Requests;
using BlogPosts.Application.Services.Responses;
using BlogPosts.Domain.Helpers;

namespace BlogPosts.Application.Services.Concretes
{
    public class ArticleService:IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ArticleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<List<GetAllArticleResponse>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<GetAllArticleResponse> Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<CreateArticleResponse> Create(CreateArticleRequest req)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task Update(UpdateArticleRequest req)
        {
            throw new System.NotImplementedException();
        }
    }
}