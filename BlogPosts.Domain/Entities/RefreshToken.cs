using BlogPosts.Domain.Commons.EntityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogPosts.Domain.Entities
{
    public class RefreshToken:IEntity
    {
        public RefreshToken()
        {
        }
        public int Id { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevorked { get; set; }
        public string UserId { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Token { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
