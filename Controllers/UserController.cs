using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitHub.Data;
using FitHub.Models;
using System.Drawing.Text;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace FitHub.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly GymDbContext _context;

        public UserController(GymDbContext context)
        {
            _context = context;
        }


        // GET: User
        [Authorize(Policy ="RequireAdminRole")]
        public async Task<IActionResult> Index()
        {
            return _context.User != null ?
                        View(await _context.User.ToListAsync()) :
                        Problem("Entity set 'GymDbContext.User'  is null.");
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            string userId = User.FindFirst("UserID").Value;
            var user = await _context.User.FindAsync(userId);
            return View("Details", user);

        }

        [Authorize]
        // GET: User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create([Bind("UserID,Email,FirstName,LastName,PhoneNumber,DOB,Gender,Address,City,Province,Country,PostalCode,Password,ConfirmPassword")] User user)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            if (UserExists(user.Email))
            {
                ModelState.AddModelError("Email", "Email Address already Exists");
                return View(user);
            }
            if (ModelState.IsValid)
            {
                user.Password = HashPassword(user.Password);
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy =("RequireAdminRole"))]
        public async Task<IActionResult> ChangeAdminStatus(string id)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user != null)
            {
                user.IsAdmin = !user.IsAdmin;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", id);
        }



#pragma warning restore CS8604 // Possible null reference argument.

        // GET: User/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            user.ConfirmPassword = user.Password;
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(string id, [Bind("UserID,Email,FirstName,LastName,PhoneNumber,DOB,Gender,Address,City,Province,Country,PostalCode,Password")] User user)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }

            ModelState.Remove("Password");

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.User.FindAsync(id);
                    if (existingUser != null)
                    {
                        existingUser.FirstName = user.FirstName;
                        existingUser.LastName = user.LastName;
                        existingUser.PhoneNumber = user.PhoneNumber;
                        existingUser.DOB = user.DOB;
                        existingUser.Gender = user.Gender;
                        existingUser.Address = user.Address;
                        existingUser.City = user.City;
                        existingUser.Province = user.Province;
                        existingUser.Country = user.Country;
                        existingUser.PostalCode = user.PostalCode;

                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Delete/5
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'GymDbContext.User'  is null.");
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string email)
        {
            return (_context.User?.Any(e => e.Email == email)).GetValueOrDefault();
        }

        [AllowAnonymous]
        private string HashPassword(string password)
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
