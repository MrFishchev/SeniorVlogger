using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SeniorVlogger.DataAccess.Repository.IRepository;
using SeniorVlogger.Models.DTO;

namespace SeniorVlogger.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public UserController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment, 
            ILogger<UserController> logger, SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]Credentials credentials)
        {
            try
            {
                await CreateUserIfEmpty(credentials.Username, credentials.Password);

                var result = await _signInManager.PasswordSignInAsync(credentials.Username, credentials.Password, 
                    credentials.Remember, false);

                if (result.Succeeded)
                {
                    var user = await _unitOfWork.ApplicationUsers.GetFirstOrDefault(u =>
                        u.UserName == credentials.Username);
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

        private async Task CreateUserIfEmpty(string username, string password)
        {
            var users = await _unitOfWork.ApplicationUsers.GetFirstOrDefault();
            if (users == null)
            {
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
        }

        private string GetRandomToken(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException();

            var jwt = new JwtService(_configuration);
            var token = jwt.GenerateSecurityToken(email);
            return token;
        }

        public class Credentials
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public bool Remember { get; set; }
        }
    }
}
