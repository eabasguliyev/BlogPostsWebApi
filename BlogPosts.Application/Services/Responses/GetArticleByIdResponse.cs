using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogPosts.Application.Services.Responses
{
    public class GetArticleByIdResponse
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }

        public GetArticleByIdAuthorResponse Author { get; set; }
    }
}
