using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogPosts.Domain.Entities
{
    public class Author:User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
