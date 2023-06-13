using Coursework.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Coursework.Models;

namespace Coursework.Controllers.Admin
{
    public class UsersController : Controller
    {
        private readonly MyDbContext _context;

        private readonly ILogger<HomeController> _logger;

        public UsersController(MyDbContext context, ILogger<HomeController> logger)
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
        public IActionResult ManageUsers()
        {
            // Get the logged-in user's ID
            int userID = GetUserID();
            // Get the logged-in user's Role
            ViewData["Role"] = HomeController.GetRole(userID, _context);
            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {

                // Retrieve the user-specific data based on the user ID
                var userData = _context.Users.Where(u => u.ID == userID).ToList();

                if (userID >= 0)
                {
                    ViewData["Data"] = userData[0];
                }
                else { ViewData["Data"] = null; }
            }
            else
            {
                // Handle the case when the user is not authenticated
                // For example, display a message or redirect to the login page
                ViewData["Data"] = null;
            }
            var data = new UserPageViewModel
            {
                Users = _context.Users.ToList(),
                Roles = _context.Roles.ToList()
            };
            return View("/Views/Admin/ManageUsers/ManageUsers.cshtml", data);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Get the logged-in user's ID
            int userID = GetUserID();
            // Get the logged-in user's Role
            ViewData["Role"] = HomeController.GetRole(userID, _context);
            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {

                // Retrieve the user-specific data based on the user ID
                var userData = _context.Users.Where(u => u.ID == userID).ToList();

                if (userID >= 0)
                {
                    ViewData["Data"] = userData[0];
                }
                else { ViewData["Data"] = null; }
            }
            else
            {
                // Handle the case when the user is not authenticated
                // For example, display a message or redirect to the login page
                ViewData["Data"] = null;
            }
            var user = _context.Users.FirstOrDefault(u => u.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View("/Views/Admin/ManageUsers/EditUser.cshtml", user);
        }

        [HttpPost]
        public IActionResult Edit(UserTable user)
        {
            // Get the logged-in user's ID
            int userID = GetUserID();
            // Get the logged-in user's Role
            ViewData["Role"] = HomeController.GetRole(userID, _context);
            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {

                // Retrieve the user-specific data based on the user ID
                var userData = _context.Users.Where(u => u.ID == userID).ToList();

                if (userID >= 0)
                {
                    ViewData["Data"] = userData[0];
                }
                else { ViewData["Data"] = null; }
            }
            else
            {
                // Handle the case when the user is not authenticated
                // For example, display a message or redirect to the login page
                ViewData["Data"] = null;
            }
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            _context.Users.Update(user);
            _context.SaveChanges();

            return RedirectToAction("ManageUsers");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            // Get the logged-in user's ID
            int userID = GetUserID();
            // Get the logged-in user's Role
            ViewData["Role"] = HomeController.GetRole(userID, _context);
            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {

                // Retrieve the user-specific data based on the user ID
                var userData = _context.Users.Where(u => u.ID == userID).ToList();

                if (userID >= 0)
                {
                    ViewData["Data"] = userData[0];
                }
                else { ViewData["Data"] = null; }
            }
            else
            {
                // Handle the case when the user is not authenticated
                // For example, display a message or redirect to the login page
                ViewData["Data"] = null;
            }
            var user = _context.Users.FirstOrDefault(u => u.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View("/Views/Admin/ManageUsers/DeleteUser.cshtml", user);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return RedirectToAction("ManageUsers");
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            // Get the logged-in user's ID
            int userID = GetUserID();
            // Get the logged-in user's Role
            ViewData["Role"] = HomeController.GetRole(userID, _context);
            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {

                // Retrieve the user-specific data based on the user ID
                var userData = _context.Users.Where(u => u.ID == userID).ToList();

                if (userID >= 0)
                {
                    ViewData["Data"] = userData[0];
                }
                else { ViewData["Data"] = null; }
            }
            else
            {
                // Handle the case when the user is not authenticated
                // For example, display a message or redirect to the login page
                ViewData["Data"] = null;
            }
            var user = _context.Users.FirstOrDefault(u => u.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View("/Views/Admin/ManageUsers/DetailsUser.cshtml", user);
        }
    }
}
