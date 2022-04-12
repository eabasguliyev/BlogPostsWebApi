using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlogPosts.Application.Services.Abstracts;
using BlogPosts.Application.Services.Requests;
using BlogPosts.Application.Services.Responses;
using BlogPosts.Authentication.Configuration;
using BlogPosts.Authentication.Models.DTOs.Requests;
using BlogPosts.Authentication.Models.DTOs.Responses;
using BlogPosts.Domain.Entities;
using BlogPosts.Infrastructure.EFCore.Databases;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BlogPosts.Application.Services.Concretes
{
    public class AuthService:IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtConfig _jwtConfig;
        private readonly ApplicationDbContext _dbContext;
        private readonly TokenValidationParameters _tokenValidationParams;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IOptionsMonitor<JwtConfig> optionsMonitor, ApplicationDbContext dbContext, TokenValidationParameters tokenValidationParams)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _dbContext = dbContext;
            _tokenValidationParams = tokenValidationParams;
        }

        public async Task<UserLoginResponse> Login(UserLoginRequest userLoginRequest)
        {
            var user = await _userManager.FindByNameAsync(userLoginRequest.Username);

            var message = "";

            if (user != null)
            {
                var result = await _userManager.CheckPasswordAsync(user, userLoginRequest.Password);

                if (result)
                {
                    var jwtToken = await GenerateJwtToken(user);

                    return await Task.FromResult(new UserLoginResponse()
                    {
                        Token = jwtToken.Token,
                        RefreshToken = jwtToken.RefreshToken,
                        Status = true,
                        Message = "Login is successfully"
                    });
                }

                message = "Password is wrong";
            }

            message = "There is no account for associated username";


            
            return await Task.FromResult(new UserLoginResponse()
            {
                Status = false,
                Message = message
            });
        }

        public async Task<UserRegisterResponse> Register(UserRegisterRequest userRegisterRequest)
        {
            User user = new Author()
            {
                FirstName = userRegisterRequest.FirstName,
                LastName = userRegisterRequest.LastName,
                UserName = userRegisterRequest.Username,
            };

            var result = _userManager.CreateAsync(user, userRegisterRequest.Password);

            UserRegisterResponse resp = null;

            if (result.Result.Succeeded)
            {
                var jwtToken = await GenerateJwtToken(user);

                resp = new UserRegisterResponse()
                {
                    Token = jwtToken,
                    Status = true,
                    Message = "You successfully registered"
                };
            }
            else
            {
                resp = new UserRegisterResponse()
                {
                    Status = false,
                    Message = "Something went wrong"
                };
            }

            return resp;

        }

        

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();

        }

        public async Task<AuthResult> GenerateJwtToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1), // 5-10 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevorked = false,
                UserId = user.Id.ToString(),
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                Token = RandomString(35) + Guid.NewGuid()
            };

            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();

            return new AuthResult()
            {
                Token = jwtToken,
                Success = true,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthResult> VerifyAndGenerateToken(TokenRequest tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Validation 1 - Validation JWT token format
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParams, out var validatedToken);

                // Validation 2 - Validate encryption alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        return null;
                    }
                }

                // Validation 3 - validate expiry date
                var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

                if (expiryDate > DateTime.UtcNow)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token has not yet expired"
                        }
                    };
                }

                // validation 4 - validate existence of the token
                var storedToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

                if (storedToken == null)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token does not exist"
                        }
                    };
                }

                // Validation 5 - validate if used
                if (storedToken.IsUsed)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token has been used"
                        }
                    };
                }

                // Validation 6 - validate if revoked
                if (storedToken.IsRevorked)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token has been revoked"
                        }
                    };
                }

                // Validation 7 - validate the id
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.JwtId != jti)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token doesn't match"
                        }
                    };
                }

                // Validation 8 - validate stored token expiry date
                if (storedToken.ExpiryDate < DateTime.UtcNow)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Refresh token has expired"
                        }
                    };
                }

                // update current token 

                storedToken.IsUsed = true;
                _dbContext.RefreshTokens.Update(storedToken);
                await _dbContext.SaveChangesAsync();

                // Generate a new token
                var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);
                return await GenerateJwtToken(dbUser);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Lifetime validation failed. The token is expired."))
                {

                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token has expired please re-login"
                        }
                    };

                }
                else
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Something went wrong."
                        }
                    };
                }
            }
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

            return dateTimeVal;
        }

        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(x => x[random.Next(x.Length)]).ToArray());
        }
    }
}
