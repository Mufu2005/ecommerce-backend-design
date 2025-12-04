using System.Security.Claims;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ShopHub.Models;
using ShopHub.Services;

namespace YourNamespace.Controllers
{
    public class AccountController : Controller
    {
        private readonly MongoDbService _mongoService;

        public AccountController(MongoDbService mongoService)
        {
            _mongoService = mongoService;
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string name, string email, string password)
        {
            var existingUser = await _mongoService.GetUserByEmailAsync(email);
            if (existingUser != null)
            {
                ViewBag.Error = "Email already registered.";
                return View();
            }

            var newUser = new User
            {
                Name = name,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = "User"
            };

            await _mongoService.CreateUserAsync(newUser);

            return RedirectToAction("Login");
        }

        public IActionResult Login(string returnUrl = null)
        {
            // Save the URL so we can use it after they type their password
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string returnUrl = null)
        {
            var user = await _mongoService.GetUserByEmailAsync(email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                ViewBag.Error = "Invalid email or password";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("FullName", user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, // Must match Program.cs
    new ClaimsPrincipal(claimsIdentity),
    new AuthenticationProperties { IsPersistent = true });

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}