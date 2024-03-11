using Api.DTOs.Account;
using API.Error;
using API.Services;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    
    public class AccountController : BaseApiController
    {
        private readonly JWTService _jwtService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
       public AccountController(JWTService jwtService,
         SignInManager<User> signInManager,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            _jwtService = jwtService;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager= userManager;
            
        }
        [Authorize]
        [HttpGet("refresh-user-token")]
        public async Task<ActionResult> RefreshUserToken()
        { 
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.Email)?.Value);
            return Ok(new SucceededRespone(200) { Data = await _jwtService.CreateApplicationUserDto(user) });

        }
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return Unauthorized(new FailResponse(400) { Errors = new string[] { "invalid userName" } });
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
                return Unauthorized(new FailResponse(400) { Errors = new string[] { "Invalid username or password"} });

             return Ok(new SucceededRespone(200) { Data = await _jwtService.CreateApplicationUserDto(user) });
        }
        [HttpGet("AllRoles")]
        public async Task<ActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(new SucceededRespone(200) { Data=roles});
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> register(RegisterDto model)
        {

            if (await CheckExistsAsync(model.Email))
            {
             return BadRequest(new FailResponse(400) { Errors = new string[] { $"An existes account is using {model.Email}, email address, Please try with another email address" } });
            }
            var role = await _roleManager.FindByIdAsync(model.Role);
            if (role == null)
            {
                return BadRequest(new FailResponse(400));

            }
            var userToAdd = new User
            {
                FirstName = model.FirstName.ToLower(),
                LastName = model.LastName.ToLower(),
                UserName = model.Email.ToLower(),
                Email = model.Email.ToLower(), 
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(userToAdd, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new FailResponse(400) { Errors = new string[] { "There are errors in creating User" } }); 
            }
            result = await _userManager.AddToRoleAsync(userToAdd, role.Name);
            if (!result.Succeeded)
            {
               
                return BadRequest(result.Errors);
            }

            return Ok(new SucceededRespone(200) { Data = await _jwtService.CreateApplicationUserDto(userToAdd) });
        }
        #region Check Email Exist 

        private async Task<bool> CheckExistsAsync(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());

        }
        #endregion

    }
}
