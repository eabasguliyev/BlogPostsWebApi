using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogPosts.Application.Services.Responses
{
    public class UserLoginResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public List<ValidationError> ValidationErrors { get; set; }
        
    }

    public record ValidationError(string Name, string Message);
}
