using BlogPosts.Application.Services.Abstracts;
using BlogPosts.Application.Services.Responses;
using BlogPosts.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogPosts.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly UserManager<User> _userManager;

        public ArticleController(IArticleService articleService, UserManager<User> userManager)
        {
            _articleService  = articleService;
            _userManager = userManager;
        }

        [HttpGet("GetById")]
        public async Task<GetArticleByIdResponse> GetById(int id)
        {
            return await _articleService.Get(id);
        }

        [HttpGet("GetAll")]
        public async Task<List<GetAllArticleResponse>> GetAll()
        {
            return await _articleService.GetAll();
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
            var id = Convert.ToInt32(HttpContext.User.Claims.First(i => i.Type == "Id").Value);

            var req2 = new CreateArticleWithAuthorRequest();
            req2.Title = req.Title;
            req2.Text = req.Text;
            req2.AuthorId = id;

            return await _articleService.Create(req2);
        }
    }
}