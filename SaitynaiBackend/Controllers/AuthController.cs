using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SaitynaiBackend.Auth;
using SaitynaiBackend.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace SaitynaiBackend.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<StoreUser> _userManager;
        private readonly JwtService _jwtTokenService;
        private readonly HttpContext _httpContext;

        public record RegisterUserDto([Required] string UserName, [EmailAddress][Required] string Email, [Required] string Password);
        public record LoginDto(string UserName, string Password);

        public record UserDto(string Id, string UserName, string Email);

        public record SuccessfulLoginDto(string AccessToken);

        public AuthController(UserManager<StoreUser> userManager, JwtService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<RegisterUserDto>> Register(RegisterUserDto registerUserDto)
        {
            var user = await _userManager.FindByNameAsync(registerUserDto.UserName);
            if (user != null)
                return BadRequest("User already exists.");

            var newUser = new StoreUser
            {
                Email = registerUserDto.Email,
                UserName = registerUserDto.UserName,
            };

            var createUserResult = await _userManager.CreateAsync(newUser, registerUserDto.Password);
            if (!createUserResult.Succeeded)
                return BadRequest("could not create a user.");

            await _userManager.AddToRoleAsync(newUser, UserStoreRoles.User);

            return CreatedAtAction(nameof(Register), new UserDto(newUser.Id, newUser.UserName, newUser.Email));
        }
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<SuccessfulLoginDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null)
                return BadRequest("User Name or password is invalid.");
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
                return BadRequest("User name or passsword is invalid.");

            await _userManager.UpdateAsync(user);
            //valid user
            //(generate token)
            var roles = await _userManager.GetRolesAsync(user);

            var accessToken = _jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);
            var refreshToken = _jwtTokenService.CreateRefreshToken(user.Id);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(1),
                Secure = false

            };
            HttpContext.Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);
            return Ok(new SuccessfulLoginDto(accessToken));
        }
        [HttpPost]
        [Route("accessToken")]
        public async Task<ActionResult<SuccessfulLoginDto>> AccessToken()
        {
            if (!HttpContext.Request.Cookies.TryGetValue("RefreshToken",out var refreshToken))
            {
                return UnprocessableEntity();
            }
            if (!_jwtTokenService.TryParseRefreshToken(refreshToken, out var claims))
            {
                return UnprocessableEntity();
            }
            var userId = claims.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return UnprocessableEntity("Invalid token");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var accessToken = _jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);
            var newRefreshToken = _jwtTokenService.CreateRefreshToken(user.Id);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(1),
                Secure = false

            };
            HttpContext.Response.Cookies.Append("RefreshToken", newRefreshToken, cookieOptions);

            return Ok(new SuccessfulLoginDto(accessToken));
        }
    }
}
