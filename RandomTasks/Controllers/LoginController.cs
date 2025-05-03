using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using RandomTasks.Models;
using System.Security.Claims;
using RandomTasks.Data;
using Microsoft.EntityFrameworkCore;

namespace RandomTasks.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Login/Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Login/Index
        [HttpPost]
        public async Task<IActionResult> Index(UserLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Fetch the user from the database
            var user = await _context.Users.SingleOrDefaultAsync(u =>
                u.Username == model.Username && u.Password == model.Password);
            if (user == null)
            {
                // ❗ Add a manual model-level error
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(model);
            }

            if (user != null)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                // Store the role as a string value
                new Claim(ClaimTypes.Role, user.RoleId.ToString())
            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Role-based redirection
                if (user.RoleId == 1)
                    return RedirectToAction("Index", "Admin");
                if (user.RoleId == 2)
                    return RedirectToAction("Index", "Customer");
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View(model);
        }

        // GET: /Login/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Login/Register
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check if the username already exists
            var existingUser = await _context.Users.AnyAsync(u => u.Username == model.Username);
            if (existingUser)
            {
                ModelState.AddModelError("", "Username already exists.");
                return View(model);
            }

            var newUser = new User
            {
                Username = model.Username,
                Password = model.Password,
                RoleId = model.RoleId
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Registration successful. Please log in.";
            return RedirectToAction("Index");
        }

        // GET: /Login/Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
    }
}
