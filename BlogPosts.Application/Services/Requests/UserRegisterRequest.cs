using BlogPosts.Domain.Commons.Responses;

namespace BlogPosts.Application.Services.Requests
{
    public class UserRegisterRequest : IDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName{ get; set; }
        public string LastName { get; set; }
    }
}