using System;
using BlogPosts.Domain.Commons.EntityBase;
using Microsoft.AspNetCore.Identity;

namespace BlogPosts.Domain.Entities
{
    public class User : IdentityUser<int>, IEntity
    {
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}