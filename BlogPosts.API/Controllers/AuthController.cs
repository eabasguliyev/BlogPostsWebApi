﻿using System.Threading.Tasks;
using BlogPosts.Application.Services.Abstracts;
using BlogPosts.Application.Services.Requests;
using BlogPosts.Application.Services.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogPosts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _userService;

        public AuthController(IAuthService authService)
        {
            _userService = authService;
        }

        [HttpPost("Signin")]
        public async Task<UserLoginResponse> Signin(UserLoginRequest req)
        {
            if (!ModelState.IsValid)
            {
                var resp = new UserLoginResponse()
                {
                    Status = false,
                    Message = "Please check inputs",
                };

               
                return await Task.FromResult(resp);
            }

            return await _userService.Login(req);
        }

        [HttpPost("Register")]
        public async Task<UserRegisterResponse> Register(UserRegisterRequest req)
        {
            if (!ModelState.IsValid)
            {
                var resp = new UserRegisterResponse()
                {
                    Status = false,
                    Message = "Please check inputs",
                };

               
                return await Task.FromResult(resp);
            }


            return await _userService.Register(req);
        }
    }
}