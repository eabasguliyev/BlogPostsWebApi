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
            var result = _unitOfWork.ArticleRepository.GetAll(noTracking: true, includeProperties: "Author").ToList();
            return result.Select(r => new GetAllArticleResponse
            {
                Id = r.Id,
                AuthorId = r.AuthorId,
                CreateDate = r.CreateDate,
                IsDeleted = r.IsDeleted,
                ModifyDate = r.ModifyDate,
                Text = r.Text,
                Title = r.Title
            }).ToList();
        }

        public async Task<GetArticleByIdResponse> Get(int id)
        {
            var article = await _unitOfWork.ArticleRepository.GetFirstOrDefault(a => a.Id == id, noTracking: true, includeProperties: "Author");

            return new GetArticleByIdResponse()
            {
                Id = article.Id,
                CreateDate = article.CreateDate,
                ModifyDate = article.ModifyDate,
                Title = article.Title,
                Text = article.Text,
                Author = new GetArticleByIdAuthorResponse()
                {
                    Id = article.AuthorId,
                    FirstName = article.Author.FirstName,
                    LastName = article.Author.LastName
                }
            };
        }

        public async Task<CreateArticleResponse> Create(CreateArticleWithAuthorRequest req)
        {
            var newArticle = new Domain.Entities.Article()
            {
                Title = req.Title,
                Text = req.Text,
                AuthorId = req.AuthorId
            };
            
            await _unitOfWork.ArticleRepository.Add(newArticle);
            await _unitOfWork.Save();
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