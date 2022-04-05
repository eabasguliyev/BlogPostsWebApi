﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogPosts.Application.Services.Abstracts;
using BlogPosts.Application.Services.Requests;
using BlogPosts.Application.Services.Responses;
using BlogPosts.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlogPosts.Application.Services.Concretes
{
    public class UserService:IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<UserLoginResponse> Login(UserLoginRequest userLoginRequest)
        {
            var user = await _userManager.FindByNameAsync(userLoginRequest.Username);

            var message = "";

            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, userLoginRequest.Password, false);

                if (result.Succeeded)
                {
                    return await Task.FromResult(new UserLoginResponse()
                    {
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

        public Task<UserRegisterResponse> Register(UserRegisterRequest userRegisterRequest)
        {
            var result = _userManager.CreateAsync(new Author()
            {
                FirstName = userRegisterRequest.FirstName,
                LastName = userRegisterRequest.LastName,
                UserName = userRegisterRequest.Username,
            }, userRegisterRequest.Password);

            UserRegisterResponse resp = null;

            if (result.Result.Succeeded)
            {
                resp = new UserRegisterResponse()
                {
                    Status = true,
                    Message = "User successfully registered"
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

            return Task.FromResult(resp);

        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();

        }
    }
}