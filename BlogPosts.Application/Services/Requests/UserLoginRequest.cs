using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogPosts.Domain.Commons.Responses;

namespace BlogPosts.Application.Services.Requests
{
    public class UserLoginRequest:IDto
    {
        public string  Username { get; set; }
        public string  Password { get; set; }
    }
}
