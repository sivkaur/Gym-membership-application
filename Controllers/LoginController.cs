//login controller
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitHub.Data;
using FitHub.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace FitHub.Controllers
{
    public class LoginController : Controller
    {
        private readonly GymDbContext _context;

        public LoginController(GymDbContext context)
        {
            _context = context;
        }

        // GET: Login/Create
        [Route("/Login")]
        public IActionResult Create()
        {
            var user = User.FindFirst("UserID")?.Value;
            if (user == null)
            {
                return View("Login");
            }
            return RedirectToAction("Profile", "User");
        }

        // POST: Login/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProceedLogin([Bind("Email,Password")] Login login)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.User
                    .Where(u => u.Email.Equals(login.Email))
                    .FirstOrDefaultAsync();

                if (user == null || HashPassword(login.Password) != user.Password)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                    return View("Login");
                };

                var claims = new List<Claim> {
                    new Claim("UserID", user.UserID),
                    new Claim("Name", user.FirstName + " " + user.LastName),
                    new Claim("Region", user.City + ", " + user.Province + ", " + user.Country),
                    new Claim("status", user.IsAdmin ? "Administrator": "Gym User"),
                    new Claim("Gender", user.Gender)
                };

                if (user.IsAdmin)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                }

                var claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);


                return RedirectToAction("Profile", "User");
            }
            ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            return View(login);
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert the byte array to a hexadecimal string
                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte b in hashedBytes)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }
    }
}
