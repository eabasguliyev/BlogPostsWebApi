using System.Collections.Generic;

namespace BlogPosts.Application.Services.Responses
{
    public class CreateArticleResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public List<ValidationError> ValidationErrors { get; set; }
    }
}