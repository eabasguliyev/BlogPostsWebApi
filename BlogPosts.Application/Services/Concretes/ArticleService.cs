using System.Collections.Generic;
using System.Linq;
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
        public async Task<List<GetAllArticleResponse>> GetAll()
        {
            var result = await _unitOfWork.ArticleRepository.GetAll(noTracking: true, includeProperties: "Author");
            return result.Select(r => new GetAllArticleResponse
            {
                Id = r.Id,
                AuthorId = r.AuthorId,
                Author = r.Author,
                CreateDate = r.CreateDate,
                IsDeleted = r.IsDeleted,
                ModifyDate = r.ModifyDate,
                Text = r.Text,
                Title = r.Title
            }).ToList();
        }

        public Task<GetAllArticleResponse> Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<CreateArticleResponse> Create(CreateArticleRequest req)
        {
            var newArticle = new Domain.Entities.Article()
            {
                Title = req.Title,
                Text = req.Text,
                AuthorId = req.AuthorId
            };
            
            await _unitOfWork.ArticleRepository.Add(newArticle);

            return new CreateArticleResponse()
            {
                Status = true,
                Message = "Article Created"
            };
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