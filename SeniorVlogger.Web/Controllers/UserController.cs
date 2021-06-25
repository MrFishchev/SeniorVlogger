using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SeniorVlogger.Common.Enums;
using SeniorVlogger.DataAccess.Repository.IRepository;
using SeniorVlogger.Models.DTO;
using SeniorVlogger.Models.Requests;
using SeniorVlogger.Models.ViewModels;

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
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public UserController(IUnitOfWork unitOfWork, JwtService jwtService, 
            ILogger<UserController> logger, SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        #endregion

        #region Actions

        [HttpGet("all")]
        public async Task<IEnumerable<UserViewModel>> GetAll()
        {
            var users = await _unitOfWork.ApplicationUsers.GetAll();
            return users?.Select(x => new UserViewModel
            {
                Id = x.Id,
                Email = x.Email,
                EmailConfirmed = x.EmailConfirmed,
                IsLocked = x.LockoutEnabled && (x.LockoutEnd != null),
                Role = Enum.GetName(typeof(Role), x.Role)
            });
        }

        [HttpGet("{id}")]
        public async Task<UserViewModel> GetById(string id)
        {
            var user = await _unitOfWork.ApplicationUsers.GetFirstOrDefault(x => x.Id == id);
            return new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                IsLocked = user.LockoutEnabled && (user.LockoutEnd != null),
                Role = Enum.GetName(typeof(Role), user.Role)
            };
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            var existingUser = await _unitOfWork.ApplicationUsers.GetFirstOrDefault(x => x.Email == request.Email);
            if (existingUser != null)
            {
                _logger.LogInformation($"User {existingUser.Email} already exists");
                return Conflict();
            }
            
            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                _logger.LogWarning($"Unable to create user: {result.Errors.FirstOrDefault()?.Description}");
                return Conflict();
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UpdateUserRequest request)
        {
            var user = await _unitOfWork.ApplicationUsers.GetFirstOrDefault(x => x.Id == id);
            if (user == null)
                return NotFound();

            await _unitOfWork.ApplicationUsers.Update(user);
            var removed = await _userManager.RemovePasswordAsync(user);
            if (!removed.Succeeded)
            {
                _logger.LogWarning($"Cannot change password for {user.Email}");
                return Conflict();
            }

            var added = await _userManager.AddPasswordAsync(user, request.Password);
            if (!added.Succeeded)
            {
                _logger.LogWarning($"Cannot change password for {user.Email}");
                return Conflict();
            }

            await _unitOfWork.Save();

            var currentUserEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserEmail == user.Email) await _signInManager.SignOutAsync();

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _unitOfWork.ApplicationUsers.GetFirstOrDefault(x => x.Id == id);
            if (user == null)
                return NotFound();

            var deleted = await _userManager.DeleteAsync(user);
            if (!deleted.Succeeded)
            {
                _logger.LogWarning($"Cannot delete user {user.Email}");
                return Conflict();
            }
            
            var currentUserEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserEmail == user.Email) await _signInManager.SignOutAsync();

            return Ok();
        }
        
        [HttpGet("verify")]
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

        [HttpPost("login")]
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

        [HttpPost("logout")]
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
