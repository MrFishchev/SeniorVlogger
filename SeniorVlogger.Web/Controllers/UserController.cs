﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SeniorVlogger.DataAccess.Repository.IRepository;
using SeniorVlogger.Models.DTO;
using SeniorVlogger.Models.Requests;

namespace SeniorVlogger.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        #region Fields

        private readonly ILogger<UserController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtService _jwtService;

        #endregion

        #region Constructor

        public UserController(IUnitOfWork unitOfWork, JwtService jwtService, 
            ILogger<UserController> logger, SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        #endregion

        #region Actions

        [HttpGet("Verify")]
        public async Task<IActionResult> Verify()
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(email)) return NotFound();

            var user = await _unitOfWork.ApplicationUsers
                .GetFirstOrDefault(u => u.Email == email);

            return Json(new
            {
                user = user.UserName,
                isEmailConfirmed = user.EmailConfirmed,
                isSubscribed = false,
            });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]CredentialsRequest credentials)
        {
            try
            {
                await CreateUserIfEmpty(credentials.Username, credentials.Password);

                var user = await _userManager.FindByEmailAsync(credentials.Username);

                if (user == null)
                    return Json(new { success = false, message = "User doesn't exist" });

                var result = await _signInManager.PasswordSignInAsync(user.UserName, credentials.Password,
                    credentials.Remember, false);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"User {credentials.Username} logged in");
                    return Json(new
                    {
                        success = true,
                        user = credentials.Username,
                        isEmailConfirmed = user.EmailConfirmed,
                        isSubscribed = false,
                        token = GetRandomToken(credentials.Username)
                    });
                }

                if (result.IsLockedOut)
                {
                    return Json(new {success = false, message = "User account locked out"});
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Invalid request to {nameof(Login)}");
            }

            return Json(new { success = false, message = "Invalid login attempt" });
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(email)) return NotFound();

            await _signInManager.SignOutAsync();

            _logger.LogInformation($"{email} logged out");
            return Ok();
        }

        #endregion

        #region Private Methods

        private async Task CreateUserIfEmpty(string username, string password)
        {
            var users = await _unitOfWork.ApplicationUsers.GetFirstOrDefault();
            if (users != null) return;

            var appUser = new ApplicationUser
            {
                UserName = username,
                Email = username
            };
            var resultUser = await _userManager.CreateAsync(appUser, password);

            _logger.LogInformation((resultUser.Succeeded)
                ? $"{appUser.UserName} has been created"
                : $"Cannot create user {resultUser.Errors.First().Description}");
        }

        private string GetRandomToken(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException();

            var token = _jwtService.GenerateSecurityToken(email);
            return token;
        }

        #endregion

    }
}
