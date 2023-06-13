using Coursework.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Coursework.Models;

namespace Coursework.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly MyDbContext _context;

        private readonly ILogger<HomeController> _logger;

        public AuthorizationController(MyDbContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        private int GetUserID()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId.HasValue)
            {
                return userId.Value;
            }

            // Return a default or error value if the user ID is not found
            return -1;
        }

        public IActionResult Login()
        {
            return View("/Views/Authorization/Login.cshtml");
        }

        private async Task SignInUserAsync(string userID, string useremail, bool rememberMe)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userID),
                new Claim(ClaimTypes.Name, useremail)
                // Add any additional claims as needed
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = rememberMe
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Store the user ID in the session
            HttpContext.Session.SetInt32("UserID", Convert.ToInt32(userID));
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Perform the necessary login authentication here
            // Example:
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                // Authentication successful
                // Sign in the user
                await SignInUserAsync(user.ID.ToString(), user.Email, false);

                // You can redirect the user to another page or perform additional actions
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Authentication failed
                // Pass an error message to the view
                ViewData["ErrorMessage"] = "Invalid email or password";

                return View();
            }
        }
        public IActionResult Logout()
        {
            return View("/Views/Authorization/Logout.cshtml");
        }
        public async Task<IActionResult> LogoutConfirmed()
        {
            // Clear the user session
            HttpContext.Session.Clear();

            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect the user to the login page
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Registration()
        {
            return View("/Views/Authorization/Registration.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserTable registrationModel)
        {
            if (ModelState.IsValid)
            {
                // Create a new user based on the registration data
                var newUser = new UserTable
                {
                    Name = registrationModel.Name,
                    Surname = registrationModel.Surname,
                    Birthdate = registrationModel.Birthdate,
                    Email = registrationModel.Email,
                    Password = registrationModel.Password
                };

                // Save the new user to the database
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                // Perform any additional actions after successful registration

                // Sign in the user
                await SignInUserAsync(newUser.ID.ToString(), newUser.Email, false);

                // Redirect the user to a success page or any other page as desired
                return RedirectToAction("Index", "Home");
            }

            // If the model state is not valid, return the registration view with error messages
            return View("/Views/Authorization/Registration.cshtml", registrationModel);
        }

    }
}
