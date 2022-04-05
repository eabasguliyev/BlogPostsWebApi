using System;
using BlogPosts.Domain.Commons.EntityBase;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogPosts.Domain.Entities
{
    public class Article: IEntity
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