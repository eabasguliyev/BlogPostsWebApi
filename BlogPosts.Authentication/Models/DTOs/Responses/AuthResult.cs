using System;
using System.Collections.Generic;
using System.Text;

namespace BlogPosts.Authentication.Models.DTOs.Responses
{
    public class AuthResult
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
