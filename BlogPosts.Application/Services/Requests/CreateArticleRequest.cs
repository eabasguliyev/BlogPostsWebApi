using System;
using BlogPosts.Domain.Entities;

namespace BlogPosts.Application.Services.Responses
{
    public class CreateArticleRequest
    {
        public string Title { get; set; }
        public string Text { get; set; }

        public int AuthorId { get; set; }
    }
}