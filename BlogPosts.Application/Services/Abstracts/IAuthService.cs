using System.Threading.Tasks;
using BlogPosts.Application.Services.Requests;
using BlogPosts.Application.Services.Responses;
using BlogPosts.Authentication.Models.DTOs.Requests;
using BlogPosts.Authentication.Models.DTOs.Responses;
using BlogPosts.Domain.Entities;

namespace BlogPosts.Application.Services.Abstracts
{
    public interface IAuthService
    {
        Task<UserLoginResponse> Login(UserLoginRequest userLoginRequest);
        Task<UserRegisterResponse> Register(UserRegisterRequest userRegisterRequest);
        Task<AuthResult> GenerateJwtToken(User user);
        Task<AuthResult> VerifyAndGenerateToken(TokenRequest tokenRequest);
        Task Logout();
    }
}