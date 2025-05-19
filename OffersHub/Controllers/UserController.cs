using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OffersHub.API.Infrastructure.Authentication.JWT;
using OffersHub.Application.Models.Users;
using OffersHub.Application.Services.Users;
using System.IdentityModel.Tokens.Jwt;

namespace OffersHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOptions<JWTConfiguration> _options;

        public UserController(IUserService userService, IOptions<JWTConfiguration> options)
        {
            _userService = userService;
            _options = options;
        }

        [Route("register")]
        [HttpPost]
        public async Task<string> Register(UserRegisterModel user, CancellationToken cancellation = default)
        {
            return await _userService.Create(user, cancellation);
        }

        [Route("login")]
        [HttpPost]
        public async Task<string> LogIn(UserLoginModel user, CancellationToken cancellation = default)
        {
            var result = await _userService.Authenticate(user.UserName, user.Password, cancellation);
            return JWTGenerator.GenerateSecurityToken(user.UserName, result.Role, _options);
        }

    }
}
