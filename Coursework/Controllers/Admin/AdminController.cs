using Coursework.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Coursework.Models;

namespace Coursework.Controllers.Admin
{
    public class AdminController : Controller
    {
        private readonly MyDbContext _context;

        private readonly ILogger<HomeController> _logger;

        public AdminController(MyDbContext context, ILogger<HomeController> logger)
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

        public IActionResult AdminMain()
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
            return View("/Views/Admin/AdminMain.cshtml");
        }
    }
}
