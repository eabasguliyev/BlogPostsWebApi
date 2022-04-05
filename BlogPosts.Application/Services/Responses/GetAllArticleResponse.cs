using System;
using System.Collections.Generic;
using BlogPosts.Domain.Entities;

namespace BlogPosts.Application.Services.Responses
{
    public class GetAllArticleResponse
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public bool IsDeleted { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}