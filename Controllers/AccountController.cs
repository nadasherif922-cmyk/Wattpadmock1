using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wattpadmock1.Models;

namespace Wattpadmock1.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password)
        {
            if (_context.Users.Any(u => u.Email == email))
            {
                ViewBag.Error = "Email already exists!";
                return View();
            }
            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                ViewBag.Error = "Invalid email or password!";
                return View();
            }
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Profile()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login");

            var userId = HttpContext.Session.GetInt32("UserId").Value;

            var user = await _context.Users
                .Include(u => u.Stories)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return RedirectToAction("Login");

            return View(user);
        }
    }
}