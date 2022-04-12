using BlogPosts.Authentication.Models.DTOs.Responses;
using System.Collections.Generic;

namespace BlogPosts.Application.Services.Responses
{
    public class UserRegisterResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public List<ValidationError> ValidationErrors { get; set; }
        public AuthResult Token { get; internal set; }
    }
}