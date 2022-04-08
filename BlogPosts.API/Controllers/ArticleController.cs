using BlogPosts.Application.Services.Abstracts;
using BlogPosts.Application.Services.Responses;
using BlogPosts.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogPosts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService  = articleService;
        }

        [HttpGet("GetAll")]
        public List<Article> GetAll()
        {
            return new List<Article>();
        }

        [HttpPost("Create")]
        public async Task<CreateArticleResponse> Create(CreateArticleRequest req)
        {
            if (!ModelState.IsValid)
            {
                return new CreateArticleResponse {
                    Message = "Inputs incorrect",
                    Status = false
                };
            }

            req.AuthorId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return await _articleService.Create(req);
        }
    }
}
