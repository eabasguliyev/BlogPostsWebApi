using System.Threading.Tasks;
using BlogPosts.Application.Services.Requests;
using BlogPosts.Application.Services.Responses;

namespace BlogPosts.Application.Services.Abstracts
{
    public interface IUserService
    {
        Task<UserLoginResponse> Login(UserLoginRequest userLoginRequest);
        Task<UserRegisterResponse> Register(UserRegisterRequest userRegisterRequest);
        Task Logout();
    }
}