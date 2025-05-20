using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OffersHub.Application.Models.Users;
using OffersHub.Application.Services.Users;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using OffersHub.Web.Models.ViewModels;
using Mapster;

namespace OffersHub.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var user = await _userService.Authenticate(model.UserName, model.Password, cancellationToken);

                HttpContext.Response.Cookies.Append("jwt", user.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)  
                });

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                TempData["WelcomeMessage"] = $"Welcome, {user.UserName}! You are logged in as {user.Role}.";

                return RedirectToAction("Index", "Home");

                //switch (user.Role.ToLower())
                //{
                //    case "admin":
                //        return RedirectToAction("AdminDashboard", "Home");
                //    case "company":
                //        return RedirectToAction("CompanyDashboard", "Home");
                //    case "client":
                //        return RedirectToAction("ClientDashboard", "Home");
                //    default:
                //        return RedirectToAction("Index", "Home");
                //}
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _userService.Create(model.Adapt<UserRegisterModel>(), cancellationToken);

                var role = model.Role;
 
                if (model.Role  == "Client")
                {
                    return RedirectToAction("CreateClient", "Client"); 
                }
                else if (model.Role == "Company")
                {
                    return RedirectToAction("CreateForm", "Company"); 
                }

                return RedirectToAction("Login");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Response.Cookies.Delete("jwt"); 
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }


}
